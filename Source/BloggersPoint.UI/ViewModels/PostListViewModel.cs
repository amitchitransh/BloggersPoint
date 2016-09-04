using BloggersPoint.UI.Common;
using BloggersPoint.Core.Services;
using System.Windows.Input;
using System;
using BloggersPoint.Services;
using BloggersPoint.Core.Models;
using System.Threading.Tasks;
using NLog;
using BloggersPoint.Properties;
using System.ComponentModel;

namespace BloggersPoint.UI.ViewModel
{
    public class PostListViewModel : ViewModelBase
    {
        private static PostListViewModel _instance = null;
        private PostViewModel _selectedPostViewModel = null;
        private Post _selectedPost = null;
        private bool _isBusy;
        private bool _isPostDetailVisible;
        private PostCollection _postList = null;
        private readonly IBloggersPointService _bloggersPointService = new BloggersPointService();
        private ICommand _loadedCommand;
        private readonly IMesaageService _messageService = new MessageService();
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private const string PostDataResource = "posts";
        private const string PostListProperty = "PostList";
        private const string SelectedPostProperty = "SelectedPost";
        private const string SelectedPostViewModelProperty = "SelectedPostViewModel";
        private const string IsPostDetailVisibleProperty = "IsPostDetailVisible";
        private const string IsBusyProperty = "IsBusy";

        public PostCollection PostList
        {
            get
            {
                return _postList;
            }
            set
            {
                _postList = value;
                OnPropertyChanged(PostListProperty);
            }
        }

        public Post SelectedPost
        {
            get
            {
                return _selectedPost;
            }
            set
            {
                _selectedPost = value;
                OnPropertyChanged(SelectedPostProperty);
            }
        }

        public PostViewModel SelectedPostViewModel
        {
            get
            {
                return _selectedPostViewModel;
            }
            set
            {
                _selectedPostViewModel = value;
                OnPropertyChanged(SelectedPostViewModelProperty);
            }
        }

        public bool IsPostDetailVisible
        {
            get
            {
                return _isPostDetailVisible;
            }

            set
            {
                _isPostDetailVisible = value;
                OnPropertyChanged(IsPostDetailVisibleProperty);
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged(IsBusyProperty);
            }

        }

        public PostListViewModel()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != SelectedPostProperty)
                return;

            if (SelectedPost != null)
            {
                IsPostDetailVisible = true;
                SelectedPostViewModel = new PostViewModel(SelectedPost);
            }
            else
                IsPostDetailVisible = false;
        }

        public static PostListViewModel Instance()
        {
            if (_instance != null)
                return _instance;

            _instance = new PostListViewModel();
            return _instance;
        }

        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand != null)
                    return _loadedCommand;

                _loadedCommand = new RelayCommand(i => ShowPosts(), null);
                return _loadedCommand;
            }
        }

        private async void ShowPosts()
        {
            IsBusy = true;
            PostList = await GetAllPosts();
            IsBusy = false;
        }

        private async Task<PostCollection> GetAllPosts()
        {
            PostCollection postData = null;

            try
            {
                postData = await _bloggersPointService.RunGetJsonDataTask<PostCollection>(PostDataResource);
            }
            catch (Exception exception)
            {
                _messageService.ShowErrorMessage(Resources.ConnectivityErrorMessage);
                _log.Error(exception);
            }
            return postData;
        }
    }
}
