namespace Fanex.Athena.ExtService.UnitTests.TagHelpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ExtService.TagHelpers;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Moq;
    using Xunit;

    public class PagerTagHelperTest
    {
        [Theory]
        [InlineData(0, 10)]
        [InlineData(11, 10)]
        public void PagerTagHelper_WhenCall_ReturnCorrectOutput(int totalItems, int pageSize)
        {
            // Arrange
            const string TagContent = "";
            var mockTagHelperContent = new Mock<TagHelperContent>();
            mockTagHelperContent.Setup(s => s.GetContent()).Returns(TagContent);

            var tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), string.Empty);

            var tagHelperOutput = new TagHelperOutput(string.Empty, new TagHelperAttributeList(),
                (a, b) => Task.FromResult(mockTagHelperContent.Object))
            {
                Content = mockTagHelperContent.Object
            };

            var tagHelperTarget = new PagerTagHelper();
            tagHelperTarget.TotalItems = totalItems;
            tagHelperTarget.PageIndex = 1;
            tagHelperTarget.PageSize = pageSize;
            tagHelperTarget.UrlTemplate = "";

            // Action
            tagHelperTarget.Process(tagHelperContext, tagHelperOutput);

            // Assert
            Assert.Equal("div", tagHelperOutput.TagName);
            Assert.Equal("class", tagHelperOutput.Attributes[0].Name);
            Assert.Equal("pagingContainer center-align ", tagHelperOutput.Attributes[0].Value);
        }
    }
}