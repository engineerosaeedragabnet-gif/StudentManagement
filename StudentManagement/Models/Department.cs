using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="اسم القسم مطلوب")]
        [StringLength(80, ErrorMessage = "اسم القسم لا يزيد عن 80 حرف")]
        [Display(Name = "اسم القسم")]
        public string Name { get; set; } = string.Empty;
        public ICollection<Student>Students { get; set; } = new List<Student>();

    }
}
