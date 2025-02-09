using SabreNDC.Application.Services;

namespace SabreNDC.Service.Services
{
    public class SearchService : ISearchService
    {
        public async Task<string> Search(string searchText)
        {
            return await Task.FromResult(searchText);
        }
    }
}
