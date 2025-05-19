using CalculoCDB.API.Models.CDB;
using CalculoCDB.API.Services.CDB;
using CalculoCDB.API.Settings;

namespace CalculoCDB.API
{
    public static class Bindings
    {
        public static IServiceCollection AddCDBServices(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.AddScoped<ICdbService, CdbService>(provider =>
            {
                return new CdbService
                (
                    configSection.Get<ConstantRates>()!,
                    new CdbValidator()
                );
            });

            return services;
        }
    }
}
