using SchoolHealthSystem.Models;

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
                PasswordHash = "admin123", // sau này thay bằng hash
                FullName = "System Admin",
                Role = UserRole.Admin
            };

            var manager = new User
            {
                Id = Guid.NewGuid(),
                Email = "manager@school.com",
                PasswordHash = "manager123",
                FullName = "School Manager",
                Role = UserRole.Manager
            };

            var nurse = new User
            {
                Id = Guid.NewGuid(),
                Email = "nurse@school.com",
                PasswordHash = "nurse123",
                FullName = "School Nurse",
                Role = UserRole.Nurse
            };

            var parent = new User
            {
                Id = Guid.NewGuid(),
                Email = "parent@school.com",
                PasswordHash = "parent123",
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