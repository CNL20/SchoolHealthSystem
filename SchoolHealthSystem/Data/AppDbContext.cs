using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
        public DbSet<MedicationRequest> MedicationRequests => Set<MedicationRequest>();
        public DbSet<VaccinationRecord> VaccinationRecords => Set<VaccinationRecord>();
        public DbSet<MedicineInventory> MedicineInventories => Set<MedicineInventory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Student - Parent
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Parent)
                .WithMany(u => u.Students)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthRecord - Student
            modelBuilder.Entity<HealthRecord>()
                .HasOne(h => h.Student)
                .WithMany(s => s.HealthRecords)
                .HasForeignKey(h => h.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // HealthRecord - Nurse
            modelBuilder.Entity<HealthRecord>()
                .HasOne(h => h.Nurse)
                .WithMany()
                .HasForeignKey(h => h.NurseId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicationRequest - Student
            modelBuilder.Entity<MedicationRequest>()
                .HasOne(m => m.Student)
                .WithMany(s => s.MedicationRequests)
                .HasForeignKey(m => m.StudentId);

            // MedicationRequest - RequestedByParent
            modelBuilder.Entity<MedicationRequest>()
                .HasOne(m => m.RequestedByParent)
                .WithMany()
                .HasForeignKey(m => m.RequestedByParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicationRequest - ApprovedByNurse
            modelBuilder.Entity<MedicationRequest>()
                .HasOne(m => m.ApprovedByNurse)
                .WithMany()
                .HasForeignKey(m => m.ApprovedByNurseId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccinationRecord - Student
            modelBuilder.Entity<VaccinationRecord>()
                .HasOne(v => v.Student)
                .WithMany(s => s.VaccinationRecords)
                .HasForeignKey(v => v.StudentId);


            base.OnModelCreating(modelBuilder);
        }
    }
}