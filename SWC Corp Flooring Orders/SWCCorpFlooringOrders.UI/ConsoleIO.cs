using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SWCCorpFlooringOrders.Models.Tools;

namespace SWCCorpFlooringOrders.UI {
    public class ConsoleIO {
        public string GetOrderDate() {
            string stringDate;
            bool isValid;

            do {
                isValid = true;
                Console.Write("Enter an order date MMDDYYYY: ");
                stringDate = Console.ReadLine().Trim();
                // Checks that the order date is 8 characters
                if (stringDate.Length != 8) {
                    PrintError(ErrorCode.OrderDateLength);
                    isValid = false;
                }
                else {
                    // Checks thate the order date is all numbers
                    isValid = stringDate.All(x => char.IsNumber(x));
                    if (!isValid) {
                        PrintError(ErrorCode.OrderDateNotAllNumbers);
                    }
                }
            } while (!isValid);

            return stringDate;
        }

        public int GetOrderNumber() {
            int retValue;
            bool isValid;

            do {
                Console.Write("Enter order number: ");
                // Checks the number is indeed a number
                isValid = int.TryParse(Console.ReadLine(), out retValue);
                if (!isValid) {
                    PrintError(ErrorCode.OrderNumberNotInt);
                }
                // Checks the number is greater than 0
                else if (retValue < 1) {
                    PrintError(ErrorCode.OrderNumberNotPositive);
                    isValid = false;
                }
            } while (!isValid);

            return retValue;
        }

