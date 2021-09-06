namespace bvdwalt.IncomeTax.Common
{
    public interface ITaxCalculatorService
    {
        TaxCalculationResult CalculateIncomeTaxPerAnnum(double grossIncome, int age, int taxYear);

        TaxCalculationResult CalculateIncomeTaxPerMonth(double grossIncome, int age, int taxYear);
    }
}