using NUnit.Framework;
using PHP.Sales.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHP.Sales.Logic.Tests
{
    [TestFixture()]
    public class AlgorithumTests
    {
        [Test()]
        public void LinearRegression()
        {
            //arrange
            double[] record = { 72, 110, 117, 172, 76, 112, 130, 194, 78, 119, 128, 201 };

            double[] xyVals = new double[record.Length];
            double[] xSqVals = new double[record.Length];

            //act
            for (int i = 0; i < record.Length; i++)
            {
                xyVals[i] = (i + 1) * record[i];
                xSqVals[i] = Math.Pow(i + 1, 2);
            }

            double xSum = (record.Length * (record.Length + 1)) / 2;
            double aVal = ((record.Sum()) * (xSqVals.Sum()) - (xSum * xyVals.Sum())) / ((record.Length * xSqVals.Sum()) - Math.Pow(xSum, 2));

            double regGrad = (record.Length * (xyVals.Sum()) - xSum * (record.Sum())) / (record.Length * xSqVals.Sum() - Math.Pow(xSum, 2));

            //assert
            Assert.GreaterOrEqual(regGrad, 5, "The gradient is lower than expected");
            Assert.LessOrEqual(regGrad, 5.5, "The gradient is greater than expected");
        }

        [Test()]
        public void SeasonalAdjustment()
        {
            //arrange
            //double[] record = { 72, 110, 117, 172, 76, 112, 130, 194, 78, 119, 128, 201 };
            double[] data = {72, 110, 117, 172, 76, 112, 130, 194, 78};

            Algorithum a = new Algorithum(data, 3);

            //act
            double e = a.Prediction(1);

            //assert
            Assert.GreaterOrEqual(e, 136, "The event is lower than expected");
            Assert.LessOrEqual(e, 140, "The event is greater than expected");
        }

        [Test()]
        public void PredictionZeroTest()
        {
            //arrange
            double[] record = { 100, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

            Algorithum a = new Algorithum(record, 3);

            //act
            double e = a.Prediction(1);

            //assert
            Assert.Zero(e, "The event is lower than expected");
        }

        [Test()]
        public void DecimalTest()
        {
            //arrange
            double[] record = { 72, 110, 117, 172, 76, 112, 130, 194, 78, 119, 128, 201 };

            Algorithum a = new Algorithum(record, 3);

            //act
            double e = a.Prediction(1);

            string[] res = e.ToString().Split('.');
            int precision = res[1].Length;

            //assert
            Assert.IsTrue(precision == 3, "The precision is not to 3 decimal places");
        }
    }
}