using System.Collections.Generic;

namespace CrmApi.Models
{
    public class AnalyticsDto
    {
        public IEnumerable<ChartDto> Chart { get; set; }
        public double Average { get; set; }
    }
}