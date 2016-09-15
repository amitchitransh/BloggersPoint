using System.Threading.Tasks;
using NUnit.Framework;
using BloggersPoint.Core.Services;
using BloggersPoint.UI.Tests.Mock;
using BloggersPoint.UI.ViewModel;
using System.ComponentModel;
using BloggersPoint.UI.Services;
using Moq;

namespace BloggersPoint.UI.Tests.ViewModels
{
    public class PostListViewModelTest
    {
        private PostListViewModel _postListViewModel;
        private bool _isBusyPropertyChangedToTrue;

        [SetUp]
        public void Setup()
        {
            IBloggersPointService fakeBloggersPointService = new FakeBloggersPointService();
            var messageService = new Mock<IMessageService>().Object;
            _postListViewModel = new PostListViewModel(fakeBloggersPointService, messageService);
            _postListViewModel.PropertyChanged -= OnPropertyChanged;
            _postListViewModel.PropertyChanged += OnPropertyChanged;
            _isBusyPropertyChangedToTrue = false;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if ((args.PropertyName == "IsBusy") && (((PostListViewModel) sender).IsBusy))
                _isBusyPropertyChangedToTrue = true;
        }

        [TestCase]
        public async Task GetAllPostsTest()
        {
            Assert.IsFalse(_postListViewModel.IsBusy, "IsBusy property should be false before fetching the post data.");
            _postListViewModel.PostList = await _postListViewModel.GetAllPosts();
            Assert.NotNull(_postListViewModel.PostList, "No posts found");
            Assert.AreEqual(_isBusyPropertyChangedToTrue, true, "IsBusy property should be true while fetching posts.");
            Assert.IsFalse(_postListViewModel.IsBusy, "IsBusy property should be false after fetching the post data.");
        }

        [TearDown]
        public void TearDown()
        {
            _postListViewModel = null;
        }
    }
}
