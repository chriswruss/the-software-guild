using SWCCorpFlooringOrders.BLL;
using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Enums;
using SWCCorpFlooringOrders.Models.Responses;
using SWCCorpFlooringOrders.Models.Tools;
using System;

namespace SWCCorpFlooringOrders.UI.Workflows {
    public class EditOrderWorkflow {
        ConsoleIO prompt = new ConsoleIO();
        private string _orderDate;
        private int _orderNumber;
        private bool _madeChanges = false;
        private Order _oldOrder;
        private YesNo _code;

        public void Execute() {
            Console.Clear();

            OrderManager orderManager = OrderManagerFactory.Create();
            Console.WriteLine("Edit Order");
            Console.WriteLine("**********");
            _orderDate = prompt.GetOrderDate();
            _orderNumber = prompt.GetOrderNumber();

            // Sends the order date and number off to the order manager for validation
            GetOrderResponse getOrderResponse = orderManager.GetOrder(_orderDate, _orderNumber);

            // Checks if the date is invalid
            if (!getOrderResponse.IsValidDate) {
                prompt.PrintError(ErrorCode.NotAValidDate);
            }
            // Checks if the date is in the future
            else if (!getOrderResponse.IsFutureDate) {
                prompt.PrintError(ErrorCode.NotAFutureDate);
            }
            else if (!getOrderResponse.Success) {
                prompt.PrintError(getOrderResponse.Code);
            }
            else {
                string strInput;
                Product product;
                decimal decInput;
                // Clone the order so it can be stored in _oldOrder and be used to compare the old and new side by side
                _oldOrder = orderManager.Clone(getOrderResponse.Order);

                Console.WriteLine("To skip changing a certain field, simply press enter without entering data.");

                // Gets the new customer name if they entered something
                strInput = prompt.GetCustomerName(getOrderResponse.Order.CustomerName);
                if (prompt.DidChange(strInput)) {
                    getOrderResponse.Order.CustomerName = strInput;
                    _madeChanges = true;
                }

                // Gets the new customer state if they entered something
                strInput = prompt.GetCustomerState(getOrderResponse.Order.State);
                if (prompt.DidChange(strInput)) {
                    getOrderResponse.Order.State = strInput;
                    _madeChanges = true;
                }

                // Gets the new product if they entered something
                product = prompt.ProductPickList(getOrderResponse.Order.ProductType);
                if (product.ProductName != null) {
                    getOrderResponse.Order.ProductType = product.ProductName;
                    _madeChanges = true;
                }

                // Gets the new area if they entered something
                decInput = prompt.GetArea(getOrderResponse.Order.Area);
                if (decInput != -1) {
                    getOrderResponse.Order.Area = decInput;
                    _madeChanges = true;
                }

                // Checks if the user made any changes to any of the above fields
                if (!_madeChanges) {
                    prompt.PrintSuccessMessage("\nNo changes were made.");
                }

                else {
                    // Sends the edited order off to the order manager for validation
                    ValidateEditOrderResponse response = orderManager.ValidateEditOrder(_orderDate, getOrderResponse.Order);

                    if (!response.Success) {
                        prompt.PrintError(response.Code);
                    }
                    else {
                        // Prints the old order information next to the edited order information
                        prompt.PrintChanges(_oldOrder, getOrderResponse.Order);
                        // Gets confirmation on whether to edit or not
                        do {
                            _code = prompt.GetConfirmation("edit");
                        } while (_code == YesNo.Invalid);
                        // If the user decides not to make changes, print that to the screen
                        if (_code == YesNo.No) {
                            prompt.PrintError(ErrorCode.DidntConfirmOrder);
                        }
                        else {
                            orderManager.EditOrder(response.Orders);
                            prompt.PrintSuccessMessage("Order edited successfully.");
                        }
                    }
                }
            }
        }
    }
}
