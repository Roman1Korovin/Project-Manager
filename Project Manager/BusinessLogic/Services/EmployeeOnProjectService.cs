using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.Data_Access.Repositories;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.DTOs;
using Project_Manager.Models.Domain;

namespace Project_Manager.BusinessLogic.Services
{
    public class EmployeeOnProjectService(
        IEmployeeOnProjectRepository employeeOnProjectRepository,
        IEmployeeRepository employeeRepository,
        IProjectRepository projectRepository
        ): IEmployeeOnProjectService
    {
        public async Task AddAsync(EmployeeOnProjectCreateDTO dto, CancellationToken cancellationToken = default)
        {
            // Check that employee with current Id is exists
            var employee = await employeeRepository.GetByIdAsync(dto.EmployeeId, cancellationToken);
            if (employee == null)
                throw new KeyNotFoundException($"Сотрудник с Id {dto.EmployeeId} не найден.");

            // Check that project with current Id is exists
            var project = await projectRepository.GetByIdAsync(dto.ProjectId, cancellationToken);
            if (project == null)
                throw new KeyNotFoundException($"Проект с Id {dto.ProjectId} не найдена.");
            //Create EmployeeOnProject object
            var eop = new EmployeeOnProject
            {
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId,
            };
            //Call repository method to add new EmployeeOnProject
            await employeeOnProjectRepository.AddAsync(eop, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Check that employeeOnProject with current Id is exists
            var eop = await employeeOnProjectRepository.GetByIdAsync(id, cancellationToken);
            if (eop == null)
                throw new KeyNotFoundException($"Запись EmployeeOnProject с Id {id} не найдена.");

            await employeeOnProjectRepository.DeleteAsync(eop, cancellationToken);
        }

        public async Task<List<ProjectOfEmployeeDTO>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            
            var employee = await employeeRepository.GetByIdAsync(employeeId, cancellationToken);
            // Check that employee with current Id is exists
            if (employee == null)
                throw new KeyNotFoundException($"Сотрудник с Id {employeeId} не найден.");

            var employees = await employeeOnProjectRepository.GetByEmployeeIdAsync(employeeId, cancellationToken);
            return employees.Select(e => new ProjectOfEmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                ProjectId = e.ProjectId,
                ProjectName = e.Project?.Name ?? "неизвестно",
                StartDate = e.Project?.StartDate ?? DateTime.MinValue,
                EndDate = e.Project?.EndDate ?? DateTime.MinValue,
                Priority = e.Project?.Priority ?? 0,
                CustomerCompanyName = e.Project?.CustomerCompany?.Name ?? "неизвестно",
                ExecutorCompanyName = e.Project?.ExecutorCompany?.Name ?? "неизвестно",
                ManagerName = e.Project?.Manager?.FullName ?? "неизвестно"
            })
                .OrderBy(p => p.ProjectName)
                .ToList();
        }

        public async Task<List<EmployeeInProjectDTO>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            
            var project = await projectRepository.GetByIdAsync(projectId, cancellationToken);
            // Check that project with current Id is exists
            if (project == null)
                throw new KeyNotFoundException($"Проект с Id {projectId} не найден.");

            var projects = await employeeOnProjectRepository.GetByProjectIdAsync(projectId, cancellationToken);
            return projects.Select(e => new EmployeeInProjectDTO
            {
                EmployeeId = e.EmployeeId,
                ProjectId = e.ProjectId,
                FullName = e.Employee?.FullName ?? "неизвестно",
                Email = e.Employee?.Email ?? "неизвестно",
            })
                .OrderBy(e => e.FullName)
                .ToList();
        }

        public async Task DeleteByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            // Check that project with current Id is exists
            var project = await projectRepository.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                throw new KeyNotFoundException($"Проект с Id {projectId} не найден.");

            // Delete employees from project
            await employeeOnProjectRepository.DeleteByProjectIdAsync(projectId, cancellationToken);
        }
    }
}
