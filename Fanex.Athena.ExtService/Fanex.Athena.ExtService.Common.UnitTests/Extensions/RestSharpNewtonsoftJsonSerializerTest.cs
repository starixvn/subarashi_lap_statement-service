using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Common.Extensions;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Extensions
{
    public class RestSharpNewtonsoftJsonSerializerTest
    {
        private readonly Mock<JsonSerializer> _mockSerializer;
        public RestSharpNewtonsoftJsonSerializerTest()
        {
            _mockSerializer = new Mock<JsonSerializer>();
        }
        [Fact]
        public void Serialize_WhenCall_SerializeWithCorrectSetting()
        {
            //Arrange
            var value = new { Id = 1, Name = "Tes" };

            const Newtonsoft.Json.DateTimeZoneHandling actualTimeZoneHandling = DateTimeZoneHandling.Local;
            //Action
            var customSerializer = new RestSharpNewtonsoftJsonSerializer(_mockSerializer.Object)
            {
                TimeZoneHandling = actualTimeZoneHandling
            };
            customSerializer.Serialize(value);
            //Assert
            _mockSerializer.VerifySet(x => x.DateTimeZoneHandling = actualTimeZoneHandling);
        }

        [Fact]
        public void Deserialize_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var value = JsonConvert.SerializeObject(new { Id = 1, Name = "Tes" });

            var mockResponse = new Mock<RestSharp.IRestResponse>();
            mockResponse.Setup(x => x.Content).Returns(value);
            //Action
            var customSerializer = new RestSharpNewtonsoftJsonSerializer(_mockSerializer.Object)
            {
            };
            var result = customSerializer.Deserialize<dynamic>(mockResponse.Object);
            //Assert
            var actualResult = Assert.IsAssignableFrom<dynamic>(result);

            Assert.Equal(value, JsonConvert.SerializeObject(actualResult));
        }
    }
}
