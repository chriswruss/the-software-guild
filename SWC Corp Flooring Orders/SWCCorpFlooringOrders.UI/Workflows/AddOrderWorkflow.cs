using SWCCorpFlooringOrders.BLL;
using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Enums;
using SWCCorpFlooringOrders.Models.Responses;
using System;

namespace SWCCorpFlooringOrders.UI.Workflows {
    public class AddOrderWorkflow {
        private string _orderDate;
        private string _customerName;
        private string _state;
        private string _productType;
        private decimal _area;
        private Product _productSelected;
        private YesNo _code;

        public void Execute() {
            Console.Clear();

            ConsoleIO prompt = new ConsoleIO();
            OrderManager orderManager = OrderManagerFactory.Create();
            Console.WriteLine("Add Order");
            Console.WriteLine("*********");
            _orderDate = prompt.GetOrderDate();
            _customerName = prompt.GetCustomerName();
            _state = prompt.GetCustomerState();
            _productSelected = prompt.ProductPickList();
            _productType = _productSelected.ProductName;
            _area = prompt.GetArea();

            // Sends the information provided off to the order manager for validation
            ValidateAddOrderResponse validateResponse = orderManager.ValidateAddOrder(_orderDate, _customerName, _state, _productType, _area);

            if (!validateResponse.Success) {
                prompt.PrintError(validateResponse.Code);
            }
            else {
                // Prints the order to the screen so user can verify information
                prompt.PrintOrder(_orderDate, validateResponse.Order);
                // Get confirmation on whether to perform the process or not
                do {
                    _code = prompt.GetConfirmation("add");
                } while (_code == YesNo.Invalid);
                // If they choose not to make changes, do nothing and print that to the screen
                if (_code == YesNo.No) {
                    prompt.PrintError(ErrorCode.DidntConfirmOrder);
                }
                else {
                    // Send the Order off to the ordermanager to be added to the file
                    orderManager.AddOrder(validateResponse.Order);
                    prompt.PrintSuccessMessage("Order added successfully.");
                }
            }
        }
    }
}
