using System;
using System.Collections.Generic;
using System.Linq;
using CrmApi.Models;

namespace CrmApi.Utilities
{
    public static class AnalyticsUtil
    {
        public const string MapOrdersDateKeyFormat = "dd.MM.yy";
        
        public static Dictionary<string, List<Order>> GetMapOrders(IEnumerable<Order> orders)
        {
            var daysOrders = new Dictionary<string, List<Order>>();

            foreach (var order in orders)
            {
                if (order.Date == DateTime.Today)
                {
                    continue;
                }

                var orderDateStr = order.Date.ToString(MapOrdersDateKeyFormat);

                if (!daysOrders.ContainsKey(orderDateStr))
                {
                    daysOrders[orderDateStr] = new List<Order>();
                }
                
                daysOrders[orderDateStr].Add(order);
            }
            
            return daysOrders;
        }

        public static double CalculateTotalRevenues(IList<Order> orders)
        {
            return orders.Aggregate(0.0, (acc, order) => 
                order.OrderItems.Aggregate(0.0, (orderTotal, orderItem) => 
                    orderTotal + orderItem.Cost * orderItem.Quantity));
        }
    }
}