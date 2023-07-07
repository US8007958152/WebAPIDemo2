using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPIDemo2.Entities;
using WebAPIDemo2.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIDemo2.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _Employee;
        private readonly IConfiguration _configuration;
        public EmployeeController(IEmployee employee, IConfiguration configuration)
        {
            _Employee = employee;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginProfile loginProfile)
        {
            if (loginProfile == null)
                return BadRequest();

            if(loginProfile.UserName!="Virat" && loginProfile.Password!="virat@123")
                return Unauthorized();
            loginProfile.Token = GenerateToken();
            return new JsonResult(loginProfile);
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddSeconds(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Request gets filter

        [HttpPost]
        
        public IActionResult Create(Employee employee)
        {
            
            try
            {
                if (employee == null)
                    return BadRequest();

                int rowCount = _Employee.Create(employee);

                if (rowCount == 0)
                    return BadRequest("Error in data saving");
                else
                    return Ok("Saved successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Response gets filter
       

        [HttpGet]
        public IActionResult Detail(int empId)
        {
            try
            {
                if (empId == 0)
                    return BadRequest();

                Employee employee = _Employee.Detail(empId);

                if (employee == null)
                    return BadRequest("Employee is not found");

                return Ok(employee);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet]
        [Route("{empId?}")]
        public IActionResult Delete(int empId)
        {
            try
            {
                if (empId == 0)
                    return BadRequest();

                int rowCount = _Employee.Delete(empId);

                if (rowCount == 0)
                    return BadRequest("Employee is not deleted");

                return Ok("Employee has been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("{empId?}/{temp?}")]
        public IActionResult Delete(int empId,int temp)
        {
            try
            {
                if (empId == 0)
                    return BadRequest();

                int rowCount = _Employee.Delete(empId);

                if (rowCount == 0)
                    return BadRequest("Employee is not deleted");

                return Ok("Employee has been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
