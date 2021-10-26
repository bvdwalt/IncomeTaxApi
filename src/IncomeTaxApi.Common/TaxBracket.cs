namespace IncomeTaxApi.Common
{
    public class TaxBracket
    {
        public TaxBracket(double lowerBound, double upperBound, double taxPercentage, double baseTax, double thresholdAmount = 0)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            TaxPercentage = taxPercentage;
            BaseTax = baseTax;
            ThresholdAmount = thresholdAmount == 0 ? lowerBound - 1 : thresholdAmount;
        }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public double TaxPercentage { get; set; }
        public double BaseTax { get; set; }
        public double ThresholdAmount { get; set; }
    }
}
