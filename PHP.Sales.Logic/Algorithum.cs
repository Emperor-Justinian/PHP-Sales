using System;
using System.Linq;

namespace PHP.Sales.Logic
{
    public class Algorithum
    {
        int RecordLength;
        int SeasonLength;

        double regGrad;
        
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
            seasonalIndex = new double[SeasonLength];
            
            // Compute arrasy for use with linear regression 
            for (int i = 0 ; i < record.Length ; i++)
            {
                xyVals[i] = (i+1) * record[i];
                xSqVals[i] = Math.Pow(i+1,2);
            }

            double xSum = (record.Length * (record.Length + 1)) / 2;

            // Calculate the linear coefficents for the regression function
            double aVal = ((record.Sum()) * (xSqVals.Sum()) - (xSum * xyVals.Sum())) / ((record.Length * xSqVals.Sum()) - Math.Pow(xSum, 2));

            regGrad = (record.Length * (xyVals.Sum()) - xSum * (record.Sum())) / (record.Length * xSqVals.Sum() - Math.Pow(xSum, 2));

            // Compute UnAdjusted array
            for (int i = 0 ; i < record.Length ; i++)
            {
                unAdjReg[i] = aVal + regGrad * (i + 1);// record[i];
            }

            // Compute Demand Forcast Array
            for (int i = 0; i < record.Length; i++)
            {
                demandForecast[i] = record[i] / unAdjReg[i];
            }

            //Create Seasonal index for all of the time periods
            if (record.Length == SeasonLength)
                seasonalIndex = demandForecast;
            else // if monthly sales is larger than 12 ie 24, 36, etc
            {
                for (int i = 0; i < SeasonLength; i++)
                {
                    double sumDemand = 0;
                    int seasonCycle = 0;

                    int cycleCount = (RecordLength % SeasonLength == 0) ? RecordLength / SeasonLength : (RecordLength / SeasonLength) + 1;
                    while ((seasonCycle*SeasonLength+i) < demandForecast.Count())
                    {
                        sumDemand += demandForecast[i + (seasonCycle++ * SeasonLength)];
                    }


                    //DEAL WITH MID-CYCLES
                    /*for (int j = 0; j < (record.Length / seasonLength) ; j++)
                    {
                        sumDemand += demandForecast[i+(j*seasonLength)];
                        if (j % seasonLength == 0) seasonCycle++;
                    }*/

                    seasonalIndex[i] = sumDemand / (record.Length / cycleCount);
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
            return (unAdjReg.Last() + regGrad*cycleEvent * seasonalIndex[(RecordLength + cycleEvent) % SeasonLength]);
        }
    }
}
