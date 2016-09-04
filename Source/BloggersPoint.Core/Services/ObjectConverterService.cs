using BloggersPoint.Core.Helper;
using BloggersPoint.Core.Models;
using BloggersPoint.Core.Properties;
using NLog;
using System;
using System.CodeDom;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace BloggersPoint.Core.Services
{
    public static class ObjectConverterService
    {
        private static readonly ConversionResult ConversionResult = new ConversionResult();
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static ConversionResult ToJson<T>(T dataObject)
        {
            ConversionResult.ConversionResultStatus = ConversionResultStatus.Ok;

            try
            {
                var j = new System.Web.Script.Serialization.JavaScriptSerializer();
                ConversionResult.ResultString = j.Serialize(dataObject);
            }
            catch (Exception exception)
            {
                ConversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                ConversionResult.ResultString = $"Error while converting from post object to JSON";
                Log.Error(exception);
            }
            return ConversionResult;
        }

        public static ConversionResult ToXml<T>(T dataObject)
        {
            ConversionResult.ConversionResultStatus = ConversionResultStatus.Ok;

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var stringWriter = new StringWriterUtf8Encoding())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        xmlSerializer.Serialize(writer, dataObject);
                        ConversionResult.ResultString = stringWriter.ToString();

                    }
                    stringWriter.Close();
                }

            }
            catch (Exception exception)
            {
                ConversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                ConversionResult.ResultString = $"Error while converting from JSON to XML";
                Log.Error(exception);
            }
            return ConversionResult;
        }

        public static ConversionResult ToHtml<T>(T dataObject)
        {
            var results = new StringWriter();
            var xmlString = ToXml<T>(dataObject).ResultString;
            ConversionResult.ConversionResultStatus = ConversionResultStatus.Ok;

            try
            {
                var compiledTransform = new XslCompiledTransform();
                using (XmlReader xmlreader = XmlReader.Create(new StringReader(Resources.Post)), xsltreader = XmlReader.Create(new StringReader(xmlString)))
                {
                    compiledTransform.Load(xmlreader);
                    compiledTransform.Transform(xsltreader, null, results);
                    ConversionResult.ResultString = results.ToString();
                }
            }
            catch(Exception exception)
            {
                ConversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                ConversionResult.ResultString = "Error while converting from object to HTML";
                Log.Error(exception);
            }
            finally
            {
                results.Dispose();
            }
            return ConversionResult;
        }

        public static ConversionResult ToPlainText<T>(T dataObject)
        {
            ConversionResult.ConversionResultStatus = ConversionResultStatus.Ok;

            try
            {
                    ConversionResult.ResultString = dataObject.ToString();                
            }
            catch (Exception exception)
            {
                ConversionResult.ConversionResultStatus = ConversionResultStatus.Failed;
                ConversionResult.ResultString = "Error while converting from object to Plain Text";
                Log.Error(exception);
            }
            return ConversionResult;
        }

    }
}
