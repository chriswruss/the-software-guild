using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Enums;
using SWCCorpFlooringOrders.Models.Interfaces;
using SWCCorpFlooringOrders.Models.Responses;
using SWCCorpFlooringOrders.Models.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SWCCorpFlooringOrders.BLL {
    public class OrderManager {
        private IOrderRepository _orderRepository;
        private Order _order = new Order();
        private State _state = new State();
        private Product _product = new Product();
        private const string COULD_NOT_FIND_FILE = "CFF";
        private const string DONT_SELL_TO_STATE = "DSS";
        private const string FILE_IS_CORRUPT = "FIC";
        private const string FILE_IS_EMPTY = "FIE";
        private const string BAD_STATE = "BST";
        private ErrorCode _validation;
        private List<Order> _loadedOrders = new List<Order>();

        public OrderManager(IOrderRepository repository) {
            _orderRepository = repository;
        }

        // Gets an order from a file based off of order date and order number
        public GetOrderResponse GetOrder(string orderDate, int orderNumber) {
            bool _foundMatch = false;
            // Sets the Orders list in the class to the orders loaded from the file
            GetOrderResponse response = new GetOrderResponse {
                Orders = _orderRepository.LoadOrders(orderDate),
                Order = new Order()
            };
            // Edit order and Remove order use the GetOrder method, so, I'm storing the loaded orders in a List 
            // to keep from reopening the file when using EditOrder and RemoveOrder
            _loadedOrders = response.Orders;

            // Checks that the date provided is a valid date
            if (!DateHandler.IsValidDate(orderDate)) {
                response.Success = false;
                response.Code = ErrorCode.NotAValidDate;
                response.IsValidDate = false;
                return response;
            }

            // Checks that the date is in the future (dates not in the future can't be edited or removed)
            if (!DateHandler.IsFutureDate(orderDate)) {
                response.Success = false;
                response.Code = ErrorCode.NotAFutureDate;
                response.IsFutureDate = false;
                return response;
            }

            // Checks if a file exists with the given order date
            if (!File.Exists(Paths.ordersFolderFilePath + orderDate + ".txt")) {
                response.Success = false;
                response.Code = ErrorCode.CouldNotFindFile;
                return response;
            }

            // Looks for an order matching the order number if file isn't empty
            if (response.Orders != null) {
                foreach (var o in response.Orders) {
                    if (o.Number == orderNumber) {
                        response.Order = o;
                        response.Success = true;
                        _foundMatch = true;
                        break;
                    }
                }
            }
            else {
                response.Success = false;
                response.Code = ErrorCode.FileIsEmpty;
                return response;
            }

            // If no match was found, return
            if (!_foundMatch) {
                response.Success = false;
                response.Code = ErrorCode.OrderNumberDoesNotExist;
                return response;
            }

            return response;
        }

        // Displays all the orders based off of order date
        public OrdersDisplayResponse DisplayOrders(string orderDate) {
            OrdersDisplayResponse response = new OrdersDisplayResponse {
                Orders = _orderRepository.LoadOrders(orderDate)
            };

            // Checks the date is valid
            if (!DateHandler.IsValidDate(orderDate)) {
                response.Success = false;
                response.Code = ErrorCode.NotAValidDate;
                return response;
            }

            // LoadOrders returns null if it can't find a file
            if (response.Orders == null) {
                response.Success = false;
                response.Code = ErrorCode.CouldNotFindFile;
                return response;
            }

            // LoadOrders returns an order with number -1 if the file it's reading from is corrupt
            if (response.Orders.Any(x => x.Number == -1)) {
                response.Success = false;
                response.Code = ErrorCode.CorruptFile;
                return response;
            }

            // If no orders were added to the list, the file is empty
            if (response.Orders.Count() == 0) {
                response.Success = false;
                response.Code = ErrorCode.FileIsEmpty;
                return response;
            }

            response.Success = true;


            return response;
        }

        // Takes the neccessary information and creates an order with it.
        public ValidateAddOrderResponse ValidateAddOrder(string orderDate, string customerName, string state, string productType, decimal area) {
            ValidateAddOrderResponse response = new ValidateAddOrderResponse {
                Orders = _orderRepository.LoadOrders(orderDate), // LoadOrders takes the order date and attempts to find and open a file with the same order date, then assigns it to the response's order list.
                Order = new Order()
            };

            if (response.Orders != null && response.Orders.Any(x => x.Number == -1)) { //LoadOrders returns null if the file does not exist.
                response.Success = false;
                response.Code = ErrorCode.CorruptFile;
                return response;
            }

            if (response.Orders == null || response.Orders.Count() == 0) { // LoadOrders returns null if the file does not exist.
                response.Order.Number = 1;
            }
            else {
                response.Order.Number = response.Orders.Last().Number + 1; // Sets the order number to 1 greater than the last order number
            }

            // Checks if the date is a valid date
            if (!DateHandler.IsValidDate(orderDate)) {
                response.Success = false;
                response.Code = ErrorCode.NotAValidDate;
                return response;
            }

            // Checks that the date is in the future
            if (!DateHandler.IsFutureDate(orderDate)) {
                response.Success = false;
                response.Code = ErrorCode.NotAFutureDate;
                return response;
            }

            // Checks that the customer name is valid
            if (!IsValidCustomerName(customerName)) {
                response.Success = false;
                response.Code = ErrorCode.NameIsInvalid;
                return response;
            }

            // STATES is an array with every state's abbreviations in it, this will take the given state and run it through the array looking for a match.
            // This is done so that if the user makes a simple typo (ie ih instead of oh), it will let them know that they entered an abbreviation that isn't
            // a state abbreviation. So, mainly, it's just for more precise error messaging.
            for (int i = 0; i < ImportantValues.STATES.Length; i++) {
                if (state == ImportantValues.STATES[i]) {
                    break;
                }

                // If it gets to the last iteration and it still hasn't found a match, the given state abbreviation is invalid.
                if (i + 1 == ImportantValues.STATES.Length) {
                    response.Success = false;
                    response.Code = ErrorCode.InvalidState;
                    return response;
                }
            }

            // Get all the information from the state and then validate the information.
            _state = GetStateInfo(state);
            _validation = ValidateState(_state.Abbreviation);
            if (_validation != ErrorCode.NoError) {
                response.Success = false;
                response.Code = _validation;
                return response;
            }

            // Get all the information from the product and then validate the information.
            _product = GetProductInfo(productType);
            _validation = ValidateProduct(_product);
            if (_validation != ErrorCode.NoError) {
                response.Success = false;
                response.Code = _validation;
                return response;
            }

            // Checks that the area isn't less than the min.
            if (area < ImportantValues.MIN_AREA) {
                response.Success = false;
                response.Code = ErrorCode.AreaTooSmall;
                return response;
            }

            // Assigns all the information to the order and performs calculations on it.
            response.Order.CustomerName = customerName;
            response.Order.State = _state.Abbreviation;
            response.Order.TaxRate = _state.TaxRate;
            response.Order.ProductType = _product.ProductName;
            response.Order.Area = area;
            response.Order.CostPerSquareFoot = _product.CostPerSquareFoot;
            response.Order.LaborCostPerSquareFoot = _product.LaborCostPerSquareFoot;
            response.Order = PerformCalculation(response.Order);
            
            response.Success = true;

            return response;
        }

        // Takes the order date, the order to be edited, and a copy of the same order to be used in printing old information vs new
        public ValidateEditOrderResponse ValidateEditOrder(string orderDate, Order order) {

            ValidateEditOrderResponse response = new ValidateEditOrderResponse {
                Orders = _loadedOrders
            };

            // Checks the customer name is valid
            if (!IsValidCustomerName(order.CustomerName)) {
                response.Success = false;
                response.Code = ErrorCode.NameIsInvalid;
                return response;
            }

            // Checks that the area isn't less than the min
            if (order.Area < ImportantValues.MIN_AREA) {
                response.Success = false;
                response.Code = ErrorCode.AreaTooSmall;
                return response;
            }

            // LoadOrders will return null if it couldn't find a file
            if (response.Orders == null) {
                response.Success = false;
                response.Code = ErrorCode.CouldNotFindFile;
                return response;
            }

            // LoadOrders will return a corrupt order with number -1 if the file is corrupt
            if (response.Orders.Any(x => x.Number == -1)) {
                response.Success = false;
                response.Code = ErrorCode.CorruptFile;
                return response;
            }

            // Checks the order number against all the orders that are in the list to see if there's a match.
            if (!response.Orders.Any(x => x.Number == order.Number)){
                response.Success = false;
                response.Code = ErrorCode.OrderNumberDoesNotExist;
                return response;
            }
            
            // Get the product information and then validate it
            _product = GetProductInfo(order.ProductType);
            _validation = ValidateProduct(_product);
            if (_validation != ErrorCode.NoError) {
                response.Success = false;
                response.Code = _validation;
                return response;
            }

            // Get the state information and then validate it
            _state = GetStateInfo(order.State);
            _validation = ValidateState(_state.Abbreviation);
            if (_validation != ErrorCode.NoError) {
                response.Success = false;
                response.Code = _validation;
                return response;
            }

            // Assign all the information pulled from the product and state to the edited order and perform calculations
            order.TaxRate = GetStateInfo(order.State).TaxRate;
            order.CostPerSquareFoot = _product.CostPerSquareFoot;
            order.LaborCostPerSquareFoot = _product.LaborCostPerSquareFoot;
            order = PerformCalculation(order);
            
            response.Success = true;

            // Gets the index based off of the order number and sets the order to the order in the list
            int index = response.Orders.FindIndex(x => x.Number == order.Number);
            response.Orders[index] = order;

            return response;
        }

        // Takes the order to be removed and the order date
        public ValidateRemoveOrderResponse ValidateRemoveOrder(string orderDate, Order order) {
            ValidateRemoveOrderResponse response = new ValidateRemoveOrderResponse {
                Orders = _loadedOrders
            };


            // LoadOrders returns null if it can't find a file to read from
            if (response.Orders == null) {
                response.Success = false;
                response.Code = ErrorCode.CouldNotFindFile;
                return response;
            }

            // LoadOrders returns a corrupt order with number -1 if the file is corrupt
            if (response.Orders.Any(x => x.Number == -1)) {
                response.Success = false;
                response.Code = ErrorCode.CorruptFile;
                return response;
            }

            // Checks if LoadOrders order list has an order number that matches the order passed in
            if (!response.Orders.Any(x => x.Number == order.Number)) {
                response.Success = false;
                response.Code = ErrorCode.OrderNumberDoesNotExist;
                return response;
            }

            response.Success = true;

            // Gets the index where the order number matches the order in the list
            int index = response.Orders.FindIndex(x => x.Number == order.Number);
            response.Orders.RemoveAt(index); // Removes the order at the index

            return response;
        }

        // Sends the order to the order repository that will add the order to the list then print to the file
        public void AddOrder(Order order) {
            _orderRepository.SaveAddedOrder(order);
        }

        // Sends the list of orders to the order repository to be written to the file
        public void EditOrder(List<Order> orders) {
            _orderRepository.SaveOrder(orders);
        }

        // Sends the list of orders to the repository so it can be written to the file
        public void RemoveOrder(List<Order> orders) {
            _orderRepository.SaveOrder(orders);
        }

        // Opens the Taxes file and gets the state information from it
        private State GetStateInfo(string stateAbbreviation) {
            State state = new State();
            string[] states; // Will hold every line from the taxes file
            string[] stateInfo; // Will hold the information for each state in states

            // Try to open the file
            try {
                // If it doesn't find a file, set the state abbreviation to a three character error code
                if (!File.Exists(Paths.taxesFilePath)) {
                    state.Abbreviation = COULD_NOT_FIND_FILE;
                }
                else {
                    states = File.ReadAllLines(Paths.taxesFilePath);
                    // If the file returns less than two lines, there aren't any states to pull information from
                    if (states.Length < 2) {
                        state.Abbreviation = FILE_IS_EMPTY;
                    }
                    // Loop through the states array looking for an abbreviation matching the one provided
                    for (int i = 1; i < states.Length; i++) {
                        stateInfo = states[i].Split(',');
                        // If the abbreviation from the file matches the abbreviation provided, pull that stat info
                        if (stateInfo[0] == stateAbbreviation) {
                            state.Abbreviation = stateInfo[0];
                            state.Name = stateInfo[1];
                            state.TaxRate = decimal.Parse(stateInfo[2]);
                            // If the abbreviation is greater than two characters, it's a bad state
                            if(state.Abbreviation.Length > 2) {
                                state = ImportantValues._badState;
                                state.Abbreviation = BAD_STATE;
                            }

                            break;
                        }

                        // If the abbreviation from the file does not match any abbreviation in the array, we don't sell to the state
                        // Validation of the state abbreviation is done prior to getting here, so we don't need to do that again
                        if (i + 1 == states.Length) {
                            state.Abbreviation = DONT_SELL_TO_STATE;
                        }
                    }
                }
            }
            catch {
                // Set the abbreviation to the corrupt file abbreviation so it can be checked later
                _state.Abbreviation = FILE_IS_CORRUPT;
            }

            return state;
        }

        // Opens the Products file and gets the product information from it
        private Product GetProductInfo(string productType) {
            Product product = new Product();
            string[] products; // Will hold every line from the products file
            string[] productInfo; // Will hold the product information for each product in products

            Console.WriteLine();

            // Try to open the products file
            try {
                // If the file doesn't exist, return a corrupt product
                if (!File.Exists(Paths.productsFilePath)) {
                    product = ImportantValues._badProduct;
                    product.CostPerSquareFoot = (decimal)ProductError.CouldNotFindFile;
                    return product;
                }

                products = File.ReadAllLines(Paths.productsFilePath);
                // Checks if the file exists, but doesn't have any products in it
                if (products.Length < 2) {
                    product = ImportantValues._badProduct;
                    product.CostPerSquareFoot = (decimal)ProductError.FileIsEmpty;
                    return product;
                }

                //Loop through the products array and split each line into the product info array
                for (int i = 1; i < products.Length; i++) {
                    productInfo = products[i].Split(',');

                    // Check if the product name matches the name provided
                    if (productInfo[0] == productType) {
                        product.ProductName = productInfo[0];
                        product.CostPerSquareFoot = decimal.Parse(productInfo[1]);
                        product.LaborCostPerSquareFoot = decimal.Parse(productInfo[2]);

                        // Checks if there's a bad product in the file
                        if (product.CostPerSquareFoot < 0  || product.LaborCostPerSquareFoot < 0) {
                            product = ImportantValues._badProduct;
                            product.CostPerSquareFoot = (decimal)ProductError.FoundNegativeValue;
                        }

                        break;
                    }

                    // If we make it to this if, the product provided wasn't in the file
                    if (i + 1 == products.Length) {
                        product = ImportantValues._badProduct;
                        product.CostPerSquareFoot = (decimal)ProductError.ProductNotFound;
                    }
                }
            }
            catch {
                product = ImportantValues._badProduct; // Set the product to be returned the value of a bad product
                product.CostPerSquareFoot = (decimal)ProductError.FileIsCorrupt;
            }

            return product;
        }

        // Performs the calculation on the remaining fields
        private Order PerformCalculation(Order order) {
            order.MaterialCost = order.Area * order.CostPerSquareFoot;
            order.LaborCost = order.Area * order.LaborCostPerSquareFoot;
            order.Tax = (order.MaterialCost + order.LaborCost) * (order.TaxRate / 100);
            order.Total = order.MaterialCost + order.LaborCost + order.Tax;

            return order;
        }

        // Takes the state abbreviation and checks it for an error code
        private ErrorCode ValidateState(string abbreviation) {
            ErrorCode code;

            switch (abbreviation) {
                case COULD_NOT_FIND_FILE:
                    code = ErrorCode.CouldNotFindFile;
                    break;
                case DONT_SELL_TO_STATE:
                    code = ErrorCode.DontSellToState;
                    break;
                case FILE_IS_CORRUPT:
                    code = ErrorCode.CorruptFile;
                    break;
                case FILE_IS_EMPTY:
                    code = ErrorCode.FileIsEmpty;
                    break;
                case BAD_STATE:
                    code = ErrorCode.FoundBadState;
                    break;
                default:
                    code = ErrorCode.NoError;
                    break;
            }

            return code;
        }

        // Takes the product and checks it if there are any error codes that need to be caught
        private ErrorCode ValidateProduct(Product product) {
            ErrorCode code;

            if (product.ProductName == ImportantValues._badProduct.ProductName && product.LaborCostPerSquareFoot == ImportantValues._badProduct.LaborCostPerSquareFoot) {
                switch (_product.CostPerSquareFoot) {
                    case (decimal)ProductError.CouldNotFindFile:
                        code = ErrorCode.CouldNotFindFile;
                        break;
                    case (decimal)ProductError.FileIsCorrupt:
                        code = ErrorCode.CorruptFile;
                        break;
                    case (decimal)ProductError.FileIsEmpty:
                        code = ErrorCode.FileIsEmpty;
                        break;
                    case (decimal)ProductError.ProductNotFound:
                        code = ErrorCode.ProductNotFound;
                        break;
                    default:
                        throw new Exception("Uh oh, you shouldn't be seeing this....");
                }
            }
            else {
                code = ErrorCode.NoError; // If it doesn't find an error, throw this code
            }

            return code;
        }

        // Clones the order passed in
        public Order Clone<Order>(Order order) {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(order, null)) {
                return default(Order);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream) {
                formatter.Serialize(stream, order);
                stream.Seek(0, SeekOrigin.Begin);
                return (Order)formatter.Deserialize(stream);
            }
        }

        // Checks the customer name is valid
        private bool IsValidCustomerName(string name) {
            return name.All(x => char.IsLetterOrDigit(x) || x.Equals(',') || x.Equals('.') || x.Equals(' ')) && !string.IsNullOrWhiteSpace(name);
        }
    }
}