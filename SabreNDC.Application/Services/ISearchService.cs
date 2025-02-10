using SabreNDC.Application.Dtos;
using TripLover.AirCommonModels;

namespace SabreNDC.Application.Services
{
    public interface ISearchService
    {
        Task<BFMResponse> Search(ACMSearchReq searchRequest);
    }
}
