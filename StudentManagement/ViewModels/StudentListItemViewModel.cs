namespace StudentManagement.ViewModels
{
    public class StudentListItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public decimal Gpa { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
