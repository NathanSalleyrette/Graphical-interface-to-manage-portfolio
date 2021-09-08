
namespace SystematicStrategies
{
    internal class Controller : AbstractController
    {



        public override void CalculVolatilities()
        {
            var n = optionToHedge.UnderlyingShareIds.Length;
            for (var j = 0; j < n * n; j++) corMatrix[j % n, j / n] = 0.15;
            var i = 0;
            foreach (string UnderlyingShareId in optionToHedge.UnderlyingShareIds)
            {
                volatilities[i] = 0.25;
                corMatrix[i, i] = 1;
                i += 1;
            };
        }

        public override int WindowSizeSetter(int windowSize)
        {
            return 1;
        }


    }
}
