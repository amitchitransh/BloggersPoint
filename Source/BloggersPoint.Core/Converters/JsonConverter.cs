using BloggersPoint.Core.Models;
using System;
using NLog;

namespace BloggersPoint.Core.Converters
{
    public class JsonConverter : IObjectConverter
    {
        private const string ErrorObjectToJsonConversion = "Error while converting from post object to JSON";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ConversionResult Convert<T>(T dataObject)
        {
            var conversionResult = new ConversionResult {ConversionResultStatus = ConversionResultStatus.Ok};

            try
            {
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                conversionResult.ResultString = javaScriptSerializer.Serialize(dataObject);
            }
            catch (Exception exception)
            {
                conversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                conversionResult.ResultString = ErrorObjectToJsonConversion;
                Log.Error(exception);
            }
            return conversionResult;
        }
    }
}
