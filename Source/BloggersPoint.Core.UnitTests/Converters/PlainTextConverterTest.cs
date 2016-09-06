using BloggersPoint.Core.Models;
using NUnit.Framework;
using BloggersPoint.Core.Converters;
using System;
using System.Text.RegularExpressions;
using System.Text;

namespace BloggersPoint.Core.UnitTests.Converters
{
    [TestFixture]
    public class PlainTextConverterTest
    {
        private Post _post = null;
        private IObjectConverter _objectConverter = null;
        private string _objectAsPlainTextString = string.Empty;

         [SetUp]
        public void Setup()
        {
            _post = new Post
            {
                PostId = 1,
                UserId = 1,
                Title = "test title",
                Body = "test body",
                Author = new Author
                {
                    Id = 1,
                    Name = "test name",
                    EMail = "test@test.com",
                    Phone = "0120-2345678",
                    UserName = "testuser",
                    Website = "www.test.com"
                },
                Comments = new CommentCollection
                {
                    new Comment {Body = "test comment Body", PostId = 1, EMail = "test1@test.com", Name = "test name", Id = 1}
                }
            };

            _objectConverter = new PlainTextConverter();
            _objectAsPlainTextString = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\TestData\\Post.txt", Encoding.UTF8);
        }

        [TearDown]
        public void TearDown()
        {
            _post = null;
        }

        [TestCase]
        public void PostObjectNotNullTest()
        {
            Assert.IsNotNull(_post);
        }

        [TestCase]
        public void ObjectToPlainTextConversionOkTest()
        {
            var conversionResult = _objectConverter.Convert(_post);
            Assert.AreEqual(conversionResult.ConversionResultStatus, ConversionResultStatus.Ok);
            Assert.AreEqual(Regex.Replace(Regex.Replace(conversionResult.ResultString, @"\r\n?|\n", ""), @"\s+", ""),
                _objectAsPlainTextString);
        }

        [TestCase]
        public void ObjectToPlainTextConversionFailTest()
        {
            _post = null;
            var conversionResult = _objectConverter.Convert(_post);
            Assert.AreEqual(conversionResult.ConversionResultStatus, ConversionResultStatus.Failed);
        }
    }
}
