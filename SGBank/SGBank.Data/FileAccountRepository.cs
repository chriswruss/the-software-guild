using SGBank.Models;
using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SGBank.Data {
    public class FileAccountRepository : IAccountRepository {
        static string path = @"C:\Users\chris\OneDrive\Repos\TSG.NET\SGBank\Accounts.txt";
        static int index = 1;
        string[] rows;
        Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        Account returnAccount = new Account();

        public Account LoadAccount(string AccountNumber) {
            AccountType accountType = new AccountType();
            string[] currentAccount = new string[0];
            try {
                path = @"C:\Users\chris\OneDrive\Repos\TSG.NET\SGBank\Accounts.txt";
                rows = File.ReadAllLines(path);

                for (int i = 1; i < rows.Length; i++) {
                    currentAccount = rows[i].Split(',');
                    switch (currentAccount[(int)AccountLabels.Type]) {
                        case "F":
                            accountType = AccountType.Free;
                            break;
                        case "B":
                            accountType = AccountType.Basic;
                            break;
                        case "P":
                            accountType = AccountType.Premium;
                            break;
                    }

                    Account temp = new Account {
                        AccountNumber = currentAccount[(int)AccountLabels.AccountNumber],
                        Name = currentAccount[(int)AccountLabels.Name],
                        Balance = decimal.Parse(currentAccount[(int)AccountLabels.Balance]),
                        Type = accountType
                    };

                    accounts.Add(i, temp);

                    if (currentAccount[(int)AccountLabels.AccountNumber] == AccountNumber) {
                        returnAccount = temp;
                        index = i;
                    }
                }
            }
            catch (Exception) {

                throw new Exception();
            }
            finally {
            }

            return returnAccount;
        }

        public void SaveAccount(Account account) {

            if (File.Exists(path)) {
                File.Delete(path);
            }

            using (StreamWriter writer = File.AppendText(path)) {
                writer.WriteLine("AccountNumber,Name,Balance,Type");
                foreach(var acc in accounts) {
                    char accType = acc.Value.Type.ToString()[0];
                    writer.WriteLine($"{acc.Value.AccountNumber},{acc.Value.Name},{acc.Value.Balance},{accType}");
                }
            }
        }
    }
}
