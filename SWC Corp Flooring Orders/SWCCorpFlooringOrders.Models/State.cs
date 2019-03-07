using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models {
    public class State {
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public decimal TaxRate { get; set; }
    }
}
