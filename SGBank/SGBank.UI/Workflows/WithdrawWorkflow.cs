using SGBank.BLL;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.UI.Workflows {
    public class WithdrawWorkflow {
        public void Execute() {
            Console.Clear();
            AccountManager accountManager = AccountManagerFactory.Create();

            Console.Write("Enter an account number: ");
            string accountNumber = Console.ReadLine();

            bool validWithdraw;
            decimal withdrawAmount;
            do {
                validWithdraw = true;
                Console.Write("Enter a deposit amount: ");
                validWithdraw = decimal.TryParse(Console.ReadLine(), out withdrawAmount);
                if (!validWithdraw) {
                    Console.WriteLine("Withdrawal amount must be a number!");
                }
            } while (validWithdraw == false);

            AccountWithdrawResponse response = accountManager.Withdraw(accountNumber, withdrawAmount);

            if (response.Success) {
                Console.WriteLine("Withdraw completed!");
                Console.WriteLine($"Account number: {response.Account.AccountNumber}");
                Console.WriteLine($"Old balance: {response.OldBalance:c}");
                Console.WriteLine($"Amount withdrawn: {response.Amount:c}");
                Console.WriteLine($"New balance: {response.Account.Balance:c}");
            }
            else {
                Console.Write("An error occured: ");
                Console.WriteLine(response.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
