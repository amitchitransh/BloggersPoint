using BloggersPoint.Core.Models;
using BloggersPoint.UI.ViewModel;
using NUnit.Framework;
using BloggersPoint.Core.Services;
using BloggersPoint.UI.Tests.Mock;
using BloggersPoint.UI.Services;
using Moq;

namespace BloggersPoint.UI.Tests.ViewModels
{
    [TestFixture]
    public class PostViewModelTest
    {
        private PostViewModel _postViewModel = null;
        private IBloggersPointService _fakeBloggersPointService;
        private bool _isBusyPropertyChangedToTrue;

        private readonly Post _post = new Post
        {
            PostId = 1,
            UserId = 1,
            Title = "test title",
            Body = "test body"
        };

        [SetUp]
        public void Setup()
        {
            _fakeBloggersPointService = new FakeBloggersPointService();
            
            _isBusyPropertyChangedToTrue = false;
        }

        [TestCase]
        public void PostSelectionAndAdditionalDataObjectsTest()
        {
            var messageService = new Mock<IMessageService>().Object;
            _postViewModel = new PostViewModel(_post, _fakeBloggersPointService, messageService);

            while (_postViewModel.IsBusy)
            {
                //wait for asynchronus call to complete and set flag for IsBusy property set to true
                //to assert later
                if (!_isBusyPropertyChangedToTrue)
                    _isBusyPropertyChangedToTrue = true;
            }

            Assert.NotNull(_postViewModel.Author);
            Assert.NotNull(_postViewModel.Comments);
            Assert.IsFalse(_postViewModel.IsBusy);
            Assert.AreEqual(true, _isBusyPropertyChangedToTrue, "IsBusy property should be true while fetching posts.");
        }

        [TestCase]
        public void ExceptionWhileFetchingDataForAdditionalDataObjectsTest()
        {
            const string action = "No response recieved from server. Please check network connectivity otherwise try again later.";
            var messageService = new Mock<IMessageService>();
            messageService.Setup(a => a.ShowErrorMessage(action));
            _postViewModel = new PostViewModel(_post, null, messageService.Object);
            messageService.Verify(v => v.ShowErrorMessage(It.IsAny<string>()));
            Assert.IsFalse(_postViewModel.IsBusy);
        }

        [TearDown]
        public void TearDown()
        {
            _postViewModel = null;
        }
    }
}
