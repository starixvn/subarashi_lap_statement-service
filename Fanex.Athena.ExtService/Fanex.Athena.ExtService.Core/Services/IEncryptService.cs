using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Services
{
   public interface IEncryptService
    {
        string Encrypt(string plaintext);
        string Decrypt(string encryptedText);
    }
}
