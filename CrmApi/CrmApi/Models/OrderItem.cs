namespace CrmApi.Models
{
    public class  OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}