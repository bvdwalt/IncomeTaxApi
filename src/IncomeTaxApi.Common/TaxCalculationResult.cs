namespace IncomeTaxApi.Common
{
    public class TaxCalculationResult
    {
        public TaxCalculationResult()
        {

        }
        public TaxCalculationResult(double incomeAfterTax, double grossIncome, double totalDeductions, double taxPercentage, int taxYear, double uIFContributionPa)
        {
            IncomeAfterTax = incomeAfterTax;
            GrossIncome = grossIncome;
            TotalDeductions = totalDeductions;
            TaxPercentage = taxPercentage;
            TaxYear = taxYear;
            UIFContribution = uIFContributionPa;
        }
        public double IncomeAfterTax { get; set; }
        public double GrossIncome { get; set; }
        public double TotalDeductions { get; set; }
        public double TaxPercentage { get; set; }
        public int TaxYear { get; set; }
        public double UIFContribution { get; set; }
        public string ErrorText { get; set; }
    }
}