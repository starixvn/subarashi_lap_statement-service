using Fanex.Athena.ExtService.Core.Extensions;
using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTest.Services
{
    public class UserManagementServiceTest
    {
        private readonly List<User> users = new List<User> { new User { UserName = "alex", Password = "1234".Sha256() } };
        private readonly Mock<IOptions<List<User>>> _mockUsers;

        public UserManagementServiceTest()
        {
            _mockUsers = new Mock<IOptions<List<User>>>();
            _mockUsers.Setup(x => x.Value).Returns(users);
        }

        [Fact]
        public void IsValidUser_WhenCalled_GenerateHasedPassword()
        {
            //Arrange
            var rawUser = new User
            {
                UserName = "UserName",
                Password = "Password"
            };

            var hashedPassword = rawUser.Password.Sha256();

            _mockUsers.Setup(x => x.Value).Returns(new List<User> { new User { UserName = rawUser.UserName, Password = hashedPassword }  });
            
            var userManagementService = new UserManagementService(_mockUsers.Object);

            //Action
            var result = userManagementService.IsValidUser(rawUser.UserName, rawUser.Password);

            //Assert

            Assert.True(result);
        }

        [Theory]
        [InlineData("alex", "123", false)]
        [InlineData(null, null, false)]
        [InlineData(null, "1234", false)]
        [InlineData("alex", null, false)]
        [InlineData("alex", "1234", true)]
        public void IsValidUser_WhenCalled_ReturnCorrectResult(string userName, string pass, bool expectedResult)
        {
            //Arrange
            var userManagementService = new UserManagementService(_mockUsers.Object);

            //Action
            var result = userManagementService.IsValidUser(userName, pass);

            //Assert

            Assert.Equal(expectedResult, result);
        }
    }
}
