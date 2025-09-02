namespace Project_Manager.DTOs
{
    public class ProjectWizardDTO
    {
        // Step 1
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = DateTime.Today;
        public DateTime? EndDate { get; set; } = DateTime.Today.AddMonths(2);
        public int Priority { get; set; } = 1;

        // Step 2
        public int? CustomerCompanyID { get; set; }
        public int? ExecutorCompanyID { get; set; }

        // Step 3
        public int? ManagerID { get; set; }

        // Step 4
        public List<int> EmployeeIDs { get; set; } = new List<int>();

        // Step 5
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}