using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentsEmployeesAPI.Models
{
    public class EmployeeBreakdown
    {
        public int DepartmentId { get; set; }
        public int EmployeeCount { get; set; }
        public string DepartmentName { get; set; }
    }
}
