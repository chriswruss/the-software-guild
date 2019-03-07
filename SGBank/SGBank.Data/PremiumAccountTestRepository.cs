using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Data {
    public class PremiumAccountTestRepository : IAccountRepository {
        private static Account _account = new Account {
            Name = "Premium Account",
            AccountNumber = "98765",
            Balance = 100m,
            Type = AccountType.Premium
        };

        public Account LoadAccount(string AccountNumber) {
            AccountLookupResponse response = new AccountLookupResponse();

            if (AccountNumber != _account.AccountNumber) {
                return null;
            }

            return _account;
        }

        public void SaveAccount(Account account) {
            _account = account;
        }
    }
}
