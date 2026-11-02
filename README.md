# 🎓 Student Management - إدارة الطلاب والأقسام

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap&logoColor=white)

تطبيق ويب لإدارة الطلاب والأقسام مع **Pagination** و**ترتيب** و**بحث وفلترة**، مبني بـ **ASP.NET Core MVC** و **Entity Framework Core** و **SQL Server**. المشروع يركّز على **ViewModels** و**Tag Helpers المخصصة**.

> A student management web app built with ASP.NET Core MVC and EF Core, featuring ViewModels, server-side paging, sorting, filtering and custom Tag Helpers, with an Arabic RTL interface and dark mode.

---

## ✨ المميزات

- 🎓 CRUD كامل للطلاب والأقسام
- 📄 **Pagination** من جهة السيرفر (`Skip/Take`): الداتابيز بترجّع الصفحة المطلوبة فقط
- ↕️ **ترتيب** بالضغط على عنوان العمود (تصاعدي / تنازلي) مع سهم يوضح الاتجاه
- 🔎 بحث بالاسم أو الإيميل وفلترة بالقسم، والبحث والترتيب والصفحة محفوظين مع بعض في الرابط
- 🧩 **ViewModels** منفصلة للعرض والفورم بدل التعامل مع الـ Entities مباشرة
- 🏷️ **Tag Helpers مخصصة**: `<pager>` و `<a sort-column>`
- ✅ Validation على مستويين: Data Annotations + `IValidatableObject` + فحوصات الداتابيز (تكرار الإيميل)
- 🛡️ منع حذف قسم فيه طلاب (`DeleteBehavior.Restrict` + رسالة واضحة)
- 🌙 وضع داكن يتحفظ في المتصفح
- 🌐 واجهة عربية RTL متجاوبة مع الموبايل
- 🔒 حماية من CSRF و Over-posting

## 🛠️ التقنيات المستخدمة

| التقنية | الاستخدام |
|---|---|
| ASP.NET Core MVC | هيكل التطبيق |
| Entity Framework Core | Code First + Migrations + Seed Data |
| SQL Server Express | قاعدة البيانات |
| LINQ | الفلترة والترتيب والـ Projection |
| ViewModels | فصل نموذج العرض عن نموذج الداتابيز |
| Custom Tag Helpers | الـ Pager وروابط الترتيب |
| Razor + Partial Views | الصفحات والفورم المشترك |
| Bootstrap 5 RTL | التصميم |

## 🗂️ هيكل المشروع

```
StudentManagement/
├── Controllers/
│   ├── StudentsController.cs        # القائمة (بحث/ترتيب/Paging) + CRUD
│   └── DepartmentsController.cs
├── Data/AppDbContext.cs             # Fluent API + Seed Data
├── Models/                          # Student, Department
├── ViewModels/                      # PaginatedList<T> + ViewModels للشاشات
├── TagHelpers/                      # PagerTagHelper, SortLinkTagHelper
├── Views/
│   ├── Students/                    # Index, Details, Create, Edit, Delete, _StudentForm
│   ├── Departments/
│   └── Shared/_Layout.cshtml
├── Migrations/
├── wwwroot/                         # CSS / JS
└── Program.cs
```

## 🚀 طريقة التشغيل

### المتطلبات
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) أو أحدث
- SQL Server Express
- أداة EF Core: `dotnet tool install --global dotnet-ef`

### الخطوات

```bash
git clone https://github.com/username/StudentManagement.git
cd StudentManagement
```

عدّل الـ Connection String في `appsettings.json` حسب جهازك:

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=StudentManagement_DB;Trusted_Connection=True;TrustServerCertificate=True"
```

ثم:

```bash
dotnet ef database update
dotnet run
```

هتلاقي **45 طالب** و**4 أقسام** تجريبية جاهزة (5 صفحات بحجم صفحة 10).

## 🔗 أمثلة روابط

```
/Students
/Students?sortOrder=gpa_desc
/Students?departmentId=2&page=2
/Students?searchTerm=محمد&sortOrder=date
```

## 🔍 كيف يعمل الـ Paging والترتيب

الاستعلام بيتبني خطوة خطوة على `IQueryable` وبيتنفذ مرة واحدة بس:

```csharp
IQueryable<Student> query = _context.Students.AsNoTracking();

if (!string.IsNullOrWhiteSpace(searchTerm))
    query = query.Where(s => s.FullName.Contains(searchTerm) || s.Email.Contains(searchTerm));

if (departmentId.HasValue)
    query = query.Where(s => s.DepartmentId == departmentId.Value);

IOrderedQueryable<Student> ordered = sortOrder switch
{
    "gpa"      => query.OrderBy(s => s.Gpa),
    "gpa_desc" => query.OrderByDescending(s => s.Gpa),
    _          => query.OrderBy(s => s.FullName)
};

var projected = ordered.ThenBy(s => s.Id)      // ترتيب ثابت يمنع تكرار الصفوف بين الصفحات
                       .Select(s => new StudentListItemViewModel { /* ... */ });

var page = await PaginatedList<StudentListItemViewModel>.CreateAsync(projected, pageIndex, 10);
```

وفي الـ View:

```html
<th><a sort-column="gpa" current-sort="@Model.SortOrder">المعدل</a></th>
...
<pager page-index="@Model.Students.PageIndex" total-pages="@Model.Students.TotalPages"></pager>
```

## 🎯 ما تعلمته من المشروع

- ViewModels والتحويل بين Entity و ViewModel
- Paging من جهة الداتابيز بـ `Skip/Take` ولماذا نحتاج `ThenBy(Id)`
- ترتيب آمن بـ `switch` (Whitelist) بدل ترتيب بنص من المستخدم
- كتابة Tag Helpers مخصصة (`TagHelper` و `IUrlHelperFactory` و `TagBuilder`)
- علاقة One-to-Many و `DeleteBehavior.Restrict`
- Partial Views لإعادة استخدام الفورم
- `IValidatableObject` للتحقق المخصص
- Change Tracking في التعديل
