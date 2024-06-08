using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Owls.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Owls.Infrastructure
{

	[HtmlTargetElement("div", Attributes = "page-model")]
	public class PaginationTagHelper : TagHelper
	{
		private IUrlHelperFactory urlHelperFactory;

		public PaginationTagHelper(IUrlHelperFactory helperFactory)
		{
			urlHelperFactory = helperFactory;

		}
		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext? ViewContext { get; set; }
		public Pager? PageModel { get; set; }
        public Filter? PageFilter { get; set; }

        public string? PageAction { get; set; }
		public bool PageClassesEnabled { get; set; } = false;
		public string PageClass { get; set; } = String.Empty;
		public string PageClassNormal { get; set; } = String.Empty;
		public string PageClassSelected { get; set; } = String.Empty;
		public string PageQuery { get; set; } = String.Empty;

		public override void Process(TagHelperContext context,TagHelperOutput output)
		{
			if (ViewContext != null && PageModel != null)
			{
				IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
				TagBuilder result = new TagBuilder("div");
				for (int i = 1; i <= PageModel.TotalPages; i++)
				{
					TagBuilder tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i, query = PageQuery, 
						sortOrder= PageFilter?.SortOrder,priceRange = PageFilter?.PriceRange,colorId = PageFilter?.ColorId });

                    if (PageClassesEnabled)
					{
						tag.AddCssClass(PageClass);
						tag.AddCssClass(i == PageModel.CurrentPage
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
