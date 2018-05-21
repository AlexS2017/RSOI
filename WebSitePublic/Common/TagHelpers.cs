using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSitePublic.Common
{
    [HtmlTargetElement(Attributes = "hide-if")]
    public class HideIfTagHelper : TagHelper
    {
        [HtmlAttributeName("hide-if")]
        public bool HideIf { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (HideIf)
            {
                output.SuppressOutput();
            }
        }
    }

    [HtmlTargetElement(Attributes = "show-if")]
    public class ShowIfTagHelper : TagHelper
    {
        [HtmlAttributeName("show-if")]
        public bool ShowIf { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ShowIf == false)
            {
                output.SuppressOutput();
            }
        }
    }
}
