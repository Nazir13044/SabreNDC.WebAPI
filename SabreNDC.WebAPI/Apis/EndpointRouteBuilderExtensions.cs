namespace ApiGateway.Apis
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapAuthorizedGroup(this IEndpointRouteBuilder routes, string pattern)
        {
            return routes.MapGroup(pattern).RequireAuthorization();
        }

        public static IEndpointRouteBuilder MapAnonymouseGroup(this IEndpointRouteBuilder routes, string pattern)
        {
            return routes.MapGroup(pattern);
        }
    }
}
