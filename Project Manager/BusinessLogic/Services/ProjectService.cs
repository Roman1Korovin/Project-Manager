using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.Data_Access.Repositories;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.DTOs;
using Project_Manager.Models.Domain;

namespace Project_Manager.BusinessLogic.Services
{
    public class ProjectService(
        IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository,
        ICustomerCompanyRepository customerCompanyRepository,
        IExecutorCompanyRepository executorCompanyRepository
        ) : IProjectService
    {

        private void ValidateProjectData(ProjectDTO dto)
        {
            //Validate that name is not empty and doesn't consists of spaces 
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentNullException("Название проекта не может быть пустым!", nameof(dto.Name));

            //Validate that name is not more than 500 symbols 
            if (dto.Name.Length > 500)
                throw new ArgumentException("Название проекта не может превышать 500 символов!", nameof(dto.Name));

            //Validate that StartDate is not later than EndDate 
            if (dto.StartDate > dto.EndDate)
                throw new InvalidOperationException("Дата начала проекта не может быть позднее даты окончания проекта!");

            //Validate that Priority equals from 1 to 5
            if (dto.Priority <1 || dto.Priority > 5)
                throw new ArgumentException("Приоритет должен быть от 1 до 5!", nameof(dto.Priority));
        }

        public async Task AddAsync(ProjectDTO dto, CancellationToken cancellationToken = default)
        {
            ValidateProjectData(dto);

            // Check that manager(employee) with current Id is exists
            var manager = await employeeRepository.GetByIdAsync(dto.ManagerID, cancellationToken);
            if (manager == null)
                throw new KeyNotFoundException($"Менеджер с Id {dto.ManagerID} не найден.");

            // Check that customerCompany with current Id is exists
            var customer = await customerCompanyRepository.GetByIdAsync(dto.CustomerCompanyID, cancellationToken);
            if (customer == null)
                throw new KeyNotFoundException($"Компания заказчик с Id {dto.CustomerCompanyID} не найдена.");

            // Check that executorCompany with current Id is exists
            var executor = await executorCompanyRepository.GetByIdAsync(dto.ExecutorCompanyID, cancellationToken);
            if (executor == null)
                throw new KeyNotFoundException($"Компания исполнитель с Id {dto.ExecutorCompanyID} не найдена.");

            //Create Project object and delete spaces at begin and at end
            var project = new Project
            {
                Name = dto.Name.Trim(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Priority = dto.Priority,
                CustomerCompanyID = dto.CustomerCompanyID,
                ExecutorCompanyID = dto.ExecutorCompanyID,
                ManagerID = dto.ManagerID
            };
            //Call repository method to add new Project
            await projectRepository.AddAsync(project, cancellationToken);

            dto.Id = project.Id; // Assign generated Id back to DTO
        }

        //Returns list of DTO contining only necessary field
        public async Task<List<ProjectDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var projects = await projectRepository.GetAllAsync(cancellationToken);
            return projects.Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Priority = p.Priority,
                CustomerCompanyID = p.CustomerCompanyID,
                CustomerName = p.CustomerCompany.Name,
                ExecutorCompanyID = p.ExecutorCompanyID,
                ExecutorName = p.ExecutorCompany.Name,
                ManagerID = p.ManagerID,
                ManagerName = p.Manager.FullName
            }).ToList();
        }



        public async Task<ProjectDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Check that project with current Id is exists
            var project = await projectRepository.GetByIdAsync(id, cancellationToken);
            if (project == null)
                throw new KeyNotFoundException($"Проект с Id {id} не найден.");

            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                CustomerCompanyID = project.CustomerCompanyID,
                ExecutorCompanyID = project.ExecutorCompanyID,
                ManagerID = project.ManagerID
            };
        }

        public async Task UpdateAsync(int id, ProjectDTO dto, CancellationToken cancellationToken = default)
        {
            ValidateProjectData(dto);

            // Check that manager(employee) with current Id is exists
            var manager = await employeeRepository.GetByIdAsync(dto.ManagerID, cancellationToken);
            if (manager == null)
                throw new KeyNotFoundException($"Сотрудник с Id {dto.ManagerID} не найден.");

            // Check that customerCompany with current Id is exists
            var customer = await customerCompanyRepository.GetByIdAsync(dto.CustomerCompanyID, cancellationToken);
            if (customer == null)
                throw new KeyNotFoundException($"Компания заказчик с Id {dto.CustomerCompanyID} не найдена.");

            // Check that executorCompany with current Id is exists
            var executor = await executorCompanyRepository.GetByIdAsync(dto.ExecutorCompanyID, cancellationToken);
            if (executor == null)
                throw new KeyNotFoundException($"Компания исполнитель с Id {dto.ExecutorCompanyID} не найдена.");

            //Check that project with current Id is exists
            var project = await projectRepository.GetByIdAsync(id, cancellationToken);
            if (project == null)
                throw new KeyNotFoundException($"Проект с Id {id} не найден.");

            //Update Project object and delete spaces at begin and at end
            project.Name = dto.Name.Trim();
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.Priority = dto.Priority;
            project.CustomerCompanyID = dto.CustomerCompanyID;
            project.ExecutorCompanyID = dto.ExecutorCompanyID;
            project.ManagerID = dto.ManagerID;

            await projectRepository.UpdateAsync(project, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Check that project with current Id is exists
            var project = await projectRepository.GetByIdAsync(id, cancellationToken);
            if (project == null)
                throw new KeyNotFoundException($"Проект с Id {id} не найден.");

            await projectRepository.DeleteAsync(project, cancellationToken);
        }

    }
}
