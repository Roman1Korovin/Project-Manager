namespace Project_Manager.DTOs
{
    public class EmployeeOnProjectDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}

