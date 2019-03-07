using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace SWCCorpFlooringOrders.Data {
    public class OrderTestRepository : IOrderRepository {
        private static Order _order = new Order {
            Number = 1,
            CustomerName = "Order Test",
            State = "OH",
            TaxRate = 6.25m,
            ProductType = "Carpet",
            Area = 125.56m,
            CostPerSquareFoot = 2.25m,
            LaborCostPerSquareFoot = 2.10m,
            MaterialCost = 250.56m,
            LaborCost = 230.26m,
            Tax = 10.21m,
            Total = 267.89m
        };

        private static Order _order2 = new Order {
            Number = 2,
            CustomerName = "Order Test 2",
            State = "OH",
            TaxRate = 6.25m,
            ProductType = "Wood",
            Area = 105.56m,
            CostPerSquareFoot = 1.25m,
            LaborCostPerSquareFoot = 2.90m,
            MaterialCost = 220.56m,
            LaborCost = 200.26m,
            Tax = 101.21m,
            Total = 467.89m
        };

        private List<Order> _orders = new List<Order>();

        public List<Order> LoadOrders(string orderNumber) {
            _orders.Add(_order);
            _orders.Add(_order2);
            return _orders;
        }

        public void SaveOrder(List<Order> orders) {
            _orders = orders;
        }

        public void SaveAddedOrder(Order order) {
            _order = order;
        }
    }
}
