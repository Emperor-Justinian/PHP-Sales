using PHP.Sales.Core.Models.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PHP.Sales.Web.ViewModels
{
    public enum PredictType
    {
        WEEKLY,
        MONTHLY
    }

    public class PredictModel
    {
        public double Value { get; set; }
        public bool IsPredict { get; set; }
    }

    public class PredictionChartViewModel
    {

        public PredictionChartViewModel()
        {
            CurrentCycle = new Dictionary<DateTime, PredictModel>();
            NextCycle = new Dictionary<DateTime, PredictModel>();
        }

        [DisplayName("Predition Method")]
        public PredictType Type { get; set; }

        public string Name { get; set; }

        [DisplayName("Product")]
        public Guid ProductID { get; set; }
        public Product Product { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Dictionary<DateTime, PredictModel> CurrentCycle { get; set; }
        public Dictionary<DateTime, PredictModel> NextCycle { get; set; }
    }
}