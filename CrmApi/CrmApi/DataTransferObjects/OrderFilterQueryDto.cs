using System;

namespace CrmApi.DataTransferObjects
{
    public class OrderFilterQueryDto
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Order { get; set; }
    }
}