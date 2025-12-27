namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class UpdateVaccinationRequest
    {
        public string? VaccineName { get; set; }
        public DateTime? VaccinationDate { get; set; }
        public string? Note { get; set; }
    }
}
