using System;

namespace CrmApi.Models
{
    public class OrderQueryDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Order { get; set; }
    }
}
