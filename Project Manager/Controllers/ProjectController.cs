using Microsoft.AspNetCore.Mvc;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;

namespace Project_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        // POST api/project
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProjectDTO dto)
        {
            try
            {
                // Call business logic to add new project
                await projectService.AddAsync(dto);
                return Ok();    // Return 200 OK if successful
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  //Return 404 Not Found if Manager(Employee),CustomerCompany or ExecutorCompany with specified Id doesnt found
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);  // Return 400 Bad Request if any validation fails
            }
        }
        // GET api/project
        [HttpGet]
        public async Task<IActionResult> GettAllAsync()
        {
            var projects = await projectService.GetAllAsync();
            return Ok(projects);   
        }

        // PUT api/project/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectDTO dto)
        {
            try
            {
                await projectService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);    //Return 404 Not Found if Employee with specified Id doesnt found
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);  // Return 400 Bad Request if validation fails
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return 400 if StartDate/EndDate invalid
            }
        }

        // Delete api/project/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await projectService.DeleteAsync(id);
                return NoContent();             // Return 204 No Content if successful
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);    //Return 404 Not Found if Employee with specified Id doesnt found
            }
        }
    }
}
