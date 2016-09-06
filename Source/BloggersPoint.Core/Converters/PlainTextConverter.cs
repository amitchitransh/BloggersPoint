using BloggersPoint.Core.Models;
using NLog;
using System;

namespace BloggersPoint.Core.Converters
{
    public class PlainTextConverter: IObjectConverter
    {
        private const string ErrorObjectToPlainTextConversion = "Error while converting from object to Plain Text";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ConversionResult Convert<T>(T dataObject)
        {
            var conversionResult = new ConversionResult { ConversionResultStatus = ConversionResultStatus.Ok };

            try
            {
                conversionResult.ResultString = dataObject.ToString();
            }
            catch (Exception exception)
            {
                conversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                conversionResult.ResultString = ErrorObjectToPlainTextConversion;
                Log.Error(exception);
            }
            return conversionResult;
        }
    }
}
