using NUnit.Framework;
using SWCCorpFlooringOrders.BLL;
using SWCCorpFlooringOrders.Data;
using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Interfaces;
using SWCCorpFlooringOrders.Models.Responses;

namespace SWCCorpFlooringOrders.Tests {
    [TestFixture]
    public class OrderManagerTests {
        [TestCase("11111111", "Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tests order date that isn't in the future
        [TestCase("13012011", "Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tests order date that isn't a valid date
        [TestCase("11112222", "", "OH", "Laminate", 100.00, 0, 0, false)] // Tests empty customer name
        [TestCase("11112222", "$Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tests customer name with invalid characters
        [TestCase("11112222", "chris", "OHS", "Laminate", 100.00, 0, 0, false)] // Tets bad state abbreviation
        [TestCase("11112222", "chris", "", "Laminate", 100.00, 0, 0, false)] // Tets empty state abbreviation
        [TestCase("11112222", "chris", "TN", "Laminate", 100.00, 0, 0, false)] // Tests state that isn't available to sell to
        [TestCase("11112222", "chris", "OH", "", 100.00, 0, 0, false)] // Tests empty product type
        [TestCase("11112222", "chris", "OH", "Linoleum", 100.00, 0, 0, false)] // Tests invalid product type
        [TestCase("11112222", "chris", "OH", "Laminate", -15, 0, 0, false)] // Tests negative area
        [TestCase("11112222", "chris", "OH", "Laminate", 99.99, 0, 0, false)] // Tests area too small
        [TestCase("11112222", "chris", "OH", "Laminate", 100.00, 24.0625, 409.0625, true)]
        [TestCase("11112222", "chris.russ", "OH", "Laminate", 100.00, 24.0625, 409.0625, true)]
        [TestCase("11112222", "chris", "PA", "Laminate", 100.00, 20.2125, 405.2125, true)]
        [TestCase("11112222", "chris", "OH", "Wood", 100.00, 61.875, 1051.875, true)]
        [TestCase("11112222", "chris", "OH", "Laminate", 200.00, 48.125, 818.125, true)]
        public void AddOrderValidation(string orderDate, string customerName, string state, string productType, decimal area, decimal tax, decimal total, bool expectedResult) {
            IOrderRepository _orderRepository = new OrderProdRepository();
            OrderManager _orderManager = OrderManagerFactory.Create();

            ValidateAddOrderResponse response = _orderManager.ValidateAddOrder(orderDate, customerName, state, productType, area);

            Assert.AreEqual(expectedResult, response.Success);
            Assert.AreEqual(tax, response.Order.Tax);
            Assert.AreEqual(total, response.Order.Total);
        }

        [TestCase(0, "11111111", "Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tets order with number 0
        [TestCase(-1, "11111111", "Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tests order with negative number
        [TestCase(3, "11111111", "Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tests order number that doesn't exist
        [TestCase(1, "11112222", "", "OH", "Laminate", 100.00, 0, 0, false)] // Tests empty customer name
        [TestCase(1, "11112222", "$Chris", "OH", "Laminate", 100.00, 0, 0, false)] // Tests customer name with invalid characters
        [TestCase(1, "11112222", "chris", "OHS", "Laminate", 100.00, 0, 0, false)] // Tets bad state abbreviation
        [TestCase(1, "11112222", "chris", "", "Laminate", 100.00, 0, 0, false)] // Tets empty state abbreviation
        [TestCase(1, "11112222", "chris", "TN", "Laminate", 100.00, 0, 0, false)] // Tests state that isn't available to sell to
        [TestCase(1, "11112222", "chris", "OH", "", 100.00, 0, 0, false)] // Tests empty product type
        [TestCase(1, "11112222", "chris", "OH", "Linoleum", 100.00, 0, 0, false)] // Tests invalid product type
        [TestCase(1, "11112222", "chris", "OH", "Laminate", -15, 0, 0, false)] // Tests negative area
        [TestCase(1, "11112222", "chris", "OH", "Laminate", 99.99, 0, 0, false)] // Tests area too small
        [TestCase(2, "11112222", "chris", "OH", "Laminate", 100.00, 24.0625, 409.0625, true)]
        [TestCase(1, "11112222", "chris", "OH", "Laminate", 100.00, 24.0625, 409.0625, true)]
        [TestCase(1, "11112222", "chris.russ", "OH", "Laminate", 100.00, 24.0625, 409.0625, true)]
        [TestCase(1, "11112222", "chris", "PA", "Laminate", 100.00, 20.2125, 405.2125, true)]
        [TestCase(1, "11112222", "chris", "OH", "Wood", 100.00, 61.875, 1051.875, true)]
        [TestCase(1, "11112222", "chris", "OH", "Laminate", 200.00, 48.125, 818.125, true)]
        public void EditOrderValidation(int orderNumber, string orderDate, string customerName, string state, string productType, decimal area, decimal tax, decimal total, bool expectedResult) {
            IOrderRepository _orderRepository = new OrderProdRepository();
            OrderManager _orderManager = OrderManagerFactory.Create();
            Order order = new Order {
                Number = orderNumber,
                CustomerName = customerName,
                State = state,
                ProductType = productType,
                Area = area
            };

            ValidateEditOrderResponse response = _orderManager.ValidateEditOrder(orderDate, order);

            Assert.AreEqual(expectedResult, response.Success);
            Assert.AreEqual(tax, order.Tax);
            Assert.AreEqual(total, order.Total);
        }

        [TestCase(0, "11111111", "Chris", "OH", "Laminate", 100.00, false)] // Tets order with number 0
        [TestCase(-1, "11111111", "Chris", "OH", "Laminate", 100.00, false)] // Tests order with negative number
        [TestCase(3, "11111111", "Chris", "OH", "Laminate", 100.00, false)] // Tests order number that doesn't exist
        [TestCase(1, "11112222", "chris", "OH", "Laminate", 200.00, true)]
        public void RemoveOrderValidation(int orderNumber, string orderDate, string customerName, string state, string productType, decimal area, bool expectedResult) {
            IOrderRepository _orderRepository = new OrderProdRepository();
            OrderManager _orderManager = OrderManagerFactory.Create();
            Order order = new Order {
                Number = orderNumber,
                CustomerName = customerName,
                State = state,
                ProductType = productType,
                Area = area
            };

            ValidateRemoveOrderResponse response = _orderManager.ValidateRemoveOrder
(orderDate, order);

            Assert.AreEqual(expectedResult, response.Success);
        }


    }
}