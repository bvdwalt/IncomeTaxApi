using bvdwalt.IncomeTax.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace bvdwalt.IncomeTax
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class IncomeTax
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        public IncomeTax(ITaxCalculatorService taxCalculatorService)
        {
            _taxCalculatorService = taxCalculatorService;
        }

        [FunctionName("IncomeTaxPerAnnum")]
        [OpenApiOperation(operationId: "IncomeTaxPerAnnum", tags: new[] { "Personal Income Tax South Africa" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "GrossIncome", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Your per annum income before tax gets deducted")]
        [OpenApiParameter(name: "TaxYear", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The Tax Year this income is for")]
        [OpenApiParameter(name: "Age", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Your Age during this tax year")]
        [OpenApiParameter(name: "SpecificProperty", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "If you only want a single property to be returned, e.g. IncomeAfterTax")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> IncomeTaxPerAnnum(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            double GrossIncome = double.Parse(req.Query["GrossIncome"]);
            int TaxYear = int.Parse(req.Query["TaxYear"]);
            int Age = int.Parse(req.Query["Age"]);
            string SpecificProperty = req.Query["SpecificProperty"];

            TaxCalculationResult result = _taxCalculatorService.CalculateIncomeTaxPerAnnum(GrossIncome, Age, TaxYear);

            if (!string.IsNullOrWhiteSpace(result.ErrorText)) return new NotFoundObjectResult(new OpenApiError(new Microsoft.OpenApi.Exceptions.OpenApiException(result.ErrorText)));

            if(!string.IsNullOrWhiteSpace(SpecificProperty))
            {
                return ParseSpecificProperty(result, SpecificProperty);
            }

            return new JsonResult(result);
        }

        [FunctionName("IncomeTaxPerMonth")]
        [OpenApiOperation(operationId: "IncomeTaxPerMonth", tags: new[] { "Personal Income Tax South Africa" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "GrossIncome", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Your per month income before tax gets deducted")]
        [OpenApiParameter(name: "TaxYear", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The Tax Year this income is for")]
        [OpenApiParameter(name: "Age", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Your Age during this tax year")]
        [OpenApiParameter(name: "SpecificProperty", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "If you only want a single property to be returned, e.g. IncomeAfterTax")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> IncomeTaxPerMonth(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            double GrossIncome = double.Parse(req.Query["GrossIncome"]);
            int TaxYear = int.Parse(req.Query["TaxYear"]);
            int Age = int.Parse(req.Query["Age"]);
            string SpecificProperty = req.Query["SpecificProperty"];

            TaxCalculationResult result = _taxCalculatorService.CalculateIncomeTaxPerMonth(GrossIncome, Age, TaxYear);

            if (!string.IsNullOrWhiteSpace(result.ErrorText)) throw new Microsoft.OpenApi.Exceptions.OpenApiException(result.ErrorText);

            if (!string.IsNullOrWhiteSpace(SpecificProperty))
            {
                return ParseSpecificProperty(result, SpecificProperty);
            }

            return new JsonResult(result);
        }

        private static JsonResult ParseSpecificProperty(TaxCalculationResult result, string SpecificProperty)
        {
            double propertyValue;
            switch (SpecificProperty)
            {
                case nameof(TaxCalculationResult.IncomeAfterTax):
                    propertyValue = result.IncomeAfterTax;
                    break;
                case nameof(TaxCalculationResult.TotalDeductions):
                    propertyValue = result.TotalDeductions;
                    break;
                case nameof(TaxCalculationResult.UIFContribution):
                    propertyValue = result.UIFContribution;
                    break;
                case nameof(TaxCalculationResult.GrossIncome):
                    propertyValue = result.GrossIncome;
                    break;
                case nameof(TaxCalculationResult.EffectiveTaxRate):
                    propertyValue = result.EffectiveTaxRate;
                    break;
                case nameof(TaxCalculationResult.TaxYear):
                    propertyValue = result.TaxYear;
                    break;
                default: throw new Microsoft.OpenApi.Exceptions.OpenApiException($"Property {SpecificProperty} not found");
            }
            return new JsonResult(propertyValue);
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}

