using System;

namespace CrmApi.Models
{
    public class AnalyticsOverviewItem
    {
        private double _percent;
        private double _compare;
        
        public double Percent { get => Math.Abs(_percent); set => _percent = value; }
        public double Compare { get => Math.Abs(_compare); set => _compare = value; }
        public double Yesterday { get; set; }
        public bool IsHigher { get; set; }
    }
    
    public class AnalyticsOverview
    {
        public AnalyticsOverviewItem Gain { get; set; }
        public AnalyticsOverviewItem Orders { get; set; }
    }
}