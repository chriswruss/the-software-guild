using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.Tests {
    [TestFixture]
    public class PremiumAccountTests {

        [TestCase("98765", "Premium Account", 100, AccountType.Premium, -100, false)]
        [TestCase("98765", "Premium Account", 100, AccountType.Premium, 250, true)]
        public void PremiumAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult) {
            IDeposit deposit = new NoLimitDepositRule();
            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;
            AccountDepositResponse response = deposit.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("98765", "Premium Account", 1500, AccountType.Basic, -1000, 500, false)] //Testing basic account type
        [TestCase("98765", "Premium Account", 100, AccountType.Free, -100, 0, false)] //Testing free account type
        [TestCase("98765", "Premium Account", 100, AccountType.Premium, 100, 100, false)] //Testing non negative withdrawal
        [TestCase("98765", "Premium Account", 0, AccountType.Premium, -501, -501, false)] //Testing overdraft past limit
        [TestCase("98765", "Premium Account", 150, AccountType.Premium, -50, 100, true)] //Testing good withdrawal
        [TestCase("98765", "Premium Account", 100, AccountType.Premium, -600, -500, true)] //Testing can overdraft to -500
        public void PremiumAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult) {
            IWithdraw withdraw = new PremiumAccountWithdrawRule();
            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;
            AccountWithdrawResponse response = withdraw.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
