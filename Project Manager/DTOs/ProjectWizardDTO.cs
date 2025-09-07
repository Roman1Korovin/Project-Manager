namespace Project_Manager.DTOs
{
    public class ProjectWizardDTO
    {

        public Step1DTO Step1 { get; set; } = new Step1DTO();
        public Step2DTO Step2 { get; set; } = new Step2DTO();
        public Step3DTO Step3 { get; set; } = new Step3DTO();
        public Step4DTO Step4 { get; set; } = new Step4DTO();
        public Step5DTO Step5 { get; set; } = new Step5DTO();
    }
    public class Step1DTO
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = DateTime.Today;
        public DateTime? EndDate { get; set; } = DateTime.Today.AddMonths(2);
        public int Priority { get; set; } = 1;
    }

    public class Step2DTO
    {
        public int? CustomerCompanyID { get; set; }
        public int? ExecutorCompanyID { get; set; }

        public string? NewCustomerCompanyName { get; set; }
        public string? NewExecutorCompanyName { get; set; }
    }

    public class Step3DTO
    {
        public int? ManagerID { get; set; }
    }

    public class Step4DTO
    {
        public List<int> EmployeeIDs { get; set; } = new List<int>();
    }

    public class Step5DTO
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}