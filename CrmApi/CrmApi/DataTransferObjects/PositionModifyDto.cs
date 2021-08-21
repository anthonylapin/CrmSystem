using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmApi.DataTransferObjects
{
    public class PositionModifyDto
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public int CategoryId { get; set; }
    }
}
