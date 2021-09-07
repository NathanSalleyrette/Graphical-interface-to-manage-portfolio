using System;
using System.Collections.Generic;
using System.Linq;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.Utilities;
using System.Runtime.InteropServices;

namespace SystematicStrategies.main
{
    class Program
    {

        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREmodelingLogReturns", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingLogReturns(
            ref int nbValues, ref int nbAsset, double[,] assetValues, ref int horizon, double[,] assetreturns, ref int info 
            );

        // import WRE dlls
        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREmodelingCorr", CallingConvention = CallingConvention.Cdecl)]

        // declaration
        public static extern int WREmodelingCorr(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREanalysisExpostVolatility", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREanalysisExpostVolatility(ref int nbValues, double[] portfolioreturns, double[] expostVolatility, ref int info);

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

        public static void dispMatrix(double[,] myCovMatrix)
        {
            int n = myCovMatrix.GetLength(0);

            Console.WriteLine("Covariance matrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(myCovMatrix[i, j] + "\t");
                }
                Console.Write("\n");
            }
        }

        public static double[,] GetReturns(List<DataFeed> dfList, string[] ids)
        {
            double[,] covportfolioreturns = new double[dfList.Count, ids.Length];

            int dataSize = covportfolioreturns.GetLength(0);
            int nbAssets = covportfolioreturns.GetLength(1);

            // for (int i = 1; i < dataSize; ++i)
            //{
            //  for (int j = 0; j < nbAssets; ++j)
            //{
            //  double logreturn = Math.Log((double)(dfList[i].PriceList[ids[j]] / dfList[i-1].PriceList[ids[j]]));
            //covportfolioreturns[i-1, j] = logreturn;
            // }
            //}

            for(int i = 0; i<dataSize; i++)
            {
                for(int j = 0; j<nbAssets; j++)
                {
                    covportfolioreturns[i, j] = (double)dfList[i].PriceList[ids[j]];
                }
            }

            double[,] assetreturns = new double[dataSize-1, nbAssets];
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
            for(int i = 0; i<covMatrix.GetLength(0); i++)
            {
                covMatrix[i, i] /= Math.Sqrt(Math.Sqrt(numberOfDays));
                
            }
            return covMatrix;
        }
        

        static void Main(string[] args)
        {
            // header
            Console.WriteLine("******************************");
            Console.WriteLine("*    WREmodelingCov in C#   *");
            Console.WriteLine("******************************");

            // sample data
            SimulatedDataFeedProvider sdf = new SimulatedDataFeedProvider();
            string[] ids = new string[] { "AC FP", "ACA FP" ,"bim"};
            List<DataFeed> dfList = sdf.GetDataFeed(ids, new DateTime(2000, 01, 05), new DateTime(2041,12,30));
            int nbValues = dfList.Count;
            double[] portfolioreturns = new double[nbValues];

            for (int i = 1; i < nbValues; i++)
            {
                double log = Math.Log((double)dfList[i].PriceList["AC FP"] / (double)dfList[i - 1].PriceList["AC FP"]);
                portfolioreturns[i] = log * Math.Sqrt(sdf.NumberOfDaysPerYear/ DayCount.CountBusinessDays(dfList[i-1].Date, dfList[i].Date) );
            }

            int info = 0;
            double[] vol = { 0 };
            nbValues--;
            int res = WREanalysisExpostVolatility(ref nbValues , portfolioreturns, vol , ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREanalysisExpostVolatility encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREanalysisExpostVolatility encountred a problem. See info parameter for more details");
            }
            Console.WriteLine(vol[0] );

            double[,] covportfolioreturns = GetReturns(dfList, ids);

            double[,] covMatrix = computeCovarianceMatrix(covportfolioreturns);

            double[,] covMatrixUpdate = Update(covMatrix, sdf.NumberOfDaysPerYear);


            dispMatrix(covMatrix);



            Console.ReadLine();

        }
    }
}
