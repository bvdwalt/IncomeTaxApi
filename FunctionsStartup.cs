using bvdwalt.IncomeTax.Common;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(bvdwalt.IncomeTax.Startup))]

namespace bvdwalt.IncomeTax
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITaxCalculatorService>((s) => {
                return new TaxCalculatorService();
            });
        }
    }
}
