namespace Project_Manager.Models.Domain
{
    public class ExecutorCompany
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        //Collection of projects where company was executor
        public ICollection<Project> ProjectAsExecutor { get; set; } = new List<Project>();

    }
}
