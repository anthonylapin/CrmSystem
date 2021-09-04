using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrmApi.Models;

namespace CrmApi.DataTransferObjects
{
    public class CreateOrderDto
    {
        public IList<CreateOrderItemDto> OrderItems { get; set; }
    }
}
