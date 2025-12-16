namespace SchoolHealthSystem.Models
{
    public class MedicineInventory
    {
        public Guid Id { get; set; }

        public string MedicineName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}