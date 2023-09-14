using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Department.Models.View_Model
{
    public class EmployeeDepartmentSummaryView
    {
        [Key]
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
         
        public string DepartmentName { get; set; } = default;
        public string Departmentcode { get; set; } = default;
    }
}
