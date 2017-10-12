using System;
using System.Collections.Generic;
using PHP.Sales.Core.Models.System;

namespace PHP.Sales.Web.ViewModels
{
    /// <summary>
    /// View Model for collecting Sales and Stock information for a report view
    /// </summary>
    public class ChartViewModel
    {
        /// <summary>
        /// Initalise the View Model with an empty dictionary
        /// </summary>
        public ChartViewModel()
        {
            Data = new Dictionary<DateTime, StockSaleSet>();
        }

        /// <summary>
        /// Model of the data to be stored
        /// </summary>
        public class StockSaleSet
        {
            public decimal Stock { get; set; }
            public decimal Sale { get; set; }
        }

        public Report Report { get; set; }

        /// <summary>
        /// Dataset dictionary for the data.
        /// </summary>
        public Dictionary<DateTime, StockSaleSet> Data { get; set; }
    }
}