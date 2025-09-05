using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;

namespace Project_Manager.Controllers.Razor
{
    public class EmployeeMvcController(
        IEmployeeService employeeService,
        IProjectService projectService,
        IEmployeeOnProjectService employeeOnProjectService
        ) : Controller
    {
        // GET: Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDTO dto)
        {
            try
            {
                await employeeService.AddAsync(dto);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: Employee/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await employeeService.GetAllAsync();
            return View(employees);
        }
        // GET: Employee/IndexEmployeeProjects/{empId}
        [HttpGet]
        public async Task<IActionResult> IndexEmployeeProjects(int employeeId)
        {
            var projects = await employeeOnProjectService.GetByEmployeeIdAsync(employeeId);
            var employee = await employeeService.GetByIdAsync(employeeId);
            ViewData["EmployeeName"] = employee?.FullName ?? "неизвестный сотрудник";
            return View(projects);
        }

        // GET: Employee/Edit/{int}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var employee = await employeeService.GetByIdAsync(id); 

                return View("Create", employee); 
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return RedirectToAction("Index");
            }
        }
        // POST: Employee/Edit/{int}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDTO dto)
        {
  
            try
            {
                await employeeService.UpdateAsync(id, dto);
                return RedirectToAction("Index"); 
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Create", dto);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Create", dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка: " + ex.Message);
                return View("Create", dto);
            }
        }
        //POST Employee/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await employeeService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Сотрудник успешно удалён";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (KeyNotFoundException ex)
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
