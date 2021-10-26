using IncomeTaxApi.Common;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IncomeTaxApi.Tests
{
    public class SouthAfricaTaxCalculatorTests
    {
        private TaxCalculator _calculator;
        [OneTimeSetUp]
        public void Setup()
        {
            _calculator = new TaxCalculator();
        }

        public static IEnumerable<TestCaseData> MonthlyTestCases
        {
            get
            {
                yield return new TestCaseData(10000, 2021, 40, 9346.50);
                yield return new TestCaseData(10000, 2022, 40, 9409.5);
                yield return new TestCaseData(20000, 2021, 40, 17242.046666666665);
                yield return new TestCaseData(20000, 2022, 40, 17373.713333333333);
                yield return new TestCaseData(150000, 2021, 65, 95532.880000000005);
                yield return new TestCaseData(150000, 2022, 65, 96197.00916666667);
            }
        }

        [TestCaseSource(nameof(MonthlyTestCases))]
        public void MonthlyIncomeTests(double grossIncome, int taxYear, int age, double expectedIncomeAfterTax)
        {
            var result = _calculator.CalculateIncomeTaxPerMonth(grossIncome, age, taxYear);

            Assert.AreEqual(expectedIncomeAfterTax, result.IncomeAfterTax);
        }

        public static IEnumerable<TestCaseData> YearlyTestCases
        {
            get
            {
                yield return new TestCaseData(120000, 2021, 40, 112158);
                yield return new TestCaseData(120000, 2022, 40, 112914);
                yield return new TestCaseData(240000, 2021, 40, 206904.56);
                yield return new TestCaseData(240000, 2022, 40, 208484.56);
                yield return new TestCaseData(1800000, 2021, 65, 1146394.5600000001);
                yield return new TestCaseData(1800000, 2022, 65, 1154364.1100000001);
            }
        }

        [TestCaseSource(nameof(YearlyTestCases))]
        public void YearlyIncomeTests(double grossIncome, int taxYear, int age, double expectedIncomeAfterTax)
        {
            var result = _calculator.CalculateIncomeTax(grossIncome, age, taxYear);

            Assert.AreEqual(expectedIncomeAfterTax, result.IncomeAfterTax);
        }
    }
}