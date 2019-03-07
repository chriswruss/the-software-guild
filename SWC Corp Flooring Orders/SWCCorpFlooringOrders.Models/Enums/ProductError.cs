using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Enums {
    // Used to mark invalid products and give a specific value to be used during error messaging.
    public enum ProductError {
        CouldNotFindFile = -101,
        FileIsCorrupt = -102,
        FileIsEmpty = -103,
        FoundNegativeValue = -104,
        ProductNotFound = -105
    }
}
