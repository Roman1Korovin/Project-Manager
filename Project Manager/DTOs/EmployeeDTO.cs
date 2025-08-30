namespace Project_Manager.DTOs
{
    //Data transfer object class
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
