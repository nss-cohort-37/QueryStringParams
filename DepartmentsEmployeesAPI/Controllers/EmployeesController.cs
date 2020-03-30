using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepartmentsEmployeesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DepartmentsEmployeesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeesController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // Make this url available with correct query params
        // api/employees?firstName=Adam&lastName=Sheaffer
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string firstName, 
            [FromQuery] string lastName, 
            [FromQuery] int? departmentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, LastName, DepartmentId 
                        FROM Employee
                        WHERE 1 = 1";

                    if (firstName != null)
                    {
                        cmd.CommandText += " AND FirstName LIKE @firstName";
                        cmd.Parameters.Add(new SqlParameter("@firstName", "%" + firstName + "%"));
                    }

                    if (lastName != null)
                    {
                        cmd.CommandText += " AND LastName LIKE @lastName";
                        cmd.Parameters.Add(new SqlParameter("@lastName", "%" + lastName + "%"));
                    }

                    if (departmentId != null)
                    {
                        cmd.CommandText += " AND DepartmentId = @deptId";
                        cmd.Parameters.Add(new SqlParameter("@deptId", departmentId));
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        int deptId = reader.GetInt32(reader.GetOrdinal("DepartmentId"));
                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));

                        Employee employee = new Employee
                        {
                            Id = id,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            DepartmentId = deptId
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    return Ok(employees);
                }
            }
        }
    }
}