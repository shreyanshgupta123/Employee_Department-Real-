using Employee_Department.Data;
using Employee_Department.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Department.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationContext context;
        public DepartmentController(ApplicationContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var data = await context.Departments.ToListAsync();
            return View(data);
        }
        // to add AddDepartment
        public async Task<IActionResult> AddDepartment(int? id)
        {
            Department department = new Department();
            if (id != null && id != 0)
            {
                department = await context.Departments.FindAsync(id);
            }
            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            else
            {
                await context.Departments.AddAsync(department);
                await context.SaveChangesAsync();
                TempData["Success"] = "Department has been created!";
            }
            return RedirectToAction("Index");
        }
        //adding delete function
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                bool status = context.Employees.Any(x => x.DepartmentId == id);
                if (status)
                {
                    TempData["warning"] = "department is taken by another employee,so can't delete this";
                }
                else
                {
                    var department = await context.Departments.FindAsync(id);
                    if (department == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        context.Departments.Remove(department);
                        await context.SaveChangesAsync();
                        TempData["Success"] = "Department has been successfully deleted";
                    }
                }
            }
            else
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteSingleOrMultiple(int[] ids)
        {
            string result = string.Empty;
            try
            {
                if (ids.Count() > 0)
                {
                    foreach (int id in ids)
                    {
                        var data = context.Departments.Where(d => d.DepartmentID == id).FirstOrDefault();
                        if (data != null)
                        {
                            context.Departments.Remove(data);
                        }
                    }
                }
                await context.SaveChangesAsync();
                TempData["success"] = "Record has been successfully deleted!";
                result = "success";
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult(result);
        }
    }
}
