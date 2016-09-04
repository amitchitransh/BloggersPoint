using System;
using System.IO;
using System.Xml.Serialization;
using BloggersPoint.Core.Properties;

namespace BloggersPoint.Core.Services
{
    public sealed class CachingService
    {
        private static volatile CachingService _instance;
        private static readonly object InstanceSync = new object();
        private const string ApplicationFolderName = "\\BloggersPoint\\";
        private const string CacheFileExtension = ".cache";

        private static readonly string CacheFilePath =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + ApplicationFolderName;

        public static CachingService Current
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (InstanceSync)
                {
                    if (_instance == null)
                    {
                        _instance = new CachingService();
                    }
                }
                return _instance;
            }
        }

        public T GetData<T>(string resourceName, string id)
        {
            var dataObject = default(T);

            EnsureDirectoryExists();

            var resourceCacheLocation = CacheFilePath + resourceName + id + CacheFileExtension;

            if (!(IsCacheAvailable(resourceCacheLocation)))
                return dataObject;

            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(resourceCacheLocation))
            {
                dataObject = (T) xmlSerializer.Deserialize(streamReader);
                streamReader.Close();
            }
            return dataObject;
        }

        private static bool IsCacheAvailable(string resourceCacheLocation)
        {
            var isFileExists = File.Exists(resourceCacheLocation);
            return isFileExists;
        }

        private static void EnsureDirectoryExists()
        {
            var directoryExists = Directory.Exists(Path.GetDirectoryName(CacheFilePath));
            if (directoryExists)
                return;

            var directoryName = Path.GetDirectoryName(CacheFilePath);

            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName), Resources.InvalidDirectory);

            Directory.CreateDirectory(directoryName);
        }

        public void AddData<T>(T autoUpdateResponse, string resourceName, string id)
        {
            var resourceCacheLocation = CacheFilePath + resourceName + id + CacheFileExtension;
            using (var streamWriter = new StreamWriter(resourceCacheLocation))
            {
                streamWriter.WriteLine(ObjectConverterService.ToXml(autoUpdateResponse).ResultString);
                streamWriter.Close();
            }
        }
    }
}
