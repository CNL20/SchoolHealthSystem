namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class CreateVaccinationRequest
    {
        public Guid StudentId { get; set; }
        public string VaccineName { get; set; } = string.Empty;
        public DateTime? VaccinationDate { get; set; }    // optional: if not provided use DateTime.UtcNow in mapping/service
        public string? Note { get; set; }
    }
}
