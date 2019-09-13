using Fanex.Athena.ExtService.Core.Services;
using Xunit;


namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    public class EncryptServiceTest
    {
        [Theory]
        [InlineData("abc-xyz-!@#-123", "test--vector-123-*/+-089as", "test--key--@123.456")]
        public void Encrypt_EncryptedText_DescryptCorrectly(string inputText, string vector, string key)
        {
            //Given
            var service = new CryptographyService(vector, key);

            //When
            var encryptedText = service.Encrypt(inputText);
            var actual = service.Decrypt(encryptedText);

            //Then
            Assert.Equal(inputText, actual);
        }
    }
}
