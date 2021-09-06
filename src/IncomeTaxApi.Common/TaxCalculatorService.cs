using System;
using System.Collections.Generic;
using System.Linq;

namespace bvdwalt.IncomeTax.Common
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly double _uifLevyPercentage = .01;
        private readonly double _maxUIFContributionPmZar = 177.12;
        private static Dictionary<int, List<TaxBracket>> _taxYears = new Dictionary<int, List<TaxBracket>>();
        private static Dictionary<int, List<AgeBracket>> _ageGroupRebates = new Dictionary<int, List<AgeBracket>>();

        private static Dictionary<string, TaxCalculationResult> _cachedResults;

        public TaxCalculatorService()
        {
            _cachedResults = new Dictionary<string, TaxCalculationResult>();
            _taxYears = new Dictionary<int, List<TaxBracket>>();
            _ageGroupRebates = new Dictionary<int, List<AgeBracket>>();
            _taxYears.Add(2022, new List<TaxBracket> {
                new TaxBracket(1, 216200, .18, 0),
                new TaxBracket(216201, 337800, .26, 38916),
                new TaxBracket(337801, 467500, .31, 70532),
                new TaxBracket(467501, 613600, .36, 110739),
                new TaxBracket(613601, 782200, .39, 163335),
                new TaxBracket(782201, 1656600, .41, 229089),
                new TaxBracket(1656600, double.MaxValue, .45, 587593)
            });

            _ageGroupRebates.Add(2022, new List<AgeBracket> {
                new AgeBracket(0, 64, 15714),
                new AgeBracket(65, 74, 8613),
                new AgeBracket(75, int.MaxValue, 2871)
            });

            _taxYears.Add(2021, new List<TaxBracket> {
                new TaxBracket(1, 205900, .18, 0),
                new TaxBracket(205901, 321600, .26, 37062),
                new TaxBracket(321601, 445100, .31, 67144),
                new TaxBracket(445101, 584200, .36, 105429),
                new TaxBracket(584201, 744800, .39, 155505),
                new TaxBracket(744801, 1577300, .41, 218139),
                new TaxBracket(1577301, double.MaxValue, .45, 559464)
            });

            _ageGroupRebates.Add(2021, new List<AgeBracket> {
                new AgeBracket(0, 64, 14958),
                new AgeBracket(65, 74, 8199),
                new AgeBracket(75, int.MaxValue, 2736)
            });
        }

        public TaxCalculationResult CalculateIncomeTaxPerAnnum(double grossIncome, int age, int taxYear)
        {
            // Get approriate Tax bracket and age group the this tax year.
            _taxYears.TryGetValue(taxYear, out var taxBracketsForYear);
            _ageGroupRebates.TryGetValue(taxYear, out var ageBracketForYear);

            if (taxBracketsForYear == null)
                return new TaxCalculationResult() { ErrorText = $"Tax year {taxYear} not found" };

            if (ageBracketForYear == null)
                return new TaxCalculationResult() { ErrorText = $"Age bracket for Tax year {taxYear} and Age {age} not found" };

            // Get the Income Tax bracket and age bracket for the tax year
            var taxBracket = taxBracketsForYear.Single(b => grossIncome >= b.LowerBoundIncomeBeforeTax && grossIncome <= b.UpperBoundIncomeBeforeTax);
            var ageBracket = ageBracketForYear.Single(b => age >= b.LowerBoundAge && age <= b.UpperBoundAge);

            if (_cachedResults.TryGetValue(GetCacheKey(grossIncome, taxYear, taxBracket, ageBracket), out var cachedResult))
            {
                return cachedResult;
            }

            // Calculate deductions
            double totalDeductions = 0;
            // if there is no base tax rate
            if (taxBracket.BaseTax == 0)
            {
                totalDeductions = (taxBracket.TaxPercentage * grossIncome);
            }
            // else add the base tax plus calculate tax on income above threshold
            else
            {
                totalDeductions = taxBracket.BaseTax + (taxBracket.TaxPercentage * (grossIncome - taxBracket.ThresholdAmount));
            }

            // Apply rebate based on age bracket
            totalDeductions -= ageBracket.RebateAmount;

            // Calculate UIF
            var UIFOnGrossIncome = _uifLevyPercentage * grossIncome;

            // If the UIF contribution of more than the maximum allowed, then just use the max. 
            var UIFContributionPa = (UIFOnGrossIncome / 12) > _maxUIFContributionPmZar ? _maxUIFContributionPmZar * 12 : UIFOnGrossIncome;

            // Income after tax, GrossIncome - all deductions
            var IncomeAfterTax = grossIncome - totalDeductions - UIFContributionPa;

            // Calculate effective Tax rate
            var taxPercentage = Math.Round((totalDeductions / grossIncome) * 100, 2);

            // Return Calculation Result
            var result = new TaxCalculationResult(IncomeAfterTax, grossIncome, totalDeductions, taxPercentage, taxYear, UIFContributionPa);

            _cachedResults.TryAdd(GetCacheKey(grossIncome, taxYear, taxBracket, ageBracket), result);

            return result;
        }

        private string GetCacheKey(double grossIncome, int taxYear, TaxBracket taxBracket, AgeBracket ageBracket)
        {
            return $"{taxYear};{grossIncome};{taxBracket.LowerBoundIncomeBeforeTax};{ageBracket.LowerBoundAge}";
        }

        public TaxCalculationResult CalculateIncomeTaxPerMonth(double grossIncome, int age, int taxYear)
        {
            var result = CalculateIncomeTaxPerAnnum(grossIncome * 12, age, taxYear);

            ChangePerAnnumCalculationResultsToPerMonth(ref result);

            return result;
        }

        private void ChangePerAnnumCalculationResultsToPerMonth(ref TaxCalculationResult result)
        {
            result.GrossIncome /= 12;
            result.IncomeAfterTax /= 12;
            result.TotalDeductions /= 12;
            result.UIFContribution /= 12;
        }
    }
}
