namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class PostponeVaccinationRequest
    {
        public DateTime NewDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
