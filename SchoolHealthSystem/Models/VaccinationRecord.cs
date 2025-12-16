namespace SchoolHealthSystem.Models
{
    public class VaccinationRecord
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public string VaccineName { get; set; } = string.Empty;

        public DateTime VaccinationDate { get; set; }

        public string? Note { get; set; }

        public Student? Student { get; set; }
    }
}