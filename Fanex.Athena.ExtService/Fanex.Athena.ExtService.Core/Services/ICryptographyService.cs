namespace Fanex.Athena.ExtService.Core.Services
{
   public interface ICryptographyService
    {
        string Encrypt(string plaintext);
        string Decrypt(string encryptedText);
    }
}
