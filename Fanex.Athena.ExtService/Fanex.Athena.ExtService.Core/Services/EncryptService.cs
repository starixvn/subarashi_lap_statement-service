using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class EncryptService : IEncryptService
    {
        private const int KeySize = 16;
        private const int BufferSize = 81920; // 80k
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptService(string vector, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Encrypt/Decrypt key is required.", nameof(key));
            }

            if (string.IsNullOrEmpty(vector))
            {
                throw new ArgumentException("Encrypt/Decrypt vector(IV) is required.", nameof(vector));
            }

            _key = Encoding.ASCII.GetBytes(key).Take(KeySize).ToArray();
            _iv = Encoding.ASCII.GetBytes(vector).Take(KeySize).ToArray();
        }

        public string Decrypt(string encryptedText)
        {
            var data = Convert.FromBase64String(encryptedText);

            using (var aes = CreateAes())
            {
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    var result = PerformCryptography(decryptor, data);
                    return Encoding.ASCII.GetString(result);
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
                    return Convert.ToBase64String(result);
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
            var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            return aes;
        }
    }
}
