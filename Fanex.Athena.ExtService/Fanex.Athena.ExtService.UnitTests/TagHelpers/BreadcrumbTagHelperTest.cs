using Fanex.Athena.ExtService.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.ExtService.UnitTests.TagHelpers
{
    public class BreadcrumbTagHelperTest
    {
        [Fact]
        public void BreadcrumbTagHelper_WhenCall_ReturnCorrectOutput()
        {
            // Arrange
            const string TagContent = "";
            var mockTagHelperContent = new Mock<TagHelperContent>();
            mockTagHelperContent.Setup(s => s.GetContent()).Returns(TagContent);

            var tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), string.Empty);

            var tagHelperOutput = new TagHelperOutput(string.Empty, new TagHelperAttributeList(),
                (a, b) => Task.FromResult<TagHelperContent>(mockTagHelperContent.Object))
            {
                Content = mockTagHelperContent.Object
            };

            var tagHelperTarget = new BreadcrumbTagHelper();
            tagHelperTarget.Items = new List<SelectListItem>();

            // Action
            tagHelperTarget.Process(tagHelperContext, tagHelperOutput);

            // Assert
            Assert.Equal("div", tagHelperOutput.TagName);
            Assert.Equal("class", tagHelperOutput.Attributes[0].Name);
            Assert.Equal("breadcrumb", tagHelperOutput.Attributes[0].Value);
        }
    }
}
