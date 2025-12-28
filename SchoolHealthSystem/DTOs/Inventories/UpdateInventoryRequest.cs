namespace SchoolHealthSystem.DTOs.Inventories
{
    public class UpdateInventoryRequest
    {
        public string? MedicineName { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
