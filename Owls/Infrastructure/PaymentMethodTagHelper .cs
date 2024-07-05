using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Owls.Infrastructure
{
    [HtmlTargetElement("payment-method")]
    public class PaymentMethodTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("value")]
        public string Value { get; set; }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isChecked = For.ModelExplorer.Model.ToString() == Value;

            output.TagName = "div";
            output.Attributes.SetAttribute("class", "w-100");

            var inputTag = new TagBuilder("input");
            inputTag.Attributes.Add("type", "radio");
            inputTag.Attributes.Add("name", For.Name);
            inputTag.Attributes.Add("id", $"{For.Name}_{Value}");
            inputTag.Attributes.Add("value", Value);
            inputTag.Attributes.Add("style", "display:none");
            if (isChecked)
            {
                inputTag.Attributes.Add("checked", "checked");
            }

            var labelTag = new TagBuilder("label");
            labelTag.Attributes.Add("for", $"{For.Name}_{Value}");
            labelTag.Attributes.Add("class", "paymentlabel" + (isChecked ? " active" : ""));
            labelTag.InnerHtml.Append(Label);

            output.Content.AppendHtml(inputTag);
            output.Content.AppendHtml(labelTag);
        }
    }
}
