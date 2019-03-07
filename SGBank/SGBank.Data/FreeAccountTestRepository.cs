using SGBank.Models;
using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGBank.Data {
    public class FreeAccountTestRepository : IAccountRepository {
        private static Account _account = new Account {
            Name = "Free Account",
            Balance = 100.00m,
            AccountNumber = "12345",
            Type = AccountType.Free
        };

        public Account LoadAccount(string AccountNumber) {

            if(AccountNumber != _account.AccountNumber) {
                return null;
            }

            return _account;
        }

        public void SaveAccount(Account account) {
            _account = account;
        }
    }
}
