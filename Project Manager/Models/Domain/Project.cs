namespace Project_Manager.Models.Domain
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; } 

        public int Priority {  get; set; }

        public int CustomerCompanyID { get; set; }
        public CustomerCompany CustomerCompany { get; set; } = null!;

        public int ExecutorCompanyID { get; set; }
        public ExecutorCompany ExecutorCompany { get; set; } = null!;

        public int ManagerID { get; set; }
        public Employee Manager { get; set; } = null!;

        //Collection of employees who work on Project 
        public ICollection<EmployeeOnProject> EmployeesOnProject { get; set; } = new List<EmployeeOnProject>();
    }
}
