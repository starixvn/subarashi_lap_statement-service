using System.Security.Cryptography;
using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;


namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    public class CryptographyServiceTest
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly Mock<IOptions<ApplicationOptions>> _mockOption;
        private readonly Mock<ILogger> _mockLogger;

        public CryptographyServiceTest()
        {
            _applicationOptions = new ApplicationOptions { AesIV = "", AesKey = "" };
            _mockLogger = new Mock<ILogger>();
            _mockOption = new Mock<IOptions<ApplicationOptions>>();
            _mockOption.SetupGet(r => r.Value).Returns(_applicationOptions);
        }

        [Theory]
        [InlineData("ascii-abc-xyz-!@#-123", "YW5zaHRnZGx0b2dqZGJjZw==", "YW5zaHRnZGx0b2dqZGJjZ2Fuc2h0Z2RsdG9namRiY2c=")]
        [InlineData("unicode-a â ấ ầ ẩ ậ ẫ o ơ ở ợ ờ ớ ỡ", "QW5zaHRnZGw1NDhqZGJjZw==", "WG5zaHRnZGw1NDhqZGJjZ0Fuc2h0Z2RsNTQ4amRiY2c=")]
        public void Encrypt_EncryptedText_DescryptCorrectly(string inputText, string vector, string key)
        {
            //Given
            _applicationOptions.AesIV = vector;
            _applicationOptions.AesKey = key;

            var service = new CryptographyService(_mockOption.Object, _mockLogger.Object);

            //When
            var encryptedText = service.Encrypt(inputText);
            var actual = service.Decrypt(encryptedText);

            //Then
            Assert.Equal(inputText, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Encrypt_NullOrEmpty_ReturnEmpty(string input)
        {
            //Given
            var service = new CryptographyService(_mockOption.Object, _mockLogger.Object);

            //When
            var actual = service.Encrypt(input);

            //Then
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Decrypt_NullOrEmpty_ReturnEmpty(string encryptedText)
        {
            //Given
            var service = new CryptographyService(_mockOption.Object, _mockLogger.Object);

            //When
            var actual = service.Decrypt(encryptedText);

            //Then
            Assert.Empty(actual);
        }

        [Fact]
        public void Decrypt_Exception_ReturnEmpty()
        {
            //Given
            var mockCrypto = new Mock<SymmetricAlgorithm>() { CallBase = false };
            var mockService = new Mock<CryptographyService>(_mockOption.Object, _mockLogger.Object) { CallBase = false };

            mockService.Setup(r => r.CreateCryptographicObject()).Returns(mockCrypto.Object);
            mockCrypto.Setup(r => r.CreateDecryptor(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Throws(new CryptographicException());

            //When
            var actual = mockService.Object.Decrypt("YW5zaHRnZGx0b2dqZGJjZw==");

            //Then
            Assert.Empty(actual);
        }
    }
}
