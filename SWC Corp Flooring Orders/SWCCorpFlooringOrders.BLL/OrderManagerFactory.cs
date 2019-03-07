using SWCCorpFlooringOrders.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.BLL {
    public class OrderManagerFactory {
        public static OrderManager Create() {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();
             
            // Takes the mode read from the app.config file and chooses the appropriate Order Manager to create
            switch (mode) {
                case "Test":
                    return new OrderManager(new OrderTestRepository());
                case "Prod":
                    return new OrderManager(new OrderProdRepository());
                default:
                    throw new Exception("Mode is invalid in app config file");
            }
        }
    }
}
