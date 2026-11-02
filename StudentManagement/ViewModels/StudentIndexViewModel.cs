using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentManagement.ViewModels
{
    public class StudentIndexViewModel
    {
        public PaginatedList<StudentListItemViewModel> Students { get; set; } = null!;
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public string SortOrder { get; set; } = "name";
        public SelectList Departments { get; set; } = null!;
    }
}
