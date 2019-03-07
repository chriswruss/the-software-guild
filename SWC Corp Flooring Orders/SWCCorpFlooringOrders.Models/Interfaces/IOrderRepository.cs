using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Interfaces {
    public interface IOrderRepository {
        List<Order> LoadOrders(string orderDate);
        void SaveAddedOrder(Order order);
        void SaveOrder(List<Order> orders);
    }
}
