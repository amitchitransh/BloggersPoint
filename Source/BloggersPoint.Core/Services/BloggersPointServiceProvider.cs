using BloggersPoint.Core.Properties;
using NLog;
using System;
using System.Net;
using System.Runtime.Serialization.Json;

namespace BloggersPoint.Core.Services
{
    public static class BloggersPointServiceProvider
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static T RequestDataFromServer<T>(string resourceName)
        {
            string requestUrl = $"{Resources.BloggersPointServiceUrl}{resourceName}";
            return RequestServer<T>(requestUrl);
        }

        public static T RequestDataFromServer<T>(string resourceName, string idField, string id)
        {
            var dataObject = CachingService.Current.GetData<T>(resourceName, id);

            if (dataObject != null)
                return dataObject;

            string requestUrl = $"{Resources.BloggersPointServiceUrl}{resourceName}?{idField}={id}";

            dataObject = RequestServer<T>(requestUrl);
            CachingService.Current.AddData<T>(dataObject, resourceName, id);
            return dataObject;
        }

        private static T RequestServer<T>(string requestUrl)
        {
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;

                if (request == null)
                    return default(T);

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response == null)
                        return default(T);

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(
                            $"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                    return (T) Convert.ChangeType(SerializeJsonResponse<T>(response), typeof(T));
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return default(T);
            }
        }

        private static object SerializeJsonResponse<T>(HttpWebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response), Resources.InvalidResponseRecievedForSerailization);

            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var stream = response.GetResponseStream();

            if (stream == null)
                throw new ArgumentNullException(nameof(stream), Resources.InvalidStream);

            return jsonSerializer.ReadObject(stream);
        }
    }
}
