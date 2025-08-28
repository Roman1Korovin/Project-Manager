namespace Project_Manager.Models.Domain
{
    public class CustomerCompany
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        //Collection of projects where company was customer 
        public ICollection<Project> ProjectAsCustomer { get; set; } = new List<Project>();
    }
}
