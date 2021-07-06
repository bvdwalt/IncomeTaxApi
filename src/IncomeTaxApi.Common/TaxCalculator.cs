using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncomeTaxApi.Common
{
    public class TaxCalculator
    {
        private readonly double MaxUIFContributionPm = 177.12;
        private static Dictionary<int, List<TaxBracket>> TaxYears = new Dictionary<int, List<TaxBracket>>();
        private static Dictionary<int, List<AgeBracket>> AgeGroupRebates = new Dictionary<int, List<AgeBracket>>();
        public TaxCalculator()
        {
            TaxYears = new Dictionary<int, List<TaxBracket>>();
            AgeGroupRebates = new Dictionary<int, List<AgeBracket>>();
            TaxYears.Add(2022, new List<TaxBracket> {
                new TaxBracket(1, 216200, .18, 0),
                new TaxBracket(216201, 337800, .26, 38916),
                new TaxBracket(337801, 467500, .31, 70532),
                new TaxBracket(467501, 613600, .36, 110739),
                new TaxBracket(613601, 782200, .39, 163335),
                new TaxBracket(782201, 1656600, .41, 229089),
                new TaxBracket(1656600, double.MaxValue, .45, 587593)
            });

            AgeGroupRebates.Add(2022, new List<AgeBracket> {
                new AgeBracket(0, 64, 15714),
                new AgeBracket(65, 74, 8613),
                new AgeBracket(75, int.MaxValue, 2871)
            });

            TaxYears.Add(2021, new List<TaxBracket> {
                new TaxBracket(1, 205900, .18, 0),
                new TaxBracket(205901, 321600, .26, 37062),
                new TaxBracket(321601, 445100, .31, 67144),
                new TaxBracket(445101, 584200, .36, 105429),
                new TaxBracket(584201, 744800, .39, 155505),
                new TaxBracket(744801, 1577300, .41, 218139),
                new TaxBracket(1577301, double.MaxValue, .45, 559464)
            });

            AgeGroupRebates.Add(2021, new List<AgeBracket> {
                new AgeBracket(0, 64, 14958),
                new AgeBracket(65, 74, 8199),
                new AgeBracket(75, int.MaxValue, 2736)
            });
        }

        public TaxCalculationResult CalculateIncomeTax(double grossIncome, int age, int taxYear)
        {
            TaxYears.TryGetValue(taxYear, out var taxBracketsForYear);
            AgeGroupRebates.TryGetValue(taxYear, out var ageBracketForYear);

            if (taxBracketsForYear == null)
                return new TaxCalculationResult() { ErrorText = $"Tax year {taxYear} not found" };

            if (ageBracketForYear == null)
                return new TaxCalculationResult() { ErrorText = $"Age bracket for Tax year {taxYear} and Age {age} not found" };

            var bracket = taxBracketsForYear.Single(b => grossIncome >= b.LowerBound && grossIncome <= b.UpperBound);
            var ageBracket = ageBracketForYear.Single(b => age >= b.LowerBoundAge && age <= b.UpperBoundAge);

            var totalTax = bracket.BaseTax == 0 ? bracket.TaxPercentage * grossIncome : bracket.BaseTax + (bracket.TaxPercentage * (grossIncome - bracket.ThresholdAmount));

            totalTax -= ageBracket.RebateAmount;

            var UIFContributionPa = (.01 * grossIncome) / 12 > MaxUIFContributionPm ? MaxUIFContributionPm * 12 : .01 * grossIncome;

            var IncomeAfterTax = Math.Round(grossIncome - totalTax - UIFContributionPa, 2);

            var taxPercentage = Math.Round((totalTax / grossIncome) * 100, 2);

            return new TaxCalculationResult(IncomeAfterTax, grossIncome, totalTax, taxPercentage, taxYear, UIFContributionPa);
        }

        public TaxCalculationResult CalculateIncomeTaxPerMonth(double grossIncome, int age, int taxYear)
        {
            var result = CalculateIncomeTax(grossIncome * 12, age, taxYear);

            result.GrossIncome = Math.Round(result.GrossIncome / 12, 2);
            result.IncomeAfterTax = Math.Round(result.IncomeAfterTax / 12, 2);
            result.TotalTax = Math.Round(result.TotalTax / 12, 2);
            result.UIFContribution = Math.Round(result.UIFContribution / 12, 2);

            return result;
        }
    }
}
