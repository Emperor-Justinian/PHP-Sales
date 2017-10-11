using System;
using System.Linq;

namespace PHP.Sales.Logic
{
    public class Algorithum
    {
        int RecordLength;
        int SeasonLength;
        
        private double[] unAdjReg;
        private double[] seasonalIndex;

        public Algorithum(double[] record, int seasonLength)
        {
            RecordLength = record.Length;
            SeasonLength = seasonLength;
            //int[] record = {72,110,117,172,76,112,130,194,78,119,128,201};

            // Generate the unadjusted regression forecast model 
            double[] xyVals = new double[record.Length];
            double[] xSqVals = new double[record.Length];
            double[] demandForecast = new double[record.Length];
            double[] adjReg = new double[record.Length];

            unAdjReg = new double[record.Length];
            seasonalIndex = new double[record.Length];
            
            // Compute arrasy for use with linear regression 
            for (int i = 0 ; i < record.Length ; i++)
            {
                xyVals[i] = (i+1) * record[i];
                xSqVals[i] = Math.Pow(i+1,2);
            }

            double xSum = (record.Length * (record.Length + 1)) / 2;

            // Calculate the linear coefficents for the regression function
            double aVal = ((record.Sum()) * (xSqVals.Sum()) - (xSum * xyVals.Sum())) / ((record.Length * xSqVals.Sum()) - Math.Pow(xSum, 2));

            double bVal = (record.Length * (xyVals.Sum()) - xSum * (record.Sum())) / (record.Length * xSqVals.Sum() - Math.Pow(xSum, 2));

            // Compute UnAdjusted array
            for (int i = 0 ; i < record.Length ; i++)
            {
                unAdjReg[i] = aVal + bVal * (i + 1);// record[i];
            }

            // Compute Demand Forcast Array
            for (int i = 0; i < record.Length; i++)
            {
                demandForecast[i] = record[i] / unAdjReg[i];
            }

            //Create Seasonal index for all of the time periods
            if (record.Length == seasonLength)
                seasonalIndex = demandForecast;
            else // if monthly sales is larger than 12 ie 24, 36, etc
            {
                for (int i = 0; i < seasonLength; i++)
                {
                    double sumDemand = 0;

                    //DEAL WITH MID-CYCLES
                    for (int j = 0; j < (record.Length / seasonLength) ; j++)
                    {
                        sumDemand += demandForecast[i+(j* seasonLength)];
                    }

                    for (int j = 0; j < (record.Length / seasonLength); j++)
                    {
                        seasonalIndex[i+(j* seasonLength)] = sumDemand / (record.Length / seasonLength);
                    }
                }
            }

            // Calculate prediction model
            for (int i = 0; i < record.Length; i++)
            {
                adjReg[i] = unAdjReg[i] * seasonalIndex[i];
            }
        }

        /// <summary>
        /// Compute Forecasted value
        /// </summary>
        /// <param name="cycleEvent">Event cycle after end of data</param>
        /// <returns>The predicted value</returns>
        public double Prediction(int cycleEvent)
        {
            //double a = unAdjReg.Last();
            //double b = (unAdjReg.Last() - unAdjReg[unAdjReg.Length - 2]);
            //double c = seasonalIndex[seasonalIndex.Length - 4];
            return (unAdjReg.Last() + (unAdjReg.Last() - unAdjReg[unAdjReg.Length - cycleEvent])) * seasonalIndex[seasonalIndex.Length - SeasonLength];
        }
    }
}
