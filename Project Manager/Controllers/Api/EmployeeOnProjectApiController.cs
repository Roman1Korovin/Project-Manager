using Microsoft.AspNetCore.Mvc;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;

namespace Project_Manager.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeOnProjectApiController(IEmployeeOnProjectService employeeOnProjectService) : ControllerBase
    {
        // POST api/employeeonproject
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] EmployeeOnProjectDTO dto)
        {
            try
            {
                // Call business logic to add new project
                await employeeOnProjectService.AddAsync(dto);
                return Ok();    // Return 200 OK if successful
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  //Return 404 Not Found if Employee or Project with specified Id doesnt found
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);  // Return 400 Bad Request if any validation fails
            }
        }
        // GET api/employeeonproject/employee/{employeeId}
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var employees = await employeeOnProjectService.GetByEmployeeIdAsync(employeeId);
                return Ok(employees);  // Return 200 OK with list of project employees
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); //Return 404 Not Found if Employee with specified Id doesnt found
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // GET api/employeeonproject/project/{projectId}
        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> GetByProjectIdAsync(int projectId)
        {
            try
            {
                var projects = await employeeOnProjectService.GetByProjectIdAsync(projectId);
                return Ok(projects);    //Return 200 OK with list of employee projects
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); //Return 404 Not Found if Employee with specified Id doesnt found
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



        // Delete api/employeeonproject/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await employeeOnProjectService.DeleteAsync(id);
                return NoContent();             // Return 204 No Content if successful
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
