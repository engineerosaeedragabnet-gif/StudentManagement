using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    public class StudentFormViewModel:IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم لا يزيد عن 100 حرف")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(150)]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        // nullable عشان [Required] تشتغل (DateTime العادي قيمته الافتراضية 0001-01-01)
        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "المعدل مطلوب")]
        [Range(0.0, 4.0, ErrorMessage = "المعدل بين 0 و 4")]
        [Display(Name = "المعدل التراكمي")]
        public decimal? Gpa { get; set; }

        [Required(ErrorMessage = "اختر القسم")]
        [Display(Name = "القسم")]
        public int? DepartmentId { get; set; }

        // للعرض فقط (القائمة المنسدلة)، مش بتتحقق منها
        [ValidateNever]
        public SelectList? Departments { get; set; }

        // تحقق مخصص على مستوى الكائن كله
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
            {
                yield return new ValidationResult(
                    "تاريخ الميلاد لا يمكن أن يكون في المستقبل",
                    new[] { nameof(BirthDate) });
            }
        }
    }
}
