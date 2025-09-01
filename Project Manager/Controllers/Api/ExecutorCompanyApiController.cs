using Microsoft.AspNetCore.Mvc;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;

namespace Project_Manager.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]

    public class ExecutorCompanyApiController(IExecutorCompanyService executorCompanyService) : ControllerBase
    {
        // POST api/executorCompany
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ExecutorCompanyDTO dto)
        {
            try
            {
                // Call business logic to add new employee
                await executorCompanyService.AddAsync(dto);
                return Ok();    // Return 200 OK if successful
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);  // Return 400 Bad Request if any validation fails
            }
        }
    }
}
