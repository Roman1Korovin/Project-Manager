using Microsoft.AspNetCore.Mvc;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.BusinessLogic.Services.Interfaces;

namespace Project_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
       
    public class EmployeeController(IEmployeeService employeeService): ControllerBase
    {
        // POST api/employee
        [HttpPost]
        public async Task<IActionResult> AddAsync(string fullName, string email)
        {
            try
            {
                // Call business logic to add new employee
                await employeeService.AddAsync(fullName, email);
                return Ok();    // Return 200 OK if successful
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);  // Return 400 Bad Request if any validation fails
            }
        }
    }
}
