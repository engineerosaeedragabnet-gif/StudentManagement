using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentListItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    StudentsCount = d.Students.Count     // بتتحول لـ COUNT subquery في SQL
                })
                .ToListAsync();

            return View(departments);
        }

        // GET: Departments/Create
        public IActionResult Create() => View();

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Department department)
        {
            if (await _context.Departments.AnyAsync(d => d.Name == department.Name))
                ModelState.AddModelError(nameof(department.Name), "القسم ده موجود بالفعل");

            if (!ModelState.IsValid) return View(department);

            _context.Add(department);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تمت إضافة القسم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id) return NotFound();

            if (await _context.Departments.AnyAsync(d => d.Name == department.Name && d.Id != id))
                ModelState.AddModelError(nameof(department.Name), "القسم ده موجود بالفعل");

            if (!ModelState.IsValid) return View(department);

            var existing = await _context.Departments.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = department.Name;
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تعديل القسم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new DepartmentListItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    StudentsCount = d.Students.Count
                })
                .FirstOrDefaultAsync();

            if (department == null) return NotFound();
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _context.Students.AnyAsync(s => s.DepartmentId == id))
            {
                TempData["Error"] = "لا يمكن حذف قسم فيه طلاب. انقل الطلاب أو احذفهم أولًا.";
                return RedirectToAction(nameof(Index));
            }

            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف القسم";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}