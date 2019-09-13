using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Fanex.Athena.ExtService.Core.Models.DTO;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class CryptographyService : ICryptographyService
    {
        private const int BufferSize = 81920;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public CryptographyService(IOptions<ApplicationOptions> applicationOptionAccessor)
        {
            _key = Convert.FromBase64String(applicationOptionAccessor.Value.AesKey);
            _iv = Convert.FromBase64String(applicationOptionAccessor.Value.AesIV);
        }

        public string Decrypt(string encryptedText)
        {
            var inputData = new StringBuilder(encryptedText);
            inputData.Replace("-", "/").Replace("_", "+");
            var data = Convert.FromBase64String(inputData.ToString());
            using (var aes = CreateAes())
            {
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    var result = PerformCryptography(decryptor, data);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

        public string Encrypt(string plaintext)
        {
            var data = Encoding.ASCII.GetBytes(plaintext);
            using (var aes = CreateAes())
            {
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    var result = PerformCryptography(encryptor, data);
                    var base64Result = new StringBuilder(Convert.ToBase64String(result, Base64FormattingOptions.None));
                    base64Result.Replace("/", "-").Replace("+", "_");
                    return base64Result.ToString();
                }
            }
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

        private SymmetricAlgorithm CreateAes()
        {
            var aes = Aes.Create("AES");
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }
    }
}
