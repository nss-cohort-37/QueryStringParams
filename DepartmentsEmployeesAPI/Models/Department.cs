using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentsEmployeesAPI.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 25 characters")]
        public string DepartmentName { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
