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
        // import WRE dlls
        [DllImport(@"wre-ensimag-c.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]

        // declaration
        public static extern int WREmodelingCov(
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
            res = WREmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
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

        static void Main(string[] args)
        {
            // header
            Console.WriteLine("******************************");
            Console.WriteLine("*    WREmodelingCov in C#   *");
            Console.WriteLine("******************************");

            // sample data
            SemiHistoricDataFeedProvider sdf = new SemiHistoricDataFeedProvider();
            string[] ids = new string[] { "AC FP" };
            List<DataFeed> dfList = sdf.GetDataFeed(ids, new DateTime(2010, 01, 05), new DateTime(2012,12,30));
            int nbValues = dfList.Count;
            double[] portfolioreturns = new double[nbValues];

            for (int i = 1; i < nbValues; i++)
            {
                portfolioreturns[i] = Math.Log((double)dfList[i].PriceList["AC FP"] / (double)dfList[i-1].PriceList["AC FP"])* Math.Sqrt(252) ;
            }

            //int nbValues = 214;

            int info = 0;

            double[] vol = { 0 };
            nbValues--;
            int v = WREanalysisExpostVolatility(ref nbValues , portfolioreturns, vol , ref info);
            Console.WriteLine(v);
            Console.WriteLine(vol[0] );
            Console.ReadLine(); 
        }
    }
}
