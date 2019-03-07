﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SGBank.Models {
    public class Account {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }
    }
}
