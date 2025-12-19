namespace SchoolHealthSystem.DTOs.Students
{
    public class CreateStudentRequest
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
        public string? Note { get; set; }
    }
}
