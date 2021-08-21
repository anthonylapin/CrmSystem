using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrmApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int OrderNumber { get; set; }
        public DateTime Date { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }
        
        public IList<OrderItem> OrderItems { get; set; }
    }
}