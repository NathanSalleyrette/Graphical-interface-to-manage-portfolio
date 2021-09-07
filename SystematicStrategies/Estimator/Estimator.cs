using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.Estimator
{
    class Estimator
    {
        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREanalysisExpostVolatility", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREanalysisExpostVolatility(ref int nbValues, double[] portfolioreturns, double[] expostVolatility, ref int info);

        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]

        // declaration
        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

        public double Volatity(List<DataFeed> dfList, int numberOfDaysPerYear, string id)
        {


            int nbValues = dfList.Count;
            double[] portfolioreturns = new double[nbValues];

            for (int i = 1; i < nbValues; i++)
            {
                double log = Math.Log((double)dfList[i].PriceList[id] / (double)dfList[i - 1].PriceList[id]);
                portfolioreturns[i] = log * Math.Sqrt(numberOfDaysPerYear / DayCount.CountBusinessDays(dfList[i - 1].Date, dfList[i].Date));
            }

            int info = 0;

            double[] volatility = { 0 };
            nbValues--;
            int res = WREanalysisExpostVolatility(ref nbValues, portfolioreturns, volatility, ref info);

            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREanalysisExpostVolatility encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREanalysisExpostVolatility encountred a problem. See info parameter for more details");
            }

            return volatility[0];
        }

        public double[,] CovMatrix(List<DataFeed> dfList, string[] ids, int numberOfDaysPerYear)
        {
            double[,] portfolioreturns = new double[dfList.Count, ids.Length];

            int dataSize = portfolioreturns.GetLength(0);
            int nbAssets = portfolioreturns.GetLength(1);

            for(int i = 1; i < dataSize; i++)
            {
                for(int j = 0; j < nbAssets; j++)
                {
                    double log = Math.Log((double)dfList[i].PriceList[ids[j]] / (double)dfList[i - 1].PriceList[ids[j]]);
                    portfolioreturns[i, j] = log * Math.Sqrt(numberOfDaysPerYear / DayCount.CountBusinessDays(dfList[i - 1].Date, dfList[i].Date));
                }
            }

            double[,] covMatrix = new double[nbAssets, nbAssets];

            int info = 0;
            int res;
            res = WREmodelingCov(ref dataSize, ref nbAssets, portfolioreturns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }

            return covMatrix;

        }
    }
}
