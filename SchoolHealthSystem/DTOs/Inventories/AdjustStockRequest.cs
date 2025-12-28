namespace SchoolHealthSystem.DTOs.Inventories
{
    public class AdjustStockRequest
    {
        public int AdjustmentQuantity { get; set; } // Positive for add, negative for subtract
        public string Reason { get; set; } = string.Empty;
    }
}
