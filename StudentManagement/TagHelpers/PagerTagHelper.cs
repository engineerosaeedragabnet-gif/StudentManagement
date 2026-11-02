using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StudentManagement.TagHelpers
{
    // الاستخدام: <pager page-index="2" total-pages="5" />
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PagerTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        public int PageIndex { get; set; }          // page-index
        public int TotalPages { get; set; }         // total-pages
        public int MaxLinks { get; set; } = 5;      // max-links
        public string ActionName { get; set; } = "Index";   // action-name

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // صفحة واحدة أو أقل: مفيش داعي للـ Pager
            if (TotalPages <= 1)
            {
                output.SuppressOutput();
                return;
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            string BuildUrl(int page)
            {
                var values = RouteValuesHelper.FromCurrentQuery(ViewContext.HttpContext.Request);
                values["page"] = page;
                return urlHelper.Action(ActionName, values) ?? "#";
            }

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("aria-label", "التنقل بين الصفحات");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination justify-content-center mb-0");

            // زر السابق
            bool isFirst = PageIndex <= 1;
            ul.InnerHtml.AppendHtml(CreateItem("السابق", isFirst ? "#" : BuildUrl(PageIndex - 1), disabled: isFirst));

            // نافذة أرقام الصفحات (مثلاً 3 4 [5] 6 7)
            int start = Math.Max(1, PageIndex - MaxLinks / 2);
            int end = Math.Min(TotalPages, start + MaxLinks - 1);
            start = Math.Max(1, end - MaxLinks + 1);

            for (int page = start; page <= end; page++)
            {
                ul.InnerHtml.AppendHtml(CreateItem(page.ToString(), BuildUrl(page), active: page == PageIndex));
            }

            // زر التالي
            bool isLast = PageIndex >= TotalPages;
            ul.InnerHtml.AppendHtml(CreateItem("التالي", isLast ? "#" : BuildUrl(PageIndex + 1), disabled: isLast));

            output.Content.AppendHtml(ul);
        }

        private static TagBuilder CreateItem(string text, string url, bool active = false, bool disabled = false)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (active) li.AddCssClass("active");
            if (disabled) li.AddCssClass("disabled");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            a.Attributes["href"] = url;
            if (active) a.Attributes["aria-current"] = "page";
            if (disabled)
            {
                a.Attributes["tabindex"] = "-1";
                a.Attributes["aria-disabled"] = "true";
            }
            a.InnerHtml.Append(text);

            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}