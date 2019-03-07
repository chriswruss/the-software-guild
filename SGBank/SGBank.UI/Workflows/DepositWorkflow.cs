using SGBank.BLL;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.UI.Workflows {
    class DepositWorkflow {
        public void Execute() {
            Console.Clear();
            AccountManager accountManager = AccountManagerFactory.Create();

            Console.Write("Enter an account number: ");
            string accountNumber = Console.ReadLine();

            bool validDeposit;
            decimal amount;
            do {
                validDeposit = true;
                Console.Write("Enter a deposit amount: ");
                validDeposit = decimal.TryParse(Console.ReadLine(), out amount);
                if (!validDeposit) {
                    Console.WriteLine("Deposit amount must be a number!");
                }
            } while (validDeposit == false);

            AccountDepositResponse response = accountManager.Deposit(accountNumber, amount);

            if (response.Success) {
                Console.WriteLine("Deposit completed!");
                Console.WriteLine($"Account number: {response.Account.AccountNumber}");
                Console.WriteLine($"Old balance: {response.OldBalance:c}");
                Console.WriteLine($"Amount deposited: {response.Amount:c}");
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
