namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class VaccinationConsentRequest
    {
        public Guid StudentId { get; set; }
        public string VaccineName { get; set; } = string.Empty;
        public DateTime? PreferredDate { get; set; }
        public string? Note { get; set; }      // parent comment / additional info
        public bool Consent { get; set; } = true; // true = give consent
    }
}
