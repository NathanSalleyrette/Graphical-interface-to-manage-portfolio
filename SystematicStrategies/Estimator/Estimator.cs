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
        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREmodelingLogReturns", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingLogReturns(
            ref int nbValues, ref int nbAsset, double[,] assetValues, ref int horizon, double[,] assetreturns, ref int info
            );

        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREanalysisExpostVolatility", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREanalysisExpostVolatility(ref int nbValues, double[] portfolioreturns, double[] expostVolatility, ref int info);

        // declaration
        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREmodelingCorr", CallingConvention = CallingConvention.Cdecl)]

        // declaration
        public static extern int WREmodelingCorr(
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

        public static double[,] computeCovarianceMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCorr(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return covMatrix;
        }

        public static double[,] GetReturns(List<DataFeed> dfList, string[] ids)
        {
            double[,] covportfolioreturns = new double[dfList.Count, ids.Length];

            int dataSize = covportfolioreturns.GetLength(0);
            int nbAssets = covportfolioreturns.GetLength(1);

            for (int i = 0; i < dataSize; i++)
            {
                for (int j = 0; j < nbAssets; j++)
                {
                    covportfolioreturns[i, j] = (double)dfList[i].PriceList[ids[j]];
                }
            }

            double[,] assetreturns = new double[dataSize - 1, nbAssets];
            int info = 0;
            int horizon = 1;
            dataSize--;
            int res = WREmodelingLogReturns(ref dataSize, ref nbAssets, covportfolioreturns, ref horizon, assetreturns, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return assetreturns;
        }

        public static double[,] Update(double[,] covMatrix, int numberOfDays)
        {
            for (int i = 0; i < covMatrix.GetLength(0); i++)
            {
                covMatrix[i, i] /= Math.Sqrt(Math.Sqrt(numberOfDays));

            }
            return covMatrix;
        }

        public double[,] CovMatrix(List<DataFeed> dfList, string[] ids, int numberOfDaysPerYear)
        {
            double[,] covportfolioreturns = GetReturns(dfList, ids);

            double[,] covMatrix = computeCovarianceMatrix(covportfolioreturns);

            double[,] covMatrixUpdate = Update(covMatrix, numberOfDaysPerYear);
            return covMatrixUpdate;

        }
    }
}
