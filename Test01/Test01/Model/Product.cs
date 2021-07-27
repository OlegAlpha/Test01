using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test01.Test01.Model
{
    public class Product
    {
        public string Region { get; set; }
        public string Country { get; set; }
        public string ItemTypes { get; set; }
        public string SalesChannel { get; set; }
        public string OrderPriority { get; set; }
        public DateTime OrderDate { get; set; }
        public uint OrderID { get; set; }
        public DateTime ShipDate { get; set; }
        public uint UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }



    }
}
