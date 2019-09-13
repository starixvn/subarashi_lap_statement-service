using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Logging;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class CryptographyService : ICryptographyService
    {
        private const int BufferSize = 81920;

        private readonly byte[] _key;
        private readonly byte[] _iv;
        private readonly ILogger _logger;

        public CryptographyService(IOptions<ApplicationOptions> applicationOptionAccessor, ILogger logger)
        {
            _key = Convert.FromBase64String(applicationOptionAccessor.Value.AesKey);
            _iv = Convert.FromBase64String(applicationOptionAccessor.Value.AesIV);
            _logger = logger;
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return string.Empty;
            }

            try
            {
                var inputData = new StringBuilder(encryptedText);
                inputData.Replace("-", "/").Replace("_", "+");
                var data = Convert.FromBase64String(inputData.ToString());
                using (var crypto = CreateCryptographicObject())
                {
                    using (var decryptor = crypto.CreateDecryptor(crypto.Key, crypto.IV))
                    {
                        var result = PerformCryptography(decryptor, data);
                        return Encoding.UTF8.GetString(result);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("Decrypt error", e);
                return string.Empty;
            }
        }

        public string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                return string.Empty;
            }

            var data = Encoding.UTF8.GetBytes(plaintext);
            using (var crypto = CreateCryptographicObject())
            {
                using (var encryptor = crypto.CreateEncryptor(crypto.Key, crypto.IV))
                {
                    var result = PerformCryptography(encryptor, data);
                    var base64Result = new StringBuilder(Convert.ToBase64String(result, Base64FormattingOptions.None));
                    base64Result.Replace("/", "-").Replace("+", "_");
                    return base64Result.ToString();
                }
            }
        }

        public virtual SymmetricAlgorithm CreateCryptographicObject()
        {
            var aes = Aes.Create("AES");
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }

        private static byte[] PerformCryptography(ICryptoTransform transform, byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(output, transform, CryptoStreamMode.Write))
                {
                    WriteToStream(data, cryptoStream);
                    cryptoStream.FlushFinalBlock();
                }

                return output.ToArray();
            }
        }

        private static void WriteToStream(byte[] data, Stream stream)
        {
            var offset = 0;
            var dataLength = data.Length;

            while (offset < dataLength)
            {
                var count = dataLength - offset;
                var bytesToBeWrite = count < BufferSize ? count : BufferSize;

                stream.Write(data, offset, bytesToBeWrite);
                offset += count;
            }
        }

    }
}
