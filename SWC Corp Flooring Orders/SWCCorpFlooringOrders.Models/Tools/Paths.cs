using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Tools {
    public class Paths {
        #region Relative File Paths 
        public static string productsFilePath = @"..\..\..\Products.txt";
        public static string taxesFilePath = @"..\..\..\Taxes.txt";
        public static string ordersFolderFilePath = @"..\..\..\Orders\Orders_";
        #endregion
        #region Absolute File Paths
        //public static string productsFilePath = @"C:\Users\chris\OneDrive\Repos\TSG.NET\SWC Corp Flooring Orders\Products.txt";
        //public static string taxesFilePath = @"C:\Users\chris\OneDrive\Repos\TSG.NET\SWC Corp Flooring Orders\Taxes.txt";
        //public static string ordersFolderFilePath = @"C:\Users\chris\OneDrive\Repos\TSG.NET\SWC Corp Flooring Orders\Orders\Orders_";
        #endregion
    }
}
