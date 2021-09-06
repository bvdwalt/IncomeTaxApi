namespace bvdwalt.IncomeTax.Common
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
            EffectiveTaxRate = taxPercentage;
            TaxYear = taxYear;
            UIFContribution = uIFContributionPa;
        }
        public double IncomeAfterTax { get; set; }
        public double GrossIncome { get; set; }
        public double TotalDeductions { get; set; }
        public double EffectiveTaxRate { get; set; }
        public int TaxYear { get; set; }
        public double UIFContribution { get; set; }
        public string ErrorText { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(ErrorText)) return ErrorText;

            return $"TY:{TaxYear}, Gross: {GrossIncome}, AfterTax:{IncomeAfterTax}, Deductions:{TotalDeductions}, EffectiveTaxRate: {EffectiveTaxRate}, UIFContribution:{UIFContribution}";
        }
    }
}