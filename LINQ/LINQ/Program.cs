using LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQ
{
    class Program
    {
        static void Main()
        {
            //PrintAllProducts();
            //PrintAllCustomers();
            //Exercise1(); //done
            //Console.ReadLine();
            //Exercise2(); //done
            //Console.ReadLine();
            //Exercise3(); //done
            //Console.ReadLine();
            //Exercise4(); //done
            //Console.ReadLine();
            //Exercise5(); //done
            //Console.ReadLine();
            //Exercise6(); //done
            //Console.ReadLine();
            //Exercise7(); //done
            //Console.ReadLine();
            //Exercise8(); //done
            //Console.ReadLine();
            //Exercise9(); //done
            //Console.ReadLine();
            //Exercise10(); //done
            //Console.ReadLine();
            //Exercise11(); //done
            //Console.ReadLine();
            //Exercise12(); //done
            //Console.ReadLine();
            //Exercise13(); //done
            //Console.ReadLine();
            //Exercise14(); //done
            //Console.ReadLine();
            //Exercise15(); //done
            //Console.ReadLine();
            //Exercise16(); //done
            //Console.ReadLine();
            //Exercise17(); //done
            //Console.ReadLine();
            //Exercise18(); //done
            //Console.ReadLine();
            //Exercise19(); //done
            //Console.ReadLine();
            //Exercise20(); //done
            //Console.ReadLine();
            Exercise21(); //
            //Console.ReadLine();
            //Exercise22(); //done
            //Console.ReadLine();
            //Exercise23(); //done
            //Console.ReadLine();
            //Exercise24(); //done
            //Console.ReadLine();
            //Exercise25(); //done
            //Console.ReadLine();
            //Exercise26(); //done
            //Console.ReadLine();
            //Exercise27(); //done
            //Console.ReadLine();
            //Exercise28(); //done
            //Console.ReadLine();
            //Exercise29(); //done
            //Console.ReadLine();
            //Exercise30(); //done
            //Console.ReadLine();
            //Exercise31(); //done

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        #region "Sample Code"
        /// <summary>
        /// Sample, load and print all the product objects
        /// </summary>
        static void PrintAllProducts()
        {
            List<Product> products = DataLoader.LoadProducts();
            PrintProductInformation(products);
        }

        /// <summary>
        /// This will print a nicely formatted list of products
        /// </summary>
        /// <param name="products">The collection of products to print</param>
        static void PrintProductInformation(IEnumerable<Product> products)
        {
            string line = "{0,-5} {1,-35} {2,-15} {3,7:c} {4,6}";
            Console.WriteLine(line, "ID", "Product Name", "Category", "Unit", "Stock");
            Console.WriteLine("==============================================================================");

            foreach (var product in products)
            {
                Console.WriteLine(line, product.ProductID, product.ProductName, product.Category,
                    product.UnitPrice, product.UnitsInStock);
            }

        }

        /// <summary>
        /// Sample, load and print all the customer objects and their orders
        /// </summary>
        static void PrintAllCustomers()
        {
            var customers = DataLoader.LoadCustomers();
            PrintCustomerInformation(customers);
        }

        /// <summary>
        /// This will print a nicely formated list of customers
        /// </summary>
        /// <param name="customers">The collection of customer objects to print</param>
        static void PrintCustomerInformation(IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine("==============================================================================");
                Console.WriteLine(customer.CompanyName);
                Console.WriteLine(customer.Address);
                Console.WriteLine("{0}, {1} {2} {3}", customer.City, customer.Region, customer.PostalCode, customer.Country);
                Console.WriteLine("p:{0} f:{1}", customer.Phone, customer.Fax);
                Console.WriteLine();
                Console.WriteLine("\tOrders");
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine("\t{0} {1:MM-dd-yyyy} {2,10:c}", order.OrderID, order.OrderDate, order.Total);
                }
                Console.WriteLine("==============================================================================");
                Console.WriteLine();
            }
        }
        #endregion

        /// <summary>
        /// Print all products that are out of stock.
        /// </summary>
        static void Exercise1()
        {
            var results = DataLoader.LoadProducts().Where(x => x.UnitsInStock == 0);

            PrintProductInformation(results);
        }

        /// <summary>
        /// Print all products that are in stock and cost more than 3.00 per unit.
        /// </summary>
        static void Exercise2()
        {
            var results = DataLoader.LoadProducts().Where(x => x.UnitsInStock != 0 && x.UnitPrice > 3m);

            PrintProductInformation(results);
        }

        /// <summary>
        /// Print all customer and their order information for the Washington (WA) region.
        /// </summary>
        static void Exercise3()
        {
            var results = DataLoader.LoadCustomers().Where(x => x.Region == "WA");

            PrintCustomerInformation(results);
        }

        /// <summary>
        /// Create and print an anonymous type with just the ProductName
        /// </summary>
        static void Exercise4()
        {
            var anonProductName = from product in DataLoader.LoadProducts()
                                  select new {
                                      ProductsName = product.ProductName
                                  };

            string line = "{0,-35}";
            Console.WriteLine(line,"Product Name");
            Console.WriteLine("==============================================================================");

            foreach (var a in anonProductName) {
                Console.WriteLine(a.ProductsName);
            }
        }

        /// <summary>
        /// Create and print an anonymous type of all product information but increase the unit price by 25%
        /// </summary>
        static void Exercise5()
        {
            var anonAllInfoUpPrice25 = from product in DataLoader.LoadProducts()
                                       select new {
                                           product.ProductID,
                                           product.ProductName,
                                           product.Category,
                                           ProductsUnitPrice = product.UnitPrice * 1.25m,
                                           product.UnitsInStock
                                       };

            string line = "{0,-5} {1,-35} {2,-15} {3,6:c} {4,6}";
            Console.WriteLine(line, "ID", "Product Name", "Category", "Unit", "Stock");
            Console.WriteLine("==============================================================================");

            foreach (var a in anonAllInfoUpPrice25) {
                Console.WriteLine(line, a.ProductID, a.ProductName, a.Category, a.ProductsUnitPrice, a.UnitsInStock);
            }
        }

        /// <summary>
        /// Create and print an anonymous type of only ProductName and Category with all the letters in upper case
        /// </summary>
        static void Exercise6()
        {
            var anonProductNameCategoryUpper = from product in DataLoader.LoadProducts()
                                               select new {
                                                   ProductsName = product.ProductName.ToUpper(),
                                                   ProductsCategory = product.Category.ToUpper()
                                               };

            string line = "{0,-35} {1,-15}";
            Console.WriteLine(line, "Product Name", "Category");
            Console.WriteLine("==============================================================================");

            foreach (var a in anonProductNameCategoryUpper) {
                Console.WriteLine(line, a.ProductsName, a.ProductsCategory);
            }

        }

        /// <summary>
        /// Create and print an anonymous type of all Product information with an extra bool property ReOrder which should 
        /// be set to true if the Units in Stock is less than 3
        /// 
        /// Hint: use a ternary expression
        /// </summary>
        static void Exercise7()
        {
            var anonProductReorder = from product in DataLoader.LoadProducts()
                                     select new {
                                         product.ProductID,
                                         product.ProductName,
                                         product.Category,
                                         product.UnitPrice,
                                         product.UnitsInStock,
                                         ReOrder = product.UnitsInStock < 3 ? true : false
                                     };

            string line = "{0,-5} {1,-35} {2,-15} {3,6:c} {4,6} {5,5}";
            Console.WriteLine(line, "ID", "Product Name", "Category", "Unit", "Stock", "ReOrder");
            Console.WriteLine("==============================================================================");

            foreach (var a in anonProductReorder) {
                Console.WriteLine(line, a.ProductID, a.ProductName, a.Category, a.UnitPrice, a.UnitsInStock, a.ReOrder);
            }
        }

        /// <summary>
        /// Create and print an anonymous type of all Product information with an extra decimal called 
        /// StockValue which should be the product of unit price and units in stock
        /// </summary>
        static void Exercise8()
        {
            var anonProductsWithStockValue = from product in DataLoader.LoadProducts()
                                             select new {
                                                 product.ProductID,
                                                 product.ProductName,
                                                 product.Category,
                                                 product.UnitPrice,
                                                 product.UnitsInStock,
                                                 StockValue = product.UnitPrice * product.UnitsInStock
                                             };

            string line = "{0,-5} {1,-35} {2,-15} {3,6:c} {4,6} {5,10:c}";
            Console.WriteLine(line, "ID", "Product Name", "Category", "Unit", "Stock", "Stock Value");
            Console.WriteLine("====================================================================================");

            foreach (var a in anonProductsWithStockValue) {
                Console.WriteLine(line, a.ProductID, a.ProductName, a.Category, a.UnitPrice, a.UnitsInStock, a.StockValue);
            }
        }

        /// <summary>
        /// Print only the even numbers in NumbersA
        /// </summary>
        static void Exercise9()
        {
            var nums = DataLoader.NumbersA.Where(x => x % 2 == 0);

            foreach (var n in nums) {
                Console.WriteLine(n);
            }
        }

        /// <summary>
        /// Print only customers that have an order whos total is less than $500
        /// </summary>
        static void Exercise10()
        {
            var customers = DataLoader.LoadCustomers();

            List<Customer> hasOrderUnder500 = new List<Customer>();

            foreach (var c in customers) {
                if (!c.Orders.All(x => x.Total >= 500)) {
                    hasOrderUnder500.Add(c);
                }
            }

            PrintCustomerInformation(hasOrderUnder500);
            
        }

        /// <summary>
        /// Print only the first 3 odd numbers from NumbersC
        /// </summary>
        static void Exercise11()
        {
            var nums = DataLoader.NumbersC.Where(x => x % 2 != 0).Take(3);

            foreach (var n in nums) {
                Console.WriteLine(n);
            }
        }

        /// <summary>
        /// Print the numbers from NumbersB except the first 3
        /// </summary>
        static void Exercise12()
        {
            int[] numbers = DataLoader.NumbersB.Skip(3).ToArray();

            for (int i = 0; i < numbers.Length; i++) {
                Console.WriteLine(numbers[i]);
            }
        }

        /// <summary>
        /// Print the Company Name and most recent order for each customer in Washington
        /// </summary>
        static void Exercise13()
        {
            var customers = from customer in DataLoader.LoadCustomers()
                            where customer.Region == "WA"
                            select new {
                                companyName = customer.CompanyName,
                                recentOrder = customer.Orders.Max(x => x.OrderDate)
                            };

            foreach (var c in customers) {
                Console.WriteLine($"{c.companyName} {c.recentOrder}");
            }
        }

        /// <summary>
        /// Print all the numbers in NumbersC until a number is >= 6
        /// </summary>
        static void Exercise14()
        {
            var numbers = DataLoader.NumbersC.TakeWhile(x => x < 6);

            foreach (int num in numbers) {
                Console.WriteLine(num);
            }
        }

        /// <summary>
        /// Print all the numbers in NumbersC that come after the first number divisible by 3
        /// </summary>
        static void Exercise15()
        {
            var nums = DataLoader.NumbersC.SkipWhile(x => x % 3 != 0).Skip(1);

            foreach (var num in nums) {
                Console.WriteLine(num);
            }
        }

        /// <summary>
        /// Print the products alphabetically by name
        /// </summary>
        static void Exercise16()
        {
            var products = DataLoader.LoadProducts().OrderBy(x => x.ProductName);

            PrintProductInformation(products);
        }

        /// <summary>
        /// Print the products in descending order by units in stock
        /// </summary>
        static void Exercise17()
        {
            var products = DataLoader.LoadProducts().OrderByDescending(x => x.UnitsInStock);

            PrintProductInformation(products);
        }

        /// <summary>
        /// Print the list of products ordered first by category, then by unit price, from highest to lowest.
        /// </summary>
        static void Exercise18()
        {
            var products = DataLoader.LoadProducts().OrderBy(x => x.Category).ThenByDescending(x => x.UnitPrice);

            PrintProductInformation(products);
        }

        /// <summary>
        /// Print NumbersB in reverse order
        /// </summary>
        static void Exercise19()
        {
            var nums = DataLoader.NumbersB.Reverse();

            foreach (var num in nums) {
                Console.WriteLine(num);
            }
        }

        /// <summary>
        /// Group products by category, then print each category name and its products
        /// ex:
        /// 
        /// Beverages
        /// Tea
        /// Coffee
        /// 
        /// Sandwiches
        /// Turkey
        /// Ham
        /// </summary>
        static void Exercise20()
        {
            var productsToPrint = DataLoader.LoadProducts().OrderBy(x => x.Category).GroupBy(x => x.Category);

            foreach (var group in productsToPrint) {
                Console.WriteLine(group.Key);
                foreach (var product in group) {
                    Console.WriteLine(product.ProductName);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print all Customers with their orders by Year then Month
        /// ex:
        /// 
        /// Joe's Diner
        /// 2015
        ///     1 -  $500.00
        ///     3 -  $750.00
        /// 2016
        ///     2 - $1000.00
        /// </summary>
        static void Exercise21()
        {
            //////*****CORRECT SOLUTION*****
            var items = from customer in DataLoader.LoadCustomers()
                        select new {
                            Name = customer.CompanyName,
                            OrdersByYear = from order in customer.Orders
                                           group order by order.OrderDate.Year into oby
                                           select new {
                                               Year = oby.Key,
                                               OrdersByMonth = from obym in oby
                                                               group obym by obym.OrderDate.Month into obm
                                                               select new {
                                                                   Month = obm.Key,
                                                                   Total = obm.Sum(o => o.Total)
                                                               }
                                           }
                        };

            foreach (var item in items) {
                Console.WriteLine($"{item.Name}");
                foreach (var yo in item.OrdersByYear) {
                    Console.WriteLine($"\t{yo.Year}");
                    foreach (var order in yo.OrdersByMonth) {
                        Console.WriteLine($"\t\t{string.Format("{0} - {1:c}", order.Month, order.Total)}");
                    }
                }
            }

            //var customers = DataLoader.LoadCustomers();

            //foreach (var customer in customers) {
            //    var orders = from order in customer.Orders
            //                 group order by order.OrderDate.Year into yg
            //                 select new {
            //                     year = yg.Key,
            //                     monthGroups = from y in yg
            //                                   group y by y.OrderDate.Month into mg
            //                                   select new {
            //                                       month = mg.Key,
            //                                       orders = mg
            //                                   }
            //                 };
            //    foreach(var o in orders) {
            //        Console.WriteLine($"{o.year} {o.mo") {
            //            foreach(var m in ) {

            //            }
            //        }
            //    }

            //}

            //var customers = from customer in DataLoader.LoadCustomers()
            //                from order in customer.Orders
            //                group customer by order.OrderDate.Year into yg
            //                select new {
            //                    year = yg.Key,
            //                    mg = from y in yg
            //                         from order in .Orders
            //                         group y by order.OrderDate.Month into mg
            //                         select new {
            //                             month = mg.Key,
            //                             orders = mg
            //                         }
            //                };

            //List<Order> orders = 

            //foreach (var c in customers) {
            //    Console.WriteLine(c.year);
            //    foreach (var month in c) {
            //        Console.WriteLine()
            //    }
            //}



        }

        /// <summary>
        /// Print the unique list of product categories
        /// </summary>
        static void Exercise22()
        {
            var toPrint = DataLoader.LoadProducts().GroupBy(x => x.Category);

            foreach (var group in toPrint) {
                Console.WriteLine(group.Key);
            }
        }

        /// <summary>
        /// Write code to check to see if Product 789 exists
        /// </summary>
        static void Exercise23()
        {
            var products = DataLoader.LoadProducts().Where(x => x.ProductID == 789 || x.ProductName == "789");

            if (!products.Any()) {
                Console.WriteLine("No results found...");
            }
            else {
                foreach (var p in products) {
                    Console.WriteLine($"{p.ProductID} {p.ProductName}");
                }
            }
        }

        /// <summary>
        /// Print a list of categories that have at least one product out of stock
        /// </summary>
        static void Exercise24()
        {
            var products = DataLoader.LoadProducts().Where(x => x.UnitsInStock == 0).GroupBy(x => x.Category);

            foreach(var group in products) {
                Console.WriteLine(group.Key);
            }
        }

        /// <summary>
        /// Print a list of categories that have no products out of stock
        /// </summary>
        static void Exercise25()
        {
            var products = DataLoader.LoadProducts().GroupBy(x => x.Category);

            foreach (var p in products) {
                if (p.All(x => x.UnitsInStock > 0)) {
                    Console.WriteLine(p.Key);
                }
            }
        }

        /// <summary>
        /// Count the number of odd numbers in NumbersA
        /// </summary>
        static void Exercise26()
        {
            var nums = DataLoader.NumbersA.Where(x => x % 2 != 0);

            Console.WriteLine(nums.Count());
        }

        /// <summary>
        /// Create and print an anonymous type containing CustomerId and the count of their orders
        /// </summary>
        static void Exercise27()
        {
            var customers = from customer in DataLoader.LoadCustomers()
                            select new {
                                customer.CompanyName,
                                orderCount = customer.Orders.Count()
                            };

            foreach (var c in customers) {
                Console.WriteLine($"{c.CompanyName} {c.orderCount}");
            }
        }

        /// <summary>
        /// Print a distinct list of product categories and the count of the products they contain
        /// </summary>
        static void Exercise28()
        {
            var toPrint = from product in DataLoader.LoadProducts()
                          group product by product.Category into groups
                          select new {
                              CategoryName = groups.Key,
                              CategoryCount = groups.Count()
                          };

            foreach (var t in toPrint) {
                Console.WriteLine($"{t.CategoryName} {t.CategoryCount}");
            }
        }

        /// <summary>
        /// Print a distinct list of product categories and the total units in stock
        /// </summary>
        static void Exercise29()
        {
            var toPrint = from product in DataLoader.LoadProducts()
                          group product by product.Category into groups
                          select new {
                              CategoryName = groups.Key,
                              CategoryCount = groups.Sum(x => x.UnitsInStock)
                          };

            foreach (var t in toPrint) {
                Console.WriteLine($"{t.CategoryName} {t.CategoryCount}");
            }
        }

        /// <summary>
        /// Print a distinct list of product categories and the lowest priced product in that category
        /// </summary>
        static void Exercise30()
        {
            var toPrint = from product in DataLoader.LoadProducts()
                          group product by product.Category into groups
                          select new {
                              CategoryName = groups.Key,
                              LowestPrice = groups.Min(x => x.UnitPrice)
                          };

            foreach (var t in toPrint) {
                Console.WriteLine($"{t.CategoryName} {t.LowestPrice:c}");
            }
        }

        /// <summary>
        /// Print the top 3 categories by the average unit price of their products
        /// </summary>
        static void Exercise31()
        {
            //var products = DataLoader.LoadProducts().GroupBy(x => x.Category, (key, groups) => new { CategoryName = key, AveragePrice = groups.Average(x => x.UnitPrice) });

            var products = from product in DataLoader.LoadProducts()
                                group product by product.Category into groups
                                select new {
                                    CategoryName = groups.Key,
                                    AveragePrice = groups.Average(x => x.UnitPrice)
                                };

            var test = products.OrderByDescending(x => x.AveragePrice).Take(3);
            
            foreach (var t in test) {
                Console.WriteLine($"{t.CategoryName} {t.AveragePrice:c}");
            }
        }
    }
}