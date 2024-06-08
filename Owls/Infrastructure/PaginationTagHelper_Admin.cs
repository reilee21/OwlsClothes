using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Owls.DTOs;

namespace Owls.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model-admin")]
    public class PaginationTagHelper_Admin : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PaginationTagHelper_Admin(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;

        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public Pager? PageModelAdmin { get; set; }

        public string? PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; } = String.Empty;
        public string PageClassNormal { get; set; } = String.Empty;
        public string PageClassSelected { get; set; } = String.Empty;
        public string PageSearch{ get; set; } = String.Empty;
        public int? PageCate { get; set; }
        public override void Process(TagHelperContext context,
        TagHelperOutput output)
        {
            if (ViewContext != null && PageModelAdmin != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                TagBuilder result = new TagBuilder("div");
                for (int i = 1; i <= PageModelAdmin.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new
                    {
                        Area="Admin",
                        page = i,
                        cate=PageCate,
                        search=PageSearch,
                    });

                    if (PageClassesEnabled)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModelAdmin.CurrentPage
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
