using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class EncryptService : IEncryptService
    {
        public string Decrypt(string encryptedText)
        {
            return encryptedText;
        }

        public string Encrypt(string plaintext)
        {
            return plaintext;
        }
    }
}
