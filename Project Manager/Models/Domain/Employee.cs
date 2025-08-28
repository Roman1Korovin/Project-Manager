namespace Project_Manager.Models.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        //Collection of projects where employee worked
        public ICollection<EmployeeOnProject> EmployeeProjects { get; set; } = new List<EmployeeOnProject>();

        //Collection projects where employee was manager
        public ICollection<Project> ManagerProjects { get; set; } = new List<Project>();
    }
}
