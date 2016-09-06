using BloggersPoint.Core.Models;
using NUnit.Framework;
using BloggersPoint.Core.Converters;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BloggersPoint.Core.UnitTests.Converters
{
    [TestFixture]
    public class JsonConverterTest
    {
        private Post _post = null;
        private IObjectConverter _objectConverter = null;
        private string _objectAsJsonString = string.Empty;

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

            _objectConverter = new JsonConverter();
            _objectAsJsonString = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\TestData\\Post.json", Encoding.UTF8);
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
        public void ObjectToJsonConversionTest()
        {
            var conversionResult = _objectConverter.Convert(_post);
            Assert.AreEqual(conversionResult.ConversionResultStatus, ConversionResultStatus.Ok);
            Assert.AreEqual(Regex.Replace(conversionResult.ResultString, @"\""", ""), _objectAsJsonString);
        }
    }
}
