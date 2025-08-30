namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task AddAsync(string fullname, string email, CancellationToken cancellationToken = default);
    }
}
