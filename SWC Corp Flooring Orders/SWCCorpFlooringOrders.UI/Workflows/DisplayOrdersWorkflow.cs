using SWCCorpFlooringOrders.BLL;
using System;
using System.Collections.Generic;
using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Responses;

namespace SWCCorpFlooringOrders.UI.Workflows {
    public class DisplayOrdersWorkflow {
        private static string _orderDate;

        public void Execute() {
            ConsoleIO prompt = new ConsoleIO();
            List<Order> orders = new List<Order>();
            OrderManager orderManager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Display Orders");
            Console.WriteLine("**************");
            _orderDate = prompt.GetOrderDate();

            // Sends the order date off to the order manager to display the orders
            OrdersDisplayResponse response = orderManager.DisplayOrders(_orderDate);

            if (response.Success) {
                PrintOrders(response.Orders);
                prompt.PressEnterToContinue();
            }
            else {
                prompt.PrintError(response.Code);
            }
        }
        private void PrintOrders(List<Order> orders) {
            Console.Clear();

            foreach (var order in orders) {
                Console.WriteLine("*********************************");
                Console.WriteLine($"{order.Number} | {_orderDate}");
                Console.WriteLine(order.CustomerName.Replace('~', ','));
                Console.WriteLine(order.State);
                Console.WriteLine($"Product type: {order.ProductType}");
                Console.WriteLine($"Materials: {order.MaterialCost:c}");
                Console.WriteLine($"Labor: {order.LaborCost:c}");
                Console.WriteLine($"Tax: {order.Tax:c}");
                Console.WriteLine($"Total: {order.Total:c}");
                Console.WriteLine("*********************************");
            }
        }
    }
}
