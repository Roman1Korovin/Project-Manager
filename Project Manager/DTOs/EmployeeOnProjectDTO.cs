namespace Project_Manager.DTOs
{
    // DTO for adding employye to project DTO 
    public class EmployeeOnProjectCreateDTO
    {
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
    }
    //DTo for displaying information about employees in specific project
    public class EmployeeInProjectDTO
    {
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
    //DTO for displaying information about projects of specific employee
    public class ProjectOfEmployeeDTO
    {
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public string CustomerCompanyName { get; set; } = null!;
        public string ExecutorCompanyName { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
    }
}

