namespace SabreNDC.Application.Services
{
    public interface ISearchService
    {
        Task<string> Search(string searchText);
    }
}
