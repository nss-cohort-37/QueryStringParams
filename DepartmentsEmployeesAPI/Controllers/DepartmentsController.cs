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
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentsController(IConfiguration config)
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


        // I want this url to be available
        // api/departments?name=Engineering
        // api/departments
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string name)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, DeptName 
                        FROM Department";

                    if (name != null)
                    {
                        cmd.CommandText += " WHERE DeptName LIKE @deptName";
                        cmd.Parameters.Add(new SqlParameter("@deptName", "%" + name + "%"));
                    }                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Department> departments = new List<Department>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string deptName = reader.GetString(reader.GetOrdinal("DeptName"));

                        Department department = new Department
                        {
                            Id = id,
                            DepartmentName = deptName
                        };

                        departments.Add(department);
                    }

                    reader.Close();

                    return Ok(departments);
                }
            }
        }
    }
}