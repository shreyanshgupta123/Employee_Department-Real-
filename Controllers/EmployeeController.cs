using Employee_Department.Data;
using Employee_Department.Models;
using Employee_Department.Models.View_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Department.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext _context;

        public EmployeeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Use LINQ to join Employees and Departments
            var data = from e in _context.Employees
                       join d in _context.Departments on e.DepartmentId equals d.DepartmentID
                       select new EmployeeDepartmentSummaryView
                       {
                           EmployeeId = e.EmployeeId,
                           FirstName = e.FirstName,
                           LastName = e.LastName,
                           Gender = e.Gender,
                           DepartmentId = e.DepartmentId,
                           DepartmentName = d.DepartmentName,
                           Departmentcode = d.Departmentcode
                       };

            return View(data.ToList());
        }

        // GET: Employee/AddEmployee
        public async Task<IActionResult> AddEmployee()
        {
            ViewBag.department = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeeDepartmentSummaryView empdep)
        {
            ViewBag.department = await _context.Departments.ToListAsync();

            try
            {
                ModelState.Remove("DepartmentName");
                ModelState.Remove("Departmentcode");
                if (ModelState.IsValid)
                {
                    // Ensure that DepartmentId is properly set from the input field.
                    var data = new Employee()
                    {
                        FirstName = empdep.FirstName,
                        LastName = empdep.LastName,
                        Gender = empdep.Gender,
                        DepartmentId = empdep.DepartmentId // Bind DepartmentId from the input field.
                    };

                    await _context.Employees.AddAsync(data);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Record has been saved";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
            }

            // If there are validation or other errors, return to the form with the model.
            ViewBag.department = await _context.Departments.ToListAsync();
            return View(empdep);
        }
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                else
                {
                    var data = _context.Employees.FindAsync(id);
                    if (data == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        _context.Employees.Remove(await data);
                        _context.SaveChangesAsync();
                        TempData["Success"] = "Record has been successfully deleted";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditEmployee(int id)
        {
            EmployeeDepartmentSummaryView employeeDepartment = new EmployeeDepartmentSummaryView();
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                else
                {
                    employeeDepartment = await (from e in _context.Employees.Where(e => e.EmployeeId == id)
                                                join d in _context.Departments on e.DepartmentId equals d.DepartmentID
                                                select new EmployeeDepartmentSummaryView
                                                {
                                                    EmployeeId = e.EmployeeId,
                                                    FirstName = e.FirstName,
                                                    LastName = e.LastName,
                                                    Gender = e.Gender,
                                                    DepartmentId = e.DepartmentId,
                                                    DepartmentName = d.DepartmentName,
                                                    Departmentcode = d.Departmentcode
                                                }).FirstOrDefaultAsync();

                    if (employeeDepartment == null)
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(employeeDepartment);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeDepartmentSummaryView empdep)
        {
            ViewBag.department = await _context.Departments.ToListAsync();
            try
            {
                if (ModelState.IsValid)
                {
                    var existingEmployee = await _context.Employees.FindAsync(empdep.EmployeeId);

                    if (existingEmployee == null)
                    {
                        return NotFound();
                    }

                    existingEmployee.FirstName = empdep.FirstName;
                    existingEmployee.LastName = empdep.LastName;
                    existingEmployee.Gender = empdep.Gender;
                    existingEmployee.DepartmentId = empdep.DepartmentId;

                    await _context.SaveChangesAsync();
                    TempData["success"] = "Record has been updated";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);

                // Log the inner exception
                Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
            }

            ViewBag.department = await _context.Departments.ToListAsync();
            return View(empdep);
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
                        var data = _context.Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
                        if (data != null)
                        {
                            _context.Employees.Remove(data);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                TempData["success"] = "Record has been successfully deleted!";
                result = "success";
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult(result);
        }
        public async Task<IActionResult> EditColumn(int employeeId, string field, string value)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(employeeId);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                switch (field)
                {
                    case "FirstName":
                        existingEmployee.FirstName = value;
                        break;
                    case "LastName":
                        existingEmployee.LastName = value;
                        break;
                    case "Gender":
                        existingEmployee.Gender = value;
                        break;
                    case "DepartmentName":
                        existingEmployee.DepartmentId = int.Parse(value); // Assuming DepartmentId is an integer
                        break;
                    case "Departmentcode":
                        // Assuming Departmentcode is a string
                        // Handle Departmentcode here
                        break;
                    default:
                        return BadRequest();
                }
                await _context.SaveChangesAsync();
                return Ok("Record updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }


    }
}
