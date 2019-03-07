using SWCCorpFlooringOrders.BLL;
using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Enums;
using SWCCorpFlooringOrders.Models.Responses;
using SWCCorpFlooringOrders.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.UI.Workflows {
    public class RemoveOrderWorkflow {
        ConsoleIO prompt = new ConsoleIO();
        private string _orderDate;
        private int _orderNumber;
        private YesNo _code;

        public void Execute() {
            Console.Clear();

            OrderManager orderManager = OrderManagerFactory.Create();
            Console.WriteLine("Remove Order");
            Console.WriteLine("************");
            _orderDate = prompt.GetOrderDate();
            _orderNumber = prompt.GetOrderNumber();

            // Sends the order date and order number off to the order manager to get the order requested
            GetOrderResponse getOrderResponse = orderManager.GetOrder(_orderDate, _orderNumber);

            if (!getOrderResponse.IsValidDate) {
                prompt.PrintError(ErrorCode.NotAValidDate);
            }
            else if (!getOrderResponse.IsFutureDate) {
                prompt.PrintError(ErrorCode.NotAFutureDate);
            }
            else if (!getOrderResponse.Success) {
                prompt.PrintError(getOrderResponse.Code);
            }
            else {
                // Sends the order to the order manager for validation
                ValidateRemoveOrderResponse response = orderManager.ValidateRemoveOrder(_orderDate, getOrderResponse.Order);

                if (!response.Success) {
                    prompt.PrintError(response.Code);
                }
                else {
                    // Prints the order to be removed to the screen for confirmation
                    prompt.PrintOrder(_orderDate, getOrderResponse.Order);
                    // Gets confirmation on whether to remove or not
                    do {
                        _code = prompt.GetConfirmation("remove");
                    } while (_code == YesNo.Invalid);
                    // If user decides not to remove, print that to the screen
                    if (_code == YesNo.No) {
                        prompt.PrintError(ErrorCode.DidntConfirmOrder);
                    }
                    else {
                        // Sends the orders list to the order manager to be written to the file
                        orderManager.RemoveOrder(response.Orders);
                        prompt.PrintSuccessMessage("Order removed successfully.");
                    }
                }
            }
        }
    }
}