        public void PrintError(string message) {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            if (Console.BackgroundColor != ConsoleColor.Black) {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
            PressEnterToContinue();
        }

        public void PrintError(ErrorCode code) {
            switch (code) {
                case ErrorCode.AreaTooSmall:
                    PrintError($"Area is too small, the min area is {ImportantValues.MIN_AREA}");
                    break;
                case ErrorCode.AreaNotDecimal:
                    PrintError("Area has to be a decimal.");
                    break;
                case ErrorCode.AreaNegative:
                    PrintError("Area cannot be negative.");
                    break;
                case ErrorCode.CorruptFile:
                    PrintError($"File is corrupt.");
                    break;
                case ErrorCode.CouldNotFindFile:
                    PrintError("Couldn't find the file.");
                    break;
                case ErrorCode.DidntConfirmOrder:
                    PrintSuccessMessage("Confirmation failed, no changes made.");
                    break;
                case ErrorCode.DontSellToState:
                    PrintError("Sorry, we currently are not accepting orders in that state.");
                    break;
                case ErrorCode.EmptyName:
                    PrintError("Name cannot be empty.");
                    break;
                case ErrorCode.FileIsEmpty:
                    PrintError("File is empty.");
                    break;
                case ErrorCode.FoundBadState:
                    PrintError("Found bad state in file.");
                    break;
                case ErrorCode.InvalidState:
                    PrintError("That's not a state.");
                    break;
                case ErrorCode.NameIsInvalid:
                    PrintError("Names can only contain letters, numbers, commas and periods.");
                    break;
                case ErrorCode.NoError:
                    PrintSuccessMessage("No errors detected.");
                    break;
                case ErrorCode.NotAFutureDate:
                    PrintError("Order date must be in the future.");
                    break;
                case ErrorCode.NotAnInt:
                    PrintError("Input must be an integer.");
                    break;
                case ErrorCode.NotAValidAbbreviation:
                    PrintError("Abbreviation must be two letter that represent a state.");
                    break;
                case ErrorCode.NotAValidDate:
                    PrintError("Date is invalid.");
                    break;
                case ErrorCode.OrderCancelled:
                    PrintError("Order cancelled.");
                    break;
                case ErrorCode.OrderDateLength:
                    PrintError("Order date should contain 8 numbers following the format of MMDDYYYY.");
                    break;
                case ErrorCode.OrderDateNotAllNumbers:
                    PrintError("Order date must contain only numbers.");
                    break;
                case ErrorCode.OrderNumberNotPositive:
                    PrintError("Order number must be positive.");
                    break;
                case ErrorCode.OrderNumberDoesNotExist:
                    PrintError("Order number does not exist.");
                    break;
                case ErrorCode.OrderNumberNotInt:
                    PrintError("Order number must be an integer.");
                    break;
                case ErrorCode.ProductNotFound:
                    PrintError("We don't sell that.");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void PressEnterToContinue() {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;

            if (Console.BackgroundColor != ConsoleColor.Black) {
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine("Press enter to continue...");
            Console.ForegroundColor = oldColor;
            Console.ReadLine();
        }

        public void PrintSuccessMessage(string message) {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            if (Console.BackgroundColor != ConsoleColor.Black) {
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
            PressEnterToContinue();
        }

        public string GetCustomerName() {
            string name;
            bool isInvalid;

            do {
                isInvalid = true;
                Console.Write("Enter customer name: ");
                name = Console.ReadLine().Trim();
                isInvalid = string.IsNullOrWhiteSpace(name);
                if (isInvalid) {
                    PrintError(ErrorCode.EmptyName);
                }
            } while (isInvalid);

            return name;
        }

        public string GetCustomerName(string oldName) {
            string name;
            bool isInValid;

            do {
                isInValid = false;
                Console.Write($"Enter new customer name ({oldName}): ");
                name = Console.ReadLine().Trim();
                if (DidChange(name)) {
                    isInValid = string.IsNullOrWhiteSpace(name);
                    if (isInValid) {
                        PrintError(ErrorCode.EmptyName);
                    }
                }
                else {
                    name = "";
                }
            } while (isInValid);

            return name;
        }

        public string GetCustomerState() {
            string retValue;
            bool isValid;

            do {
                isValid = true;
                Console.Write("Enter the two character abbreviation for the state: ");
                retValue = Console.ReadLine().Trim().ToUpper();
                if (retValue.Length != 2) {
                    isValid = false;
                }
                else {
                    isValid = retValue.All(x => char.IsLetter(x));
                }

                if (!isValid) {
                    PrintError(ErrorCode.NotAValidAbbreviation);
                }
            } while (!isValid);

            return retValue;
        }

        public string GetCustomerState(string oldStateAbbreviation) {
            string retValue;
            bool isValid;

            do {
                isValid = true;
                Console.Write($"Enter the two character abbreviation for the state ({oldStateAbbreviation}): ");
                retValue = Console.ReadLine().Trim().ToUpper();
                if (DidChange(retValue)) {
                    if (retValue.Length != 2) {
                        isValid = false;
                    }
                    else {
                        isValid = retValue.All(x => char.IsLetter(x));
                    }

                    if (!isValid) {
                        PrintError(ErrorCode.NotAValidAbbreviation);
                    }
                }
            } while (!isValid);

            return retValue;
        }

        public Product ProductPickList() {
            int productChoice = -2;
            string[] products;
            string[] productInfo;
            Dictionary<int, Product> productList = new Dictionary<int, Product>();
            string lastSuccessfulProductRead = "";
            Product retProduct = new Product();

            Console.WriteLine();

            try {
                if (!File.Exists(Paths.productsFilePath)) {
                    PrintError("Couldn't find the Products.txt file.");
                    productChoice = -1;
                }
                else {
                    products = File.ReadAllLines(Paths.productsFilePath);
                    if (products.Length < 2) {
                        throw new Exception();
                    }
                    for (int i = 1; i < products.Length; i++) {
                        productInfo = products[i].Split(',');

                        int iterator = 0;
                        Product temp = new Product {
                            ProductName = productInfo[iterator++],
                            CostPerSquareFoot = decimal.Parse(productInfo[iterator++]),
                            LaborCostPerSquareFoot = decimal.Parse(productInfo[iterator++])
                        };

                        productList.Add(i, temp);
                        lastSuccessfulProductRead = temp.ProductName;
                    }

                    string headerFormat = "{0, 3} {1, 20}\t{2, 9}\t{3, 15}";
                    string format = "{0, 3}. {1, 20}\t{2, 9:c}\t{3, 15:c}";
                    Console.WriteLine(headerFormat, "#", "Product Name", "Cost/sqft", "Labor Cost/sqft");
                    Console.WriteLine("****************************************************************");
                    foreach (var product in productList) {
                        Console.WriteLine(format, product.Key, product.Value.ProductName, product.Value.CostPerSquareFoot, product.Value.LaborCostPerSquareFoot);
                        Console.WriteLine("----------------------------------------------------------------");
                    }

                    bool isValid;
                    do {
                        isValid = true;
                        Console.Write("Enter the number corresponding to the product type: ");
                        if (!int.TryParse(Console.ReadLine(), out productChoice)) {
                            PrintError("Please enter an integer.");
                            isValid = false;
                        }
                        else if (productChoice < 1 || productChoice > productList.Keys.Max()) {
                            PrintError("Please enter a valid product type number.");
                            isValid = false;
                        }
                    } while (!isValid);

                    retProduct = productList[productChoice];
                }
            }
            catch {
                if (!productList.Any()) {
                    PrintError("The Products.txt file is empty or the first line is corrupt.");
                }
                else {
                    PrintError($"Uh oh, something went wrong trying to read product after {lastSuccessfulProductRead}.");
                }
            }

            return retProduct;
        }

        public Product ProductPickList(string oldProductType) {
            int productChoice = -2;
            string[] products;
            string[] productInfo;
            Dictionary<int, Product> productList = new Dictionary<int, Product>();
            string lastSuccessfulProductRead = "";
            Product retProduct = new Product();

            Console.WriteLine();

            try {
                if (!File.Exists(Paths.productsFilePath)) {
                    PrintError("Couldn't find the Products.txt file.");
                    productChoice = -1;
                }
                else {
                    products = File.ReadAllLines(Paths.productsFilePath);
                    if (products.Length < 2) {
                        throw new Exception();
                    }
                    for (int i = 1; i < products.Length; i++) {
                        productInfo = products[i].Split(',');

                        int iterator = 0;
                        Product temp = new Product {
                            ProductName = productInfo[iterator++],
                            CostPerSquareFoot = decimal.Parse(productInfo[iterator++]),
                            LaborCostPerSquareFoot = decimal.Parse(productInfo[iterator++])
                        };

                        productList.Add(i, temp);
                        lastSuccessfulProductRead = temp.ProductName;
                    }
                    string headerFormat = "{0, 3} {1, 20}\t{2, 9}\t{3, 15}";
                    string format = "{0, 3}. {1, 20}\t{2, 9:c}\t{3, 15:c}";
                    Console.WriteLine(headerFormat, "#", "Product Name", "Cost/sqft", "Labor Cost/sqft");
                    Console.WriteLine("****************************************************************");
                    foreach (var product in productList) {
                        Console.WriteLine(format, product.Key, product.Value.ProductName, product.Value.CostPerSquareFoot, product.Value.LaborCostPerSquareFoot);
                        Console.WriteLine("----------------------------------------------------------------");
                    }

                    bool isValid;
                    string input;
                    do {
                        isValid = true;
                        Console.Write($"Enter the number corresponding to the product type ({oldProductType}): ");
                        input = Console.ReadLine().Trim();
                        if (DidChange(input)) {
                            if (!int.TryParse(input, out productChoice)) {
                                PrintError(ErrorCode.NotAnInt);
                                isValid = false;
                            }
                            else if (productChoice < 1 || productChoice > productList.Keys.Max()) {
                                PrintError("Please enter a valid product type number.");
                                isValid = false;
                            }
                        }

                    } while (!isValid);

                    if (DidChange(input)) {
                        retProduct = productList[productChoice];
                    }
                    else {
                        retProduct.ProductName = null;
                    }
                }
            }
            catch {
                if (!productList.Any()) {
                    PrintError("The Products.txt file is empty or the first line is corrupt.");
                }
                else {
                    PrintError($"Uh oh, something went wrong trying to read product after {lastSuccessfulProductRead}.");
                }
            }

            return retProduct;
        }

        public decimal GetArea() {
            decimal area;
            bool isValid;

            do {
                isValid = true;
                Console.Write("Enter the area needed: ");
                if (!decimal.TryParse(Console.ReadLine(), out area)) {
                    PrintError(ErrorCode.AreaNotDecimal);
                    isValid = false;
                }
                else if (area < 0) {
                    PrintError(ErrorCode.AreaNegative);
                    isValid = false;
                }
            } while (!isValid);

            return area;
        }

        public decimal GetArea(decimal oldArea) {
            decimal retArea;
            bool isValid;

            do {
                isValid = true;
                Console.Write($"Enter the area needed ({oldArea}): ");
                string input = Console.ReadLine().Trim();
                if (DidChange(input)) {
                    if (!decimal.TryParse(input, out retArea)) {
                        PrintError(ErrorCode.AreaNotDecimal);
                        isValid = false;
                    }

                    else if (retArea < 0) {
                        isValid = false;
                        PrintError(ErrorCode.AreaNegative);
                    }
                }
                else {
                    retArea = -1;
                }

            } while (!isValid);

            return retArea;
        }

        public bool DidChange(string input) {
            return string.IsNullOrWhiteSpace(input) ? false : true;
        }

        public YesNo GetConfirmation(string process) {
            YesNo retVal;
            string response;

            do {
                Console.Write($"Are you sure you want to {process} this order? -> (Y/N) : ");
                response = Console.ReadLine().ToUpper();
                switch (response) {
                    case "Y":
                        retVal = YesNo.Yes;
                        break;
                    case "N":
                        retVal = YesNo.No;
                        break;
                    default:
                        retVal = YesNo.Invalid;
                        break;
                }
            } while (retVal == YesNo.Invalid);

            return retVal;
        }

        public void PrintOrder(string orderDate, Order order) {
            Console.Clear();

            Console.WriteLine($"Order Date: {orderDate}");
            Console.WriteLine($"Name: {order.CustomerName}");
            Console.WriteLine($"State: {order.State}");
            Console.WriteLine($"Tax Rate: {order.TaxRate}%");
            Console.WriteLine($"Product Type: {order.ProductType}");
            Console.WriteLine($"Area: {order.Area:n}");
            Console.WriteLine($"Total: {order.Total:c}");
        }

        public void PrintChanges(Order oldOrder, Order editedOrder) {
            Console.Clear();

            Console.WriteLine($"{oldOrder.CustomerName} -> {editedOrder.CustomerName}");
            Console.WriteLine($"{oldOrder.State} -> {editedOrder.State}");
            Console.WriteLine($"{oldOrder.ProductType} -> {editedOrder.ProductType}");
            Console.WriteLine($"{oldOrder.Area} -> {editedOrder.Area}\n");
        }
    }
}