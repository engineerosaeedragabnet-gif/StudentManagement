using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StudentManagement.TagHelpers
{
    // الاستخدام: <a sort-column="name" current-sort="@Model.SortOrder">الاسم</a>
    [HtmlTargetElement("a", Attributes = "sort-column")]
    public class SortLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public SortLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        public string SortColumn { get; set; } = string.Empty;   // sort-column
        public string? CurrentSort { get; set; }                  // current-sort
        public string ActionName { get; set; } = "Index";         // action-name

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            bool isAscending = CurrentSort == SortColumn;
            bool isDescending = CurrentSort == SortColumn + "_desc";

            // الضغطة الأولى = تصاعدي، الضغطة الثانية = تنازلي
            string nextSort = isAscending ? SortColumn + "_desc" : SortColumn;

            var values = RouteValuesHelper.FromCurrentQuery(ViewContext.HttpContext.Request);
            values["sortOrder"] = nextSort;
            values.Remove("page");                                // نرجع للصفحة الأولى عند تغيير الترتيب

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.Attributes.SetAttribute("href", urlHelper.Action(ActionName, values));

            // نحافظ على النص الأصلي ونضيف سهم الاتجاه
            var childContent = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(childContent);

            if (isAscending)
            {
                output.Attributes.SetAttribute("title", "مرتب تصاعديًا، اضغط للعكس");
                output.Content.AppendHtml("<span class=\"sort-arrow\" aria-hidden=\"true\">▲</span>");
            }
            else if (isDescending)
            {
                output.Attributes.SetAttribute("title", "مرتب تنازليًا، اضغط للعكس");
                output.Content.AppendHtml("<span class=\"sort-arrow\" aria-hidden=\"true\">▼</span>");
            }
            else
            {
                output.Attributes.SetAttribute("title", "اضغط للترتيب");
            }

            // نشيل الـ attributes الخاصة بينا من الـ HTML النهائي
            output.Attributes.RemoveAll("sort-column");
            output.Attributes.RemoveAll("current-sort");
        }
    }
}