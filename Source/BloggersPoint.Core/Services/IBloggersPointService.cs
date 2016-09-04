using System.Threading.Tasks;

namespace BloggersPoint.Core.Services
{
    public interface IBloggersPointService
    {
        Task<T> RunGetJsonDataTask<T>(string jsonResource);
        Task<T> RunGetJsonDataUsingIdTask<T>(string jsonResource, string idField, string id);
    }
}
