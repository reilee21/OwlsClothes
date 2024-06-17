using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Owls.DTOs;

namespace Owls.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model-admin2")]

    public class PaginationTagHelper_Admin2 : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PaginationTagHelper_Admin2(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;

        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public Pager? PageModelAdmin2 { get; set; }

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; } = String.Empty;
        public string PageClassNormal { get; set; } = String.Empty;
        public string PageClassSelected { get; set; } = String.Empty;
        public override void Process(TagHelperContext context,
       TagHelperOutput output)
        {
            if (ViewContext != null && PageModelAdmin2 != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                TagBuilder result = new TagBuilder("div");
                for (int i = 1; i <= PageModelAdmin2.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.Attributes["onclick"] = $"loadPage({i})";
                    if (PageClassesEnabled)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModelAdmin2.CurrentPage
                        ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }
                output.Content.AppendHtml(result.InnerHtml);
            }

        }
    }
}
