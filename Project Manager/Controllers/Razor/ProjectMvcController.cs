using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.DTOs;
using Project_Manager.Models.Domain;

namespace Project_Manager.Controllers.Razor
{
    public class ProjectMvcController(IProjectService projectService) : Controller
    {
        // GET Project/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // protection from CSRF
        public async Task<IActionResult> Create(ProjectDTO dto)
        {
            try
            {
                // Call business logic to add new project
                await projectService.AddAsync(dto);
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
                return View(project);
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
