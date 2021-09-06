namespace bvdwalt.IncomeTax.Common
{
    public class TaxBracket
    {
        public TaxBracket(double lowerBoundIncomeBeforeTax, double upperBoundIncomeBeforeTax, double taxPercentage, double baseTax, double thresholdAmount = 0)
        {
            LowerBoundIncomeBeforeTax = lowerBoundIncomeBeforeTax;
            UpperBoundIncomeBeforeTax = upperBoundIncomeBeforeTax;
            TaxPercentage = taxPercentage;
            BaseTax = baseTax;
            ThresholdAmount = thresholdAmount == 0 ? lowerBoundIncomeBeforeTax - 1 : thresholdAmount;
        }
        public double LowerBoundIncomeBeforeTax { get; set; }
        public double UpperBoundIncomeBeforeTax { get; set; }
        public double TaxPercentage { get; set; }
        public double BaseTax { get; set; }
        public double ThresholdAmount { get; set; }
    }
}
