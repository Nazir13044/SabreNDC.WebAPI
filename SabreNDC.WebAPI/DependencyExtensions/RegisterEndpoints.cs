using SabreNDC.WebAPI.Apis.Search;

namespace SabreNDC.WebAPI.DependencyExtensions
{
    public static class RegisterEndpoints
    {
        public static void MapAllEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.SearchEndpoint();
        }
    }
}
