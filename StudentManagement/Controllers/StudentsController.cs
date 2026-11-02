using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
    public class StudentsController : Controller
    {
        private const int PageSize = 10;
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students?searchTerm=احمد&departmentId=2&sortOrder=gpa_desc&page=3
        public async Task<IActionResult> Index(
            string? searchTerm, int? departmentId, string? sortOrder, int page = 1)
        {
            sortOrder ??= "name";

            // ---- 1) البداية: استعلام لسه ما اتنفذش ----
            IQueryable<Student> query = _context.Students.AsNoTracking();

            // ---- 2) الفلترة ----
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(s => s.FullName.Contains(searchTerm) ||
                                         s.Email.Contains(searchTerm));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(s => s.DepartmentId == departmentId.Value);
            }

            // ---- 3) الترتيب (switch expression) ----
            IOrderedQueryable<Student> ordered = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(s => s.FullName),
                "dept" => query.OrderBy(s => s.Department.Name),
                "dept_desc" => query.OrderByDescending(s => s.Department.Name),
                "gpa" => query.OrderBy(s => s.Gpa),
                "gpa_desc" => query.OrderByDescending(s => s.Gpa),
                "date" => query.OrderBy(s => s.EnrollmentDate),
                "date_desc" => query.OrderByDescending(s => s.EnrollmentDate),
                _ => query.OrderBy(s => s.FullName)
            };

            // ---- 4) الإسقاط (Projection) على الـ ViewModel ----
            // ThenBy(Id) يضمن ترتيب ثابت لو فيه قيم متساوية (مهم للـ Paging)
            var projected = ordered
                .ThenBy(s => s.Id)
                .Select(s => new StudentListItemViewModel
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Email = s.Email,
                    DepartmentName = s.Department.Name,
                    Gpa = s.Gpa,
                    EnrollmentDate = s.EnrollmentDate
                });

            // ---- 5) الـ Paging: هنا بس بيتنفذ الاستعلام ----
            var viewModel = new StudentIndexViewModel
            {
                Students = await PaginatedList<StudentListItemViewModel>
                                .CreateAsync(projected, page, PageSize),
                SearchTerm = searchTerm,
                DepartmentId = departmentId,
                SortOrder = sortOrder,
                Departments = await GetDepartmentsAsync(departmentId)
            };

            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();
            return View(student);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new StudentFormViewModel
            {
                Departments = await GetDepartmentsAsync()
            };
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentFormViewModel viewModel)
        {
            await ValidateStudentAsync(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Departments = await GetDepartmentsAsync(viewModel.DepartmentId);
                return View(viewModel);
            }

            // تحويل من ViewModel إلى Entity
            var student = new Student
            {
                FullName = viewModel.FullName.Trim(),
                Email = viewModel.Email.Trim(),
                BirthDate = viewModel.BirthDate!.Value,
                Gpa = viewModel.Gpa!.Value,
                DepartmentId = viewModel.DepartmentId!.Value,
                EnrollmentDate = DateTime.Today
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تمت إضافة الطالب بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();

            // تحويل من Entity إلى ViewModel
            var viewModel = new StudentFormViewModel
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                BirthDate = student.BirthDate,
                Gpa = student.Gpa,
                DepartmentId = student.DepartmentId,
                Departments = await GetDepartmentsAsync(student.DepartmentId)
            };
            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentFormViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            await ValidateStudentAsync(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Departments = await GetDepartmentsAsync(viewModel.DepartmentId);
                return View(viewModel);
            }

            // نجيب السجل من الداتابيز (Tracked) ونعدّل الحقول المسموحة بس
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.FullName = viewModel.FullName.Trim();
            student.Email = viewModel.Email.Trim();
            student.BirthDate = viewModel.BirthDate!.Value;
            student.Gpa = viewModel.Gpa!.Value;
            student.DepartmentId = viewModel.DepartmentId!.Value;

            // مفيش داعي لـ Update(): EF بيتابع التغييرات لوحده (Change Tracking)
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تعديل بيانات الطالب بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الطالب";
            }
            return RedirectToAction(nameof(Index));
        }

        // ---------- دوال مساعدة ----------

        private async Task<SelectList> GetDepartmentsAsync(int? selectedId = null)
        {
            var departments = await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();

            return new SelectList(departments, nameof(Department.Id), nameof(Department.Name), selectedId);
        }

        // تحققات تحتاج الداتابيز (تكرار الإيميل + وجود القسم)
        private async Task ValidateStudentAsync(StudentFormViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.Email))
            {
                var email = vm.Email.Trim();
                bool emailTaken = await _context.Students
                    .AnyAsync(s => s.Email == email && s.Id != vm.Id);

                if (emailTaken)
                    ModelState.AddModelError(nameof(vm.Email), "البريد الإلكتروني مسجل لطالب آخر");
            }

            if (vm.DepartmentId.HasValue &&
                !await _context.Departments.AnyAsync(d => d.Id == vm.DepartmentId.Value))
            {
                ModelState.AddModelError(nameof(vm.DepartmentId), "القسم غير موجود");
            }
        }
    }
}