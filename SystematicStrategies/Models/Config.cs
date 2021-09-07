using PricingLibrary.FinancialProducts;
using System;

namespace SystematicStrategies.Models
{
    internal class Config
    {
        public string type;
        public string name;
        public double strike;
        public Share[] underlyingShares;
        public double[] weights;
        public DateTime startDate;
        public DateTime maturity;
        public int estimatedWindowSize;
        public string dataType;
    }
}