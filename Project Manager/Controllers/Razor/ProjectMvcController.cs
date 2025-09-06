using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;
using System.Linq.Dynamic.Core;

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
            HttpContext.Session.Remove("ProjectWizard");
            return RedirectToAction("CreateStep1");
        }

        //Common method for getting DTO from Session
        private ProjectWizardDTO GetWizardFromSession()
        {
            var json = HttpContext.Session.GetString("ProjectWizard");
            return string.IsNullOrEmpty(json)
                ? new ProjectWizardDTO()
                : JsonConvert.DeserializeObject<ProjectWizardDTO>(json)!;
        }

        //Common method for saving DTO from Session
        private void SaveWizardToSession(ProjectWizardDTO dto)
        {
            HttpContext.Session.SetString("ProjectWizard", JsonConvert.SerializeObject(dto));
        }

        // GET Project/CreateStep1
        [HttpGet]
        public IActionResult CreateStep1()
        {
            var dto = GetWizardFromSession();
            return View(dto.Step1);
        }

        // POST Project/CreatStep1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep1(Step1DTO step1)
        {

            var wizard = GetWizardFromSession();
            wizard.Step1 = step1; // update only step 1
            SaveWizardToSession(wizard);
            // Redirect to the second step of the wizard
            return RedirectToAction("CreateStep2");
        }

        // GET Project/CreateStep2
        [HttpGet]
        public async Task<IActionResult> CreateStep2()
        {

            var dto = GetWizardFromSession();

            // Download companies from services 
            var customerCompanies = await customerCompanyService.GetAllAsync(); 
            var executorCompanies = await executorCompanyService.GetAllAsync();

            //Passing to ViewBag for drop-down lists
            ViewBag.CustomerCompanies = new SelectList(customerCompanies, "Id", "Name");
            ViewBag.ExecutorCompanies = new SelectList(executorCompanies, "Id", "Name");

            return View(dto.Step2);
        }

        // POST Project/CreateStep2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep2(Step2DTO step2)
        {
            var wizard = GetWizardFromSession();
            wizard.Step2 = step2;
            SaveWizardToSession(wizard);

            // Redirect to step 3
            return RedirectToAction("CreateStep3");
        }

        // GET Project/CreateStep3
        [HttpGet]
        public async Task<IActionResult> CreateStep3()
        {

            var dto = GetWizardFromSession();

            return View(dto.Step3);
        }

        // POST Project/CreateStep3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep3(Step3DTO step3)
        {
            var wizard = GetWizardFromSession();
            wizard.Step3 = step3;
            SaveWizardToSession(wizard);
            return RedirectToAction("CreateStep4");
        }

        // GET Employee/Search (для AJAX автокомплита)
        [HttpGet]
        public async Task<IActionResult> SearchEmployees(string term)
        {
            var employees = await employeeService.GetAllAsync();

            var filtered = string.IsNullOrWhiteSpace(term)
                ? employees
                : employees.Where(e => e.FullName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();

            var result = filtered.Select(e => new { id = e.Id, text = e.FullName }).ToList();

            return Json(result);
        }

        // GET Project/CreateStep4
        [HttpGet]
        public async Task<IActionResult> CreateStep4()
        {
            var dto = GetWizardFromSession();

            var employees = await employeeService.GetAllAsync();
            ViewBag.Employees = employees;

            return View(dto.Step4);
        }

        // POST Project/CreateStep4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStep4(Step4DTO step4, int[]? selectedEmployeeIds)
        {
            var wizard = GetWizardFromSession();
            wizard.Step4 = step4;
            if (selectedEmployeeIds != null)
                step4.EmployeeIDs = selectedEmployeeIds.ToList();

            SaveWizardToSession(wizard);

            return RedirectToAction("CreateStep5");
        }

        // GET Project/CreateStep5
        [HttpGet]
        public IActionResult CreateStep5()
        {
            var dto = GetWizardFromSession();

            return View(dto.Step5);
        }

        // POST Project/CreateStep5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStep5(Step5DTO step5)
        {
            var wizard = GetWizardFromSession();
            wizard.Step5 = step5;

            int? projectId = HttpContext.Session.GetInt32("ProjectId");

            if (projectId.HasValue)
            {
                await UpdateProjectAsync(projectId.Value, wizard);
            }
            else
            {
                await CreateProjectAsync(wizard);
            }

            // clear Session after save
            HttpContext.Session.Remove("ProjectWizard");
            HttpContext.Session.Remove("ProjectId");

            return RedirectToAction("Index"); 

        }

        private async Task CreateProjectAsync(ProjectWizardDTO dto)
        {
            //CreateProject
            var project = new ProjectDTO
            {
                Name = dto.Step1.Name,
                StartDate = dto.Step1.StartDate!.Value,
                EndDate = dto.Step1.EndDate!.Value,
                Priority = dto.Step1.Priority,
                CustomerCompanyID = dto.Step2.CustomerCompanyID!.Value,
                ExecutorCompanyID = dto.Step2.ExecutorCompanyID!.Value,
                ManagerID = dto.Step3.ManagerID!.Value
            };
            await projectService.AddAsync(project);

            //Create Employees On Project
            foreach (var empId in dto.Step4.EmployeeIDs)
            {
                var empOnProject = new EmployeeOnProjectCreateDTO
                {
                    ProjectId = project.Id,
                    EmployeeId = empId
                };
                await employeeOnProjectService.AddAsync(empOnProject);
            }
        }

        private async Task UpdateProjectAsync(int projectId, ProjectWizardDTO dto)
        {
            // Update project
            var project = new ProjectDTO
            {
                Id = projectId,
                Name = dto.Step1.Name,
                StartDate = dto.Step1.StartDate!.Value,
                EndDate = dto.Step1.EndDate!.Value,
                Priority = dto.Step1.Priority,
                CustomerCompanyID = dto.Step2.CustomerCompanyID!.Value,
                ExecutorCompanyID = dto.Step2.ExecutorCompanyID!.Value,
                ManagerID = dto.Step3.ManagerID!.Value
            };

            await projectService.UpdateAsync(projectId, project);

            // Update employees
            await employeeOnProjectService.DeleteByProjectIdAsync(projectId); //Delete old Employees
            foreach (var empId in dto.Step4.EmployeeIDs)
            {
                var empOnProject = new EmployeeOnProjectCreateDTO
                {
                    ProjectId = project.Id,
                    EmployeeId = empId
                };
                await employeeOnProjectService.AddAsync(empOnProject);
            }
        }

        // GET Project/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await projectService.GetAllAsync();
            return View(projects);
        }
        // GET Project/EmployeesTablePartial
        [HttpGet]
        public async Task<IActionResult> ProjectsTablePartial(
            string sortColumn = "Name",
            string sortDirection = "asc",
            string? nameFilter = null,
            string? customerFilter = null,
            string? executorFilter = null,
            string? managerFilter = null,
            DateTime? startDateFrom = null,
            DateTime? startDateTo = null,
            DateTime? endDateFrom = null,
            DateTime? endDateTo = null,
            int? priorityFilter = null)
        {
            var projects = await projectService.GetAllAsync();

            // Filter by name
            if (!string.IsNullOrEmpty(nameFilter))
                projects = projects.Where(p => p.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase)).ToList();

            // Filter by customer
            if (!string.IsNullOrEmpty(customerFilter))
                projects = projects.Where(p => p.CustomerName.Contains(customerFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            // Filter by executor
            if (!string.IsNullOrEmpty(executorFilter))
                projects = projects.Where(p => p.ExecutorName.Contains(executorFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            // Filter by manager
            if (!string.IsNullOrEmpty(managerFilter))
                projects = projects.Where(p => p.ManagerName.Contains(managerFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            // Filter by startDate
            if (startDateFrom != null)
                projects = projects.Where(p => p.StartDate >= startDateFrom).ToList();
            // Filter by startDate
            if (startDateTo != null)
                projects = projects.Where(p => p.StartDate <= startDateTo).ToList();
            // Filter by endDate
            if (endDateFrom != null)
                projects = projects.Where(p => p.EndDate >= endDateFrom).ToList();
            // Filter by endDate
            if (endDateTo != null)
                projects = projects.Where(p => p.EndDate <= endDateTo).ToList();
            // Filter by priority
            if (priorityFilter != null)
                projects = projects.Where(p => p.Priority == priorityFilter).ToList();

            // Use Dynamic LINQ to sort by any column and direction
            var sortedProjects = projects.AsQueryable()
                                 .OrderBy($"{sortColumn} {sortDirection}")
                                 .ToList();

            ViewData["SortColumn"] = sortColumn;
            ViewData["SortDirection"] = sortDirection;
            return PartialView("ProjectsTable", sortedProjects);
        }

        // GET Project/IndexProjectEmployees/{int}
        [HttpGet]
        public async Task<IActionResult> IndexProjectEmployees(int projectId)
        {
            var employees = await employeeOnProjectService.GetByProjectIdAsync(projectId);
            var project = await projectService.GetByIdAsync(projectId);
            ViewData["ProjectName"] = project?.Name ?? "неизвестный проект";
            return View(employees); 
        }

        // GET Project/EditEmployees/{projectId}
        [HttpGet]
        public async Task<IActionResult> EditEmployees(int projectId)
        {
            try
            {
                var project = await projectService.GetByIdAsync(projectId);

                var wizardDto = new ProjectWizardDTO
                {
                    Step1 = new Step1DTO
                    {
                        Name = project.Name,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        Priority = project.Priority
                    },
                    Step2 = new Step2DTO
                    {
                        CustomerCompanyID = project.CustomerCompanyID,
                        ExecutorCompanyID = project.ExecutorCompanyID
                    },
                    Step3 = new Step3DTO
                    {
                        ManagerID = project.ManagerID
                    },
                    Step4 = new Step4DTO
                    {
                        EmployeeIDs = (await employeeOnProjectService.GetByProjectIdAsync(projectId))
                            .Select(e => e.EmployeeId)
                            .ToList()
                    }
                };

                HttpContext.Session.SetString("ProjectWizard", JsonConvert.SerializeObject(wizardDto));
                HttpContext.Session.SetInt32("ProjectId", projectId);

                return RedirectToAction("CreateStep4");
            }
            catch (KeyNotFoundException ex)
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




        // GET Project/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var project = await projectService.GetByIdAsync(id);
                var employees = await employeeOnProjectService.GetByProjectIdAsync(id);

                var wizardDto = new ProjectWizardDTO
                {
                    Step1 = new Step1DTO
                    {
                        Name = project.Name,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        Priority = project.Priority
                    },
                    Step2 = new Step2DTO
                    {
                        CustomerCompanyID = project.CustomerCompanyID,
                        ExecutorCompanyID = project.ExecutorCompanyID
                    },
                    Step3 = new Step3DTO
                    {
                        ManagerID = project.ManagerID
                    },
                    Step4 = new Step4DTO
                    {
                        EmployeeIDs = employees.Select(e => e.EmployeeId).ToList()
                    }
                };


                HttpContext.Session.SetString("ProjectWizard", JsonConvert.SerializeObject(wizardDto));

                HttpContext.Session.SetInt32("ProjectId", id);

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
