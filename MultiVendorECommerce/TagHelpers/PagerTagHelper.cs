using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MultiVendorECommerce.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PagerTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (TotalPages <= 1) return; // لو صفحة واحدة مش محتاجين باجينيشن

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "nav";
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination justify-content-center");

            // زرار Previous
            ul.InnerHtml.AppendHtml(BuildPageItem(urlHelper, CurrentPage - 1, "Previous", CurrentPage > 1));

            // الأرقام
            for (int i = 1; i <= TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(BuildPageItem(urlHelper, i, i.ToString(), true, i == CurrentPage));
            }

            // زرار Next
            ul.InnerHtml.AppendHtml(BuildPageItem(urlHelper, CurrentPage + 1, "Next", CurrentPage < TotalPages));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder BuildPageItem(IUrlHelper url, int pageNumber, string text, bool enabled, bool active = false)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (!enabled) li.AddCssClass("disabled");
            if (active) li.AddCssClass("active");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            a.InnerHtml.Append(text);

            if (enabled)
            {
                var query = ViewContext.HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
                query["page"] = pageNumber.ToString();

                a.Attributes["href"] = url.Action(ViewContext.RouteData.Values["action"]?.ToString(), query);
            }
            else
            {
                a.Attributes["href"] = "#";
            }

            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
