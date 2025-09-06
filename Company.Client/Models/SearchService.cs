// In Services/SearchService.cs
namespace Company.Client.Services
{
    public class SearchService
    {
        public event Action? OnSearch;

        public void NotifySearch()
        {
            OnSearch?.Invoke();
        }
    }
}
