namespace SchoolHealthSystem.DTOs.Inventories
{
    public class CreateInventoryRequest
    {
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
