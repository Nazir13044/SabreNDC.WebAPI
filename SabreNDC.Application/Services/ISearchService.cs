using TripLover.AirCommonModels;

namespace SabreNDC.Application.Services
{
    public interface ISearchService
    {
        Task<string> Search(ACMSearchReq searchRequest);
    }
}
