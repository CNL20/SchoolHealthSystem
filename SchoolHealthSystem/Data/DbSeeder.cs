using SchoolHealthSystem.Models;
using SchoolHealthSystem.Helpers;

namespace SchoolHealthSystem.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Users.Any())
                return;

            // 1️⃣ Seed Users
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@school.com",
                PasswordHash = PasswordHasher.Hash("Admin123!"), // Hash password đúng cách
                FullName = "System Admin",
                Role = UserRole.Admin
            };            
            
            var manager = new User
            {
                Id = Guid.NewGuid(),
                Email = "manager@school.com",
                PasswordHash = PasswordHasher.Hash("Manager123!"), // Hash password
                FullName = "School Manager",
                Role = UserRole.Manager
            };

            var nurse = new User
            {
                Id = Guid.NewGuid(),
                Email = "nurse@school.com",
                PasswordHash = PasswordHasher.Hash("Nurse123!"), // Hash password
                FullName = "School Nurse",
                Role = UserRole.Nurse
            };

            var parent = new User
            {
                Id = Guid.NewGuid(),
                Email = "parent@school.com",
                PasswordHash = PasswordHasher.Hash("Parent123!"), // Hash password
                FullName = "Nguyen Van A",
                Role = UserRole.Parent
            };

            context.Users.AddRange(admin, manager, nurse, parent);
            await context.SaveChangesAsync();

            // 2️⃣ Seed Student
            var student = new Student
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van B",
                DateOfBirth = new DateTime(2015, 5, 20),
                ParentId = parent.Id
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();
        }
    }
}