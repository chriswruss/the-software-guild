using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Tools {
    public class ImportantValues {
        public const decimal MIN_AREA = 100;
        public static string[] STATES = { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
                                          "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
                                          "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
                                          "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
                                          "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
        }; // Array with all state abbreviations
        public static Product _badProduct = new Product {
            ProductName = "BAD PRODUCT",
            LaborCostPerSquareFoot = -10231451451465146142323234.3242651656114341684135234m
        }; // A product created with bad values
        public static State _badState = new State {
            Name = "BAD STATE",
            TaxRate = 134134153424521452482256245.35346452412424121484824262452m
        }; // A state created with bad values
        public static Order nullOrder = new Order {
            Area = 0,
            CostPerSquareFoot = 0,
            CustomerName = null,
            LaborCost = 0,
            LaborCostPerSquareFoot = 0,
            MaterialCost = 0,
            Number = 0,
            ProductType = null,
            State = null,
            Tax = 0,
            TaxRate = 0,
            Total = 0
        }; // A null order
        public static Product nullProduct = new Product {
            ProductName = null,
            CostPerSquareFoot = 0,
            LaborCostPerSquareFoot = 0
        }; // A null product
    }
}
