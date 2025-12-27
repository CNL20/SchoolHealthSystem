namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class VaccinationResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string VaccineName { get; set; } = string.Empty;
        public DateTime VaccinationDate { get; set; }
        public string? Note { get; set; }
    }
}
