using System.Threading.Tasks;

namespace BloggersPoint.Core.Services
{
    public sealed class BloggersPointService: IBloggersPointService
    {
        public async Task<T> RunGetJsonDataTask<T>(string jsonResource)
        {
            return await Task.Run(() => BloggersPointServiceProvider.RequestDataFromServer<T>(jsonResource));
        }

        public async Task<T> RunGetJsonDataUsingIdTask<T>(string jsonResource, string idField, string id)
        {
            return await Task.Run(() => BloggersPointServiceProvider.RequestDataFromServer<T>(jsonResource, idField, id));
        }
    }
}
