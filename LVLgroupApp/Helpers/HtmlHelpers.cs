using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Threading.Tasks;

namespace LVLgroupApp.Helpers
{
    public static class HtmlHelpers
    {

        //---------------------------------------------------------------------------------------------------


        public static IHtmlContent Partial(this IHtmlHelper htmlHelper, string partialViewName, object model, string prefix)
        {
            var viewData = new ViewDataDictionary(htmlHelper.ViewData);
            var htmlPrefix = viewData.TemplateInfo.HtmlFieldPrefix;
            viewData.TemplateInfo.HtmlFieldPrefix += !Equals(htmlPrefix, string.Empty) ? $".{prefix}" : prefix;
            return htmlHelper.Partial(partialViewName, model, viewData);
        }


        //---------------------------------------------------------------------------------------------------


        public static Task<IHtmlContent> PartialAsync(this IHtmlHelper htmlHelper, string partialViewName, object model, string prefix)
        {
            var viewData = new ViewDataDictionary(htmlHelper.ViewData);
            var htmlPrefix = viewData.TemplateInfo.HtmlFieldPrefix;
            viewData.TemplateInfo.HtmlFieldPrefix += !Equals(htmlPrefix, string.Empty) ? $".{prefix}" : prefix;
            return htmlHelper.PartialAsync(partialViewName, model, viewData);
        }


        //---------------------------------------------------------------------------------------------------

    }


    [HtmlTargetElement("input", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
    public class DefaultDateTimeTagHelper : TagHelper
    {
        private const string ValueAttribute = "value";
        private DateTime DefaultDateTime = new DateTime(1900, 1, 1);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var value = output.Attributes[ValueAttribute].Value;
            if (value != null)
            {
                DateTime dt;
                if (DateTime.TryParse(value.ToString(), out dt)
                    && dt.Date == DefaultDateTime)
                {
                    output.Attributes.SetAttribute(ValueAttribute, "");
                }
            }
        }
    }

}