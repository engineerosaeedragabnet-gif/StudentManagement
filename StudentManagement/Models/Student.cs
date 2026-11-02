using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        [Display(Name ="الاسم")]
        public string FullName { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(150)]
        [Display(Name ="البريد الالكتروني")]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        [Display(Name ="تاريخ الميلاد")]
        public DateTime BirthDate { get; set; }
        [Display(Name ="المعدل التراكمي")]
        public decimal Gpa { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الالتحاق")]
        public DateTime EnrollmentDate { get; set; }
        [Display(Name = "القسم")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
    }
}
