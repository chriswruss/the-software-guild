using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Interfaces;
using SWCCorpFlooringOrders.Models.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Data {
    public class OrderProdRepository : IOrderRepository {
        private string[] _orders;
        private string[] _orderData;
        private Order _corruptFile = new Order {
            Number = -1
        };
        private List<Order> _ordersList = new List<Order>();
        private string _orderDate; // This string gets used several times throughout many methods

        // Returns a list of orders that are pulled from the file with the given date
        public List<Order> LoadOrders(string orderDate) {
            _orderDate = orderDate;
            _ordersList = new List<Order>();
            try {
                // Checks if the file exists
                if (!File.Exists($"{Paths.ordersFolderFilePath}{orderDate}.txt")) {
                    _ordersList = null;
                }
                else {
                    // Reads all the lines into the orders array
                    _orders = File.ReadAllLines($"{Paths.ordersFolderFilePath}{orderDate}.txt");
                    for (int i = 1; i < _orders.Length; i++) {
                        _orderData = _orders[i].Split(',');

                        int iterator = 0; // Used to make copy and paste easier for the Order creation directly below

                        // Create a temporary order with all the information pulled from the file
                        Order temp = new Order {
                            Number = int.Parse(_orderData[iterator++]),
                            CustomerName = _orderData[iterator++],
                            State = _orderData[iterator++],
                            TaxRate = decimal.Parse(_orderData[iterator++]),
                            ProductType = _orderData[iterator++],
                            Area = decimal.Parse(_orderData[iterator++]),
                            CostPerSquareFoot = decimal.Parse(_orderData[iterator++]),
                            LaborCostPerSquareFoot = decimal.Parse(_orderData[iterator++]),
                            MaterialCost = decimal.Parse(_orderData[iterator++]),
                            LaborCost = decimal.Parse(_orderData[iterator++]),
                            Tax = decimal.Parse(_orderData[iterator++]),
                            Total = decimal.Parse(_orderData[iterator++]),
                        };

                        // Add the temporary order to the orders list
                        _ordersList.Add(temp);
                    }
                }
            }
            catch {
                // If the orders file is corrupt, remove every element in the orders list and add a corrupt order
                _ordersList.RemoveRange(0, _ordersList.Count());
                _ordersList.Add(_corruptFile);
            }

            return _ordersList;
        }

        // Used only when adding an order
        public void SaveAddedOrder(Order order) {
            // Checks that the file exists, if it does, delete it
            if (File.Exists($"{Paths.ordersFolderFilePath}{_orderDate}.txt")) {
                File.Delete($"{Paths.ordersFolderFilePath}{_orderDate}.txt");
            }
            
            if (_ordersList == null) { // LoadOrders returns null if the file doesn't already exist
                _ordersList = new List<Order>(); // Create a new list
            }

            // Add the order to the list
            _ordersList.Add(order);

            // Append the order to the orders file
            using(StreamWriter writer = File.AppendText($"{Paths.ordersFolderFilePath}{_orderDate}.txt")) {
                writer.WriteLine(PrintFormatting.orderFileaHeader);
                foreach (var o in _ordersList) {
                    // When writing, replace any commas with tildes
                    writer.WriteLine($"{o.Number},{o.CustomerName.Replace(',', '~')},{o.State},{o.TaxRate:f},{o.ProductType},{o.Area:f},{o.CostPerSquareFoot:f},{o.LaborCostPerSquareFoot:f},{o.MaterialCost:f},{o.LaborCost:f},{o.Tax:f},{o.Total:f}");
                }
            }
        }

        // Used when editing or removing an order
        public void SaveOrder(List<Order> orders) {
            // Checks if the order file exsits, if so, delete it
            if (File.Exists($"{Paths.ordersFolderFilePath}{_orderDate}.txt")) {
                File.Delete($"{Paths.ordersFolderFilePath}{_orderDate}.txt");
            }

            // Write the orders list to the file
            using (StreamWriter writer = File.AppendText($"{Paths.ordersFolderFilePath}{_orderDate}.txt")) {
                writer.WriteLine(PrintFormatting.orderFileaHeader);
                foreach (var o in _ordersList) {
                    // When writing, replace any commas with tildes
                    writer.WriteLine($"{o.Number},{o.CustomerName.Replace(',', '~')},{o.State},{o.TaxRate:f},{o.ProductType},{o.Area:f},{o.CostPerSquareFoot:f},{o.LaborCostPerSquareFoot:f},{o.MaterialCost:f},{o.LaborCost:f},{o.Tax:f},{o.Total:f}");
                }
            }
        }
    }
}