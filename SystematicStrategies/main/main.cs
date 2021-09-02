using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary;
namespace SystematicStrategies.main
{
    class main
    {
        PricingLibrary.FinancialProducts.VanillaCall n = new PricingLibrary.FinancialProducts.VanillaCall("oui", new PricingLibrary.FinancialProducts.Share("loic", "1"), new DateTime(), 23.0 );
    }
}
