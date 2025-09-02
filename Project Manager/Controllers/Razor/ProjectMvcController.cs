using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;
using Project_Manager.Models.Domain;
using System.Linq;

namespace Project_Manager.Controllers.Razor
{
    public class ProjectMvcController(
        IProjectService projectService,
        ICustomerCompanyService customerCompanyService,
        IExecutorCompanyService executorCompanyService,
        IEmployeeService employeeService,
        IEmployeeOnProjectService employeeOnProjectService

        ) : Controller
    {   // GET Project/StartWizard
        public IActionResult StartWizard()
        {
            TempData.Remove("ProjectWizard");
            return RedirectToAction("CreateStep1");
        }

        // GET Project/CreateStep1
        [HttpGet]
        public IActionResult CreateStep1()
        {
            // Render first step of project creation wizard
            var dto = TempData.Peek("ProjectWizard") is string temp
                ? JsonConvert.DeserializeObject<ProjectWizardDTO>(temp) ?? new ProjectWizardDTO()
                : new ProjectWizardDTO();

            return View(dto);
        }
        // POST Project/CreatStep1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep1(ProjectWizardDTO dto)
        {
            // Save partially filled DTO to temporary storage for next step
            TempData["ProjectWizard"] = JsonConvert.SerializeObject(dto);
            // Redirect to the second step of the wizard
            return RedirectToAction("CreateStep2");
        }

        // GET Project/CreateStep2
        [HttpGet]
        public async Task<IActionResult> CreateStep2()
        {

            //Extract existing ProjectDTO from TempData or create a new one
            var dto = TempData.Peek("ProjectWizard") is string temp
                ? JsonConvert.DeserializeObject<ProjectWizardDTO>(temp) ?? new ProjectWizardDTO()
                : new ProjectWizardDTO();

            // Download companies from services 
            var customerCompanies = await customerCompanyService.GetAllAsync(); 
            var executorCompanies = await executorCompanyService.GetAllAsync();

            //Passing to ViewBag for drop-down lists
            ViewBag.CustomerCompanies = new SelectList(customerCompanies, "Id", "Name");
            ViewBag.ExecutorCompanies = new SelectList(executorCompanies, "Id", "Name");
            return View(dto);
        }

        // POST Project/CreateStep2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep2(ProjectWizardDTO dto)
        {
            // Save updated DTO to TempData for next step
            TempData["ProjectWizard"] = JsonConvert.SerializeObject(dto);

            // Redirect to step 3
            return RedirectToAction("CreateStep3");
        }

        // GET Project/CreateStep3
        [HttpGet]
        public async Task<IActionResult> CreateStep3()
        {

            var dto = TempData.Peek("ProjectWizard") is string temp
                ? JsonConvert.DeserializeObject<ProjectWizardDTO>(temp) ?? new ProjectWizardDTO()
                : new ProjectWizardDTO();

            var employees = await employeeService.GetAllAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");

            return View(dto);
        }

        // POST Project/CreateStep3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep3(ProjectWizardDTO dto)
        {
            TempData["ProjectWizard"] = JsonConvert.SerializeObject(dto);

            return RedirectToAction("CreateStep4");
        }

        // GET Project/CreateStep4
        [HttpGet]
        public async Task<IActionResult> CreateStep4()
        {
            var dto = TempData.Peek("ProjectWizard") is string temp
                ? JsonConvert.DeserializeObject<ProjectWizardDTO>(temp) ?? new ProjectWizardDTO()
                : new ProjectWizardDTO();

            var employees = await employeeService.GetAllAsync();
            ViewBag.Employees = employees;

            return View(dto);
        }

        // POST Project/CreateStep4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep4(ProjectWizardDTO dto, int[]? selectedEmployeeIds)
        {
            if (selectedEmployeeIds != null)
                dto.EmployeeIDs = selectedEmployeeIds.ToList();

            TempData["ProjectWizard"] = JsonConvert.SerializeObject(dto);

            return RedirectToAction("CreateStep5");
        }

        // GET Project/CreateStep5
        [HttpGet]
        public IActionResult CreateStep5()
        {
            var dto = TempData.Peek("ProjectWizard") is string temp
                ? JsonConvert.DeserializeObject<ProjectWizardDTO>(temp) ?? new ProjectWizardDTO()
                : new ProjectWizardDTO();

            return View(dto);
        }

        // POST Project/CreateStep5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStep5(ProjectWizardDTO dto)
        {
            //CreateProject
            var project = new ProjectDTO
            {
                Name = dto.Name,
                StartDate = dto.StartDate!.Value, 
                EndDate = dto.EndDate!.Value,
                Priority = dto.Priority,
                CustomerCompanyID = dto.CustomerCompanyID!.Value,
                ExecutorCompanyID = dto.ExecutorCompanyID!.Value,
                ManagerID = dto.ManagerID!.Value
            };
            await projectService.AddAsync(project);

            //Create Employees On Project
            foreach (var empId in dto.EmployeeIDs)
            {
                var empOnProject = new EmployeeOnProjectDTO
                {
                    ProjectId = project.Id,
                    EmployeeId = empId
                };
                await employeeOnProjectService.AddAsync(empOnProject);
            }

            TempData.Remove("ProjectWizard"); // clear TempData after save

            return RedirectToAction("Index"); 

        }


        // GET Project/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await projectService.GetAllAsync();
            return View(projects);
        }


        // GET Project/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var project = await projectService.GetByIdAsync(id);

                var wizardDto = new ProjectWizardDTO
                {
                    Name = project.Name,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Priority = project.Priority,
                    CustomerCompanyID = project.CustomerCompanyID,
                    ExecutorCompanyID = project.ExecutorCompanyID,
                    ManagerID = project.ManagerID,
                    EmployeeIDs = (await employeeOnProjectService.GetByProjectIdAsync(id))
                           .Select(e => e.EmployeeId).ToList()
                    
                };


                TempData["ProjectWizard"] = JsonConvert.SerializeObject(wizardDto);

    
                TempData["ProjectId"] = id;

                return RedirectToAction("CreateStep1");
            }
            catch(KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index");
            }

        }

        // POST Project/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectDTO dto)
        {
            try
            {
                await projectService.UpdateAsync(id, dto);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка: " + ex.Message);
                return View(dto);
            }
        }
        //POST Project/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await projectService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Проект успешно удалён";

            }
            catch(KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");

        }
    }
}
