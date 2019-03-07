using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Responses {
    public class ValidateAddOrderResponse : Response{
        public Order Order { get; set; }
    }
}
