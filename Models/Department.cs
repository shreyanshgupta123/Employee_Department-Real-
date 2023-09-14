using System.ComponentModel.DataAnnotations;

namespace Employee_Department.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [Display(Name ="Department Name")]
        [Required(ErrorMessage ="Please enter department name")]
        public string DepartmentName { get; set; }=default;
        [Display(Name = "Department Code")]
        [Required(ErrorMessage = "Please enter department Code")]
        public string Departmentcode { get; set; } = default;
    }
}
