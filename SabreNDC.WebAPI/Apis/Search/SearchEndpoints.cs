using SabreNDC.Application.Services;

namespace SabreNDC.WebAPI.Apis.Search
{
    public static class SearchEndpoints
    {
        public static void SearchEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/search", async (ISearchService _searchService) =>
            {
                return Results.Ok(new { Message = await _searchService.Search("testing") });
            })
            .WithGroupName("Search");
        }
    }
}
