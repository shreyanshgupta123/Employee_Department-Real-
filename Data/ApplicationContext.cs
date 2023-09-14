
using Employee_Department.Models;
using Employee_Department.Models.View_Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Department.Data

{
    public class ApplicationContext:DbContext
    {
       public ApplicationContext(DbContextOptions<ApplicationContext>options):base(options)
        { }
        //how many table you have 
        public DbSet<Employee>Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        [NotMapped]
        public DbSet<EmployeeDepartmentSummaryView>employeeDepartmentSummaryViews { get; set; }
    }
}
