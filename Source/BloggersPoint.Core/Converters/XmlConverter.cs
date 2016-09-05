using BloggersPoint.Core.Helper;
using BloggersPoint.Core.Models;
using NLog;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace BloggersPoint.Core.Converters
{
    public class XmlConverter: IObjectConverter
    {
        private const string ErrorJsonToXmlConversion = "Error while converting from JSON to XML";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ConversionResult Convert<T>(T dataObject)
        {
            var conversionResult = new ConversionResult { ConversionResultStatus = ConversionResultStatus.Ok };

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var stringWriter = new StringWriterUtf8Encoding())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        xmlSerializer.Serialize(writer, dataObject);
                        conversionResult.ResultString = stringWriter.ToString();

                    }
                    stringWriter.Close();
                }

            }
            catch (Exception exception)
            {
                conversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                conversionResult.ResultString = ErrorJsonToXmlConversion;
                Log.Error(exception);
            }
            return conversionResult;
        }
    }
}
