namespace CrmApi.DataTransferObjects
{
    public class CreateOrderItemDto
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
    }
}