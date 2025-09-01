using Microsoft.AspNetCore.Mvc;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.BusinessLogic.Services.Interfaces;
using System.Threading;
using Project_Manager.DTOs;

namespace Project_Manager.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeApiController(IEmployeeService employeeService) : ControllerBase
    {


        // POST api/employee
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] EmployeeDTO dto)
        {
            try
            {
                // Call business logic to add new employee
                await employeeService.AddAsync(dto);
                return Ok();    // Return 200 OK if successful
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);  // Return 400 Bad Request if any validation fails
            }
        }
        // GET api/employee
        [HttpGet]
        public async Task<IActionResult> GettAllAsync()
        {
            var employees = await employeeService.GetAllAsync();
            return Ok(employees);   // Return 200 OK if successful
        }

        // PUT api/employee/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] EmployeeDTO dto)
        {
            try
            {
                await employeeService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);    //Return 404 Not Found if Employee with specified Id doesnt found
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);  // Return 400 Bad Request if any validation fails
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);  
            }
        }

        // Delete api/employee/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await employeeService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);    //Return 404 Not Found if Employee with specified Id doesnt found
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


