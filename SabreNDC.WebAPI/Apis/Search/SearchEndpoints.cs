using SabreNDC.Application.Services;
using TripLover.AirCommonModels;

namespace SabreNDC.WebAPI.Apis.Search
{
    public static class SearchEndpoints
    {
        public static void SearchEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/search", async (ISearchService _searchService, ACMSearchReq searchRequest) =>
            {
                return Results.Ok(new { Message = await _searchService.Search(searchRequest) });
            })
            .WithGroupName("Search");
        }
    }
}
