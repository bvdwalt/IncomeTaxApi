namespace IncomeTaxApi.Common
{
    public class TaxCalculationResult
    {
        public TaxCalculationResult(double incomeAfterTax, double grossIncome, double totalTax, double taxPercentage, int taxYear, double uIFContributionPa)
        {
            IncomeAfterTax = incomeAfterTax;
            GrossIncome = grossIncome;
            TotalTax = totalTax;
            TaxPercentage = taxPercentage;
            TaxYear = taxYear;
            UIFContribution = uIFContributionPa;
        }
        public double IncomeAfterTax { get; set; }
        public double GrossIncome { get; set; }
        public double TotalTax { get; set; }
        public double TaxPercentage { get; set; }
        public int TaxYear { get; set; }
        public double UIFContribution { get; set; }
    }
}