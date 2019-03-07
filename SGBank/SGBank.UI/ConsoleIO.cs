using SGBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.UI {
    class ConsoleIO {
        public static void DisplayAccountDetails(Account account) {
            Console.WriteLine($"Account number: {account.AccountNumber}");
            Console.WriteLine($"Name: {account.Name}");
            Console.WriteLine($"Balance: {account.Balance:c}");
        } 
    }
}
