namespace Project_Manager.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int CustomerCompanyID { get; set; }
        public string CustomerName { get; set; } = null!;
        public int ExecutorCompanyID { get; set; }
        public string ExecutorName { get; set; } = null!;
        public int ManagerID { get; set; }
        public string ManagerName { get; set; } = null!;

        public List<int> EmployeeIDs { get; set; } = new List<int>();

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
