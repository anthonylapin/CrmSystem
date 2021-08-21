using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrmApi.Models;

namespace CrmApi.DataTransferObjects
{
    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
    }
}
