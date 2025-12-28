namespace SchoolHealthSystem.DTOs.Inventories
{
    public class InventoryResponse
    {
        public Guid Id { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Computed properties
        public bool IsExpired => ExpiryDate.Date < DateTime.UtcNow.Date;
        public bool IsExpiringSoon => ExpiryDate.Date <= DateTime.UtcNow.Date.AddDays(30) && !IsExpired;
        public bool IsLowStock => Quantity <= 10;
        public string Status => GetStatus();

        private string GetStatus()
        {
            if (IsExpired) return "Hết hạn";
            if (IsExpiringSoon) return "Sắp hết hạn";
            if (IsLowStock) return "Sắp hết";
            return "Bình thường";
        }
    }
}
