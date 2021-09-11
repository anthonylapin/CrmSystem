using System;
using System.Collections.Generic;
using CrmApi.Contracts;
using CrmApi.Models;

namespace CrmApi.Utilities
{
    public class AnalyticsBuilder : IBuilder<AnalyticsOverview>
    {
        private readonly AnalyticsOverview _analyticsOverview = new();

        public AnalyticsBuilder FromOrders(IList<Order> orders)
        {
            var daysOrders = AnalyticsUtil.GetMapOrders(orders);
            var yesterdayOrdersKey = DateTime.Now.AddDays(-1).ToString(AnalyticsUtil.MapOrdersDateKeyFormat);
            var yesterdayOrders = daysOrders.ContainsKey(yesterdayOrdersKey)
                ? daysOrders[yesterdayOrdersKey]
                : new List<Order>();
            
            // number of orders yesterday
            var yesterdayOrdersNumber = yesterdayOrders.Count;
            
            // number of orders
            var totalOrdersNumber = orders.Count;
            
            // number of days in total
            var daysNumber = daysOrders.Count;
            
            // Number of orders in a day
            int ordersPerDayNumber = totalOrdersNumber / daysNumber;
            
            // % of amount of orders
            // ((yesterday's orders / number of orders per day) - 1) % 100
            var ordersPercent = ((double)yesterdayOrdersNumber / ordersPerDayNumber - 1) * 100;
            
            // total revenues
            var totalRevenues = AnalyticsUtil.CalculateTotalRevenues(orders);
            
            // revenue per day
            double revenuePerDay = totalRevenues / daysNumber;
            
            // yesterday gain
            var yesterdayRevenue = AnalyticsUtil.CalculateTotalRevenues(yesterdayOrders);
            
            // revenue percent
            var revenuePercent = (yesterdayRevenue / revenuePerDay - 1) * 100;

            // compare revenue
            var compareRevenue = yesterdayRevenue - revenuePerDay;
            
            // compare orders number
            var compareNumber = yesterdayOrdersNumber - ordersPerDayNumber;

            _analyticsOverview.Gain = new AnalyticsOverviewItem()
            {
                Percent = Math.Abs(revenuePercent),
                Compare = Math.Abs(compareRevenue),
                Yesterday = yesterdayRevenue,
                IsHigher = revenuePercent > 0
            };

            _analyticsOverview.Orders = new AnalyticsOverviewItem()
            {
                Percent = Math.Abs(ordersPercent),
                Compare = Math.Abs(compareNumber),
                Yesterday = yesterdayOrdersNumber,
                IsHigher = ordersPercent > 0
            };

            return this;
        }
        
        public AnalyticsOverview Build()
        {
            return _analyticsOverview;
        }
    }
}