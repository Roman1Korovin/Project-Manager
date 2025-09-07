using Microsoft.EntityFrameworkCore;
using Project_Manager.Models.Domain;
using System;
using System.Reflection.Emit;

namespace Project_Manager.Data_Access
{
    public class AppContextDB(DbContextOptions<AppContextDB> options) : DbContext(options)
    {
        //Use DbSet to specify which entities will be store in DB 
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<CustomerCompany> CustomerCompanies { get; set; } = null!;
        public DbSet<ExecutorCompany> ExecutorCompanies { get; set; } = null!;
        public DbSet<EmployeeOnProject> EmployeeOnProjects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set primary keys
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<CustomerCompany>().HasKey(c => c.Id);
            modelBuilder.Entity<ExecutorCompany>().HasKey(e => e.Id);

            //Set composite key
            modelBuilder.Entity<EmployeeOnProject>().HasKey(ep => new { ep.ProjectId, ep.EmployeeId });

            // Configure Project - Manager(Employee) relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany(e => e.ManagerProjects)
                .HasForeignKey(p => p.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Project - CustomerCompany relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.CustomerCompany)
                .WithMany(c => c.ProjectAsCustomer)
                .HasForeignKey(p => p.CustomerCompanyID)
                .OnDelete(DeleteBehavior.Restrict);         //can't delete customer while project exists

            // Configure Project - ExecutorCompany relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ExecutorCompany)
                .WithMany(e => e.ProjectAsExecutor)
                .HasForeignKey(p => p.ExecutorCompanyID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure EmployeeOnProject - Project relationship
            modelBuilder.Entity<EmployeeOnProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeesOnProject)
                .HasForeignKey(ep => ep.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure EmployeeOnProject - Employee relationship
            modelBuilder.Entity<EmployeeOnProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set field and restrictions
            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Project>()
                .Property(p => p.StartDate)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.EndDate)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Priority)
                .IsRequired();

            // Add check constraint at database level
            //Priority must always be from 1 to 5
            modelBuilder.Entity<Project>()
                .ToTable(t => t.HasCheckConstraint("CK_Project_Priority", "[Priority] BETWEEN 1 AND 5"));


            modelBuilder.Entity<CustomerCompany>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<ExecutorCompany>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Employee>()
                .Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(500);

            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DbInitializer
    {
        public static void Seed(this AppContextDB context)
        {

            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee { FullName = "Соболев Богдан Ильич", Email = "piwekiy_uko29@outlook.com" },
                    new Employee { FullName = "Семина Алёна Адамовна", Email = "lunini-fugu82@outlook.com" },
                    new Employee { FullName = "Глебова Анастасия Давидовна", Email = "ful-uridujo67@hotmail.com" },
                    new Employee { FullName = "Калашникова Варвара Сергеевна", Email = "gajati_genu74@gmail.com" },
                    new Employee { FullName = "Родионов Вадим Константинович", Email = "sago_wayana67@mail.com" },
                    new Employee { FullName = "Алексеев Даниил Павлович", Email = "nozara_bama12@hotmail.com" },
                    new Employee { FullName = "Петрова Кира Даниловна", Email = "fifes-ojosi93@aol.com" },
                    new Employee { FullName = "Куликов Егор Давидович", Email = "zawer-ihaca54@mail.com" },
                    new Employee { FullName = "Чернышев Сергей Михайлович", Email = "munumoc_uce37@aol.com" },
                    new Employee { FullName = "Кондратьев Ярослав Егорович", Email = "zecubu-hiti92@outlook.com" }

                    );
            }
            if (!context.CustomerCompanies.Any())
            {
                context.CustomerCompanies.AddRange(
                    new CustomerCompany { Name = "Городская больница 5" },
                    new CustomerCompany { Name = "База отдыха \"Домовенок\"" }
                );
            }
            if (!context.ExecutorCompanies.Any())
            {
                context.ExecutorCompanies.AddRange(
                    new ExecutorCompany { Name = "Горизонт" },
                    new ExecutorCompany { Name = "Альфа" },
                    new ExecutorCompany { Name = "ProProject" }
                );
            }
            context.SaveChanges();
        }
    }
}
