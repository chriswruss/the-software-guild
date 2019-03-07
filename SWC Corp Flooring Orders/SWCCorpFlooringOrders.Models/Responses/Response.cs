using SWCCorpFlooringOrders.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Responses {
    public class Response {
        public bool Success { get; set; }
        public ErrorCode Code { get; set; }
        public List<Order> Orders { get; set; }
    }
}
