using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;


namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    public class CryptographyServiceTest
    {
        [Theory]
        [InlineData("ascii-abc-xyz-!@#-123", "YW5zaHRnZGx0b2dqZGJjZw==", "YW5zaHRnZGx0b2dqZGJjZ2Fuc2h0Z2RsdG9namRiY2c=")]
        [InlineData("unicode-a â ấ ầ ẩ ậ ẫ o ơ ở ợ ờ ớ ỡ", "QW5zaHRnZGw1NDhqZGJjZw==", "WG5zaHRnZGw1NDhqZGJjZ0Fuc2h0Z2RsNTQ4amRiY2c=")]
        public void Encrypt_EncryptedText_DescryptCorrectly(string inputText, string vector, string key)
        {
            //Given
            var mockOption = new Mock<IOptions<ApplicationOptions>>();
            mockOption.SetupGet(r => r.Value).Returns( new ApplicationOptions
            {
                AesIV = vector,
                AesKey = key
            });

            var service = new CryptographyService(mockOption.Object);

            //When
            var encryptedText = service.Encrypt(inputText);
            var actual = service.Decrypt(encryptedText);

            //Then
            Assert.Equal(inputText, actual);
        }
    }
}
