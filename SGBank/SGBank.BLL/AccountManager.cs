using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SGBank.BLL {
    public class AccountManager {
        public int fileIndex = 0;
        public string[] rows;
        public string[] columns;

        private IAccountRepository _accountRepository;

        public AccountManager(IAccountRepository accountRepository) {
            _accountRepository = accountRepository;
        }

        public AccountLookupResponse LookupAccount(string accountNumber) {
            AccountLookupResponse response = new AccountLookupResponse {
                Account = _accountRepository.LoadAccount(accountNumber)
            };

            if (response.Account.AccountNumber == null) {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
            }
            else {
                response.Success = true;
            }

            return response;
        }

        public AccountDepositResponse Deposit(string accountNumber, decimal amount) {
            AccountDepositResponse response = new AccountDepositResponse {
                Account = _accountRepository.LoadAccount(accountNumber)
            };

            if (response.Account.AccountNumber == null) {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
                return response;
            }
            else {
                response.Success = true;
            }

            IDeposit depositRule = DepositRulesFactory.Create(response.Account.Type);
            response = depositRule.Deposit(response.Account, amount);

            if(response.Success) {
                _accountRepository.SaveAccount(response.Account);
            }
            return response;
        }

        public AccountWithdrawResponse Withdraw(string accountNumber, decimal amount) {
            AccountWithdrawResponse response = new AccountWithdrawResponse {
                Account = _accountRepository.LoadAccount(accountNumber)
            };

            if (response.Account.AccountNumber == null) {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
                return response;
            }

            IWithdraw withdraw = WithdrawRulesFactory.Create(response.Account.Type);
            response = withdraw.Withdraw(response.Account, amount);

            if (response.Success) {
                _accountRepository.SaveAccount(response.Account);
            }

            return response;
        }
    }
}
