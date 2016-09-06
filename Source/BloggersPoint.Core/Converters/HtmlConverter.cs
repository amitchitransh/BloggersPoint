using BloggersPoint.Core.Models;
using BloggersPoint.Core.Properties;
using NLog;
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace BloggersPoint.Core.Converters
{
    public class HtmlConverter : IObjectConverter
    {
        private const string ErrorObjectToHtmlConversion = "Error while converting from object to HTML";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ConversionResult Convert<T>(T dataObject)
        {
            var results = new StringWriter();
            IObjectConverter objectConverter = new XmlConverter();
            var conversionResult = objectConverter.Convert<T>(dataObject);

            if (conversionResult.ConversionResultStatus == ConversionResultStatus.Failed)
                return conversionResult;

            try
            {
                var compiledTransform = new XslCompiledTransform();
                using (
                    XmlReader xmlreader = XmlReader.Create(new StringReader(Resources.Post)),
                        xsltreader = XmlReader.Create(new StringReader(conversionResult.ResultString)))
                {
                    compiledTransform.Load(xmlreader);
                    compiledTransform.Transform(xsltreader, null, results);
                    conversionResult.ResultString = results.ToString();
                }
            }
            catch (Exception exception)
            {
                conversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                conversionResult.ResultString = ErrorObjectToHtmlConversion;
                Log.Error(exception);
            }
            finally
            {
                results.Dispose();
            }
            return conversionResult;
        }
    }
}
