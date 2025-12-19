namespace SchoolHealthSystem.DTOs.Students
{
    public class StudentResponse
    {
        public Guid Id { get; set; }

        public string StudentCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;
        public string? Note { get; set; }

        public bool IsActive { get; set; }

        public Guid ParentId { get; set; }
    }
}
