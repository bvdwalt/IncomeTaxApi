using IncomeTaxApi.Common;
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
    public static class IncomeTax
    {
        [FunctionName("IncomeTaxPerAnnum")]
        [OpenApiOperation(operationId: "IncomeTaxPerAnnum", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "GrossIncome", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Your per annum income before tax gets deducted")]
        [OpenApiParameter(name: "TaxYear", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The Tax Year this income is for")]
        [OpenApiParameter(name: "Age", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Your Age during this tax year")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> IncomeTaxPerAnnum(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            double GrossIncome = double.Parse(req.Query["GrossIncome"]);
            int TaxYear = int.Parse(req.Query["TaxYear"]);
            int Age = int.Parse(req.Query["Age"]);

            TaxCaculator taxCalculator = new TaxCaculator();

            TaxCalculationResult result = await taxCalculator.CalculateIncomeTax(GrossIncome, Age, TaxYear);

            if (result == null) return new NotFoundResult();

            return new OkObjectResult(result);
        }

        [FunctionName("IncomeTaxPerMonth")]
        [OpenApiOperation(operationId: "IncomeTaxPerMonth", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "GrossIncome", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Your per month income before tax gets deducted")]
        [OpenApiParameter(name: "TaxYear", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The Tax Year this income is for")]
        [OpenApiParameter(name: "Age", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Your Age during this tax year")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> IncomeTaxPerMonth(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            double GrossIncome = double.Parse(req.Query["GrossIncome"]);
            int TaxYear = int.Parse(req.Query["TaxYear"]);
            int Age = int.Parse(req.Query["Age"]);

            TaxCaculator taxCalculator = new TaxCaculator();

            TaxCalculationResult result = await taxCalculator.CalculateIncomeTaxPerMonth(GrossIncome, Age, TaxYear);

            if (result == null) return new NotFoundResult();

            return new OkObjectResult(result);
        }
    }
}

