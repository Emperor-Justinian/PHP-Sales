using System;
using System.Collections.Generic;
using PHP.Sales.Core.Models.System;

namespace PHP.Sales.Web.ViewModels
{
    public class ChartViewModel
    {
        public ChartViewModel()
        {
            Data = new Dictionary<DateTime, StockSaleSet>();
        }

        public class StockSaleSet
        {
            public decimal Stock { get; set; }
            public decimal Sale { get; set; }
        }

        public Report Report { get; set; }
        public Dictionary<DateTime, StockSaleSet> Data { get; set; }

        /*
         * DateTime     => Stock, Sale
         * 2010-10-01   => 10,    1
         * 2010-10-02   => 13,    2
         * 2010-10-03   => 12,    1
         * 2010-10-04   => 20,    7
         * 2010-10-05   => 17,    0
         * 2010-10-06   => 15,    10
         */
    }
}