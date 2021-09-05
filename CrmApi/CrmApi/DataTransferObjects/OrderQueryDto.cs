using System;

namespace CrmApi.DataTransferObjects
{
    public class OrderQueryDto : OrderFilterQueryDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
