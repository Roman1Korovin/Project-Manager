namespace Project_Manager.Models.Domain
{
    public class EmployeeOnProject
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
