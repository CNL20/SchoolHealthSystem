using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.Students
{
    public class UpdateStudentRequest
    {
        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? ClassName { get; set; }

        public Guid? ParentId { get; set; }

        public string? Note { get; set; }

        public bool? IsActive { get; set; }
    }
}
