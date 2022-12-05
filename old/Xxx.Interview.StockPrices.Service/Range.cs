namespace Cibc.StockPrices.Service
{
    public readonly struct Range
    {
        public int Min { get; }

        public int Max { get; }

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}