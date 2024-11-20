namespace CarBookingSystemAPI.Models
{
    public class SupplierPrice
    {
        public int PriceId { get; set; }
        public int CarId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }  
    }
}
