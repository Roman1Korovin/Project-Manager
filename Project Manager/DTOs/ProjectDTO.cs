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
        public int ExecutorCompanyID { get; set; }
        public int ManagerID { get; set; }
    }
}
