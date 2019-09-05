using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Fanex.Athena.ExtService.Common.TagHelpers
{
    public class MoneyTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isNumeric = Decimal.TryParse(output.Content.GetContent(), out decimal number);
            output.TagName = "span";
            if (isNumeric)
            {
                output.Attributes.SetAttribute("class", number >= 0 ? "bl-underdog" : "bl-favorite");
                output.Content.SetContent($"{number:#,##0.00}");
            }


        }
    }
}
