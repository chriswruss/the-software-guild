using SWCCorpFlooringOrders.Models;
using SWCCorpFlooringOrders.Models.Tools;
using SWCCorpFlooringOrders.UI.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.UI {
    public class Menu {
        static bool isDone = false;

        public static void Start() {
            ConsoleIO prompt = new ConsoleIO();
            while (!isDone) {
                Console.Clear();
                Console.WriteLine("******************************");
                Console.WriteLine("* " + PrintFormatting.companyName);
                Console.WriteLine("*");
                Console.WriteLine("* 1. Display Orders");
                Console.WriteLine("* 2. Add Order");
                Console.WriteLine("* 3. Edit Order");
                Console.WriteLine("* 4. Remove Order");
                Console.WriteLine("* 5. Quit");
                Console.WriteLine("*");
                Console.WriteLine("******************************");

                Console.Write("Enter your choice: ");
                string userInput = Console.ReadLine().ToUpper();

                switch (userInput) {
                    case "1":
                        DisplayOrdersWorkflow display = new DisplayOrdersWorkflow();
                        display.Execute();
                        break;
                    case "2":
                        AddOrderWorkflow add = new AddOrderWorkflow();
                        add.Execute();
                        break;
                    case "3":
                        EditOrderWorkflow edit = new EditOrderWorkflow();
                        edit.Execute();
                        break;
                    case "4":
                        RemoveOrderWorkflow remove = new RemoveOrderWorkflow();
                        remove.Execute();
                        break;
                    case "5":
                        isDone = true;
                        break;
                    default:
                        prompt.PrintError("Please enter a number from 1 to 5.");
                        break;
                }
            }
        }
    }
}
