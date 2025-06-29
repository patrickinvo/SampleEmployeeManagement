using EmployeeManagement.Models;

namespace EmployeeManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Employees.Any())
            {
                return; // DB has been seeded
            }

            var employees = new Employee[]
            {
                    new Employee{FirstName="John", LastName="Doe", Email="john.doe@company.com", Department="IT", Position="Developer", Status=EmployeeStatus.ACTIVE, ProfileImage="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face"},
                    new Employee{FirstName="Jane", LastName="Smith", Email="jane.smith@company.com", Department="HR", Position="Manager", Status=EmployeeStatus.ACTIVE, ProfileImage="https://images.unsplash.com/photo-1494790108755-2616c1d7c0e3?w=40&h=40&fit=crop&crop=face"},
                    new Employee{FirstName="Mike", LastName="Johnson", Email="mike.johnson@company.com", Department="Finance", Position="Analyst", Status=EmployeeStatus.ON_LEAVE, ProfileImage="https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=40&h=40&fit=crop&crop=face"},
                    new Employee{FirstName="Sarah", LastName="Wilson", Email="sarah.wilson@company.com", Department="Marketing", Position="Specialist", Status=EmployeeStatus.INACTIVE, ProfileImage="https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=40&h=40&fit=crop&crop=face"},
                    new Employee{FirstName="David", LastName="Brown", Email="david.brown@company.com", Department="Sales", Position="Representative", Status=EmployeeStatus.ACTIVE, ProfileImage="https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=40&h=40&fit=crop&crop=face"},
                    new Employee{FirstName="Lisa", LastName="Garcia", Email="lisa.garcia@company.com", Department="IT", Position="Senior Developer", Status=EmployeeStatus.ACTIVE, ProfileImage="https://images.unsplash.com/photo-1544725176-7c40e5a71c5e?w=40&h=40&fit=crop&crop=face"}
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }
    }
}