using SabreNDC.Application.Services;
using SabreNDC.Service.Services;

namespace SabreNDC.WebAPI.DependencyExtensions
{
    public static class RegisterServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISearchService, SearchService>();
        }
    }
}
