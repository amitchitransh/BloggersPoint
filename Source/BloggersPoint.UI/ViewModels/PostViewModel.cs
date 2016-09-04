using BloggersPoint.UI.Common;
using BloggersPoint.Core.Models;
using System.Threading.Tasks;
using BloggersPoint.Core.Services;
using System;
using BloggersPoint.Services;
using BloggersPoint.Properties;
using NLog;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BloggersPoint.UI.ViewModel
{
    public class PostViewModel : ViewModelBase
    {
        private Author _author;
        private bool _isBusy;
        private Post _post;
        private ICommand _copyJsonCommand;
        private ICommand _copyPlainTextCommand;
        private ICommand _copyHtmlCommand;
        private CommentsCollection _comments;
        private readonly IBloggersPointService _bloggersPointService = new BloggersPointService();
        private readonly IMesaageService _messageService = new MessageService();
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private const string AuthorDataResource = "users";
        private const string CommentsDataResource = "comments";
        private const string IdField = "id";
        private const string PostIdField = "postId";

        private const string AuthorProperty = "Author";
        private const string CommentsProperty = "Comments";
        private const string IsBusyProperty = "IsBusy";
        private const string PostProperty = "Post";

        public Post Post
        {
            get { return _post; }
            set
            {
                if (_post == value)
                    return;

                _post = value;
                OnPropertyChanged(PostProperty);
            }
        }

        public Author Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                OnPropertyChanged(AuthorProperty);
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

        public CommentsCollection Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
                OnPropertyChanged(CommentsProperty);
            }
        }

        public ICommand CopyJsonCommand
        {
            get
            {
                if (_copyJsonCommand != null)
                    return _copyJsonCommand;

                _copyJsonCommand = new RelayCommand(i => ConvertObject(ConversionOption.Json), null);
                return _copyJsonCommand;
            }
        }

        public ICommand CopyPlainTextCommand
        {
            get
            {
                if (_copyPlainTextCommand != null)
                    return _copyPlainTextCommand;

                _copyPlainTextCommand = new RelayCommand(i => ConvertObject(ConversionOption.PlainText), null);
                return _copyPlainTextCommand;
            }
        }

        public ICommand CopyHtmlCommand
        {
            get
            {
                if (_copyHtmlCommand != null)
                    return _copyHtmlCommand;

                _copyHtmlCommand = new RelayCommand(i => ConvertObject(ConversionOption.Html), null);
                return _copyHtmlCommand;
            }
        }

        private void ConvertObject(ConversionOption conversionOption)
        {
            ConversionResult converionResult = null;

            PrepareAdditionalObjects();

            switch (conversionOption)
            {
                case ConversionOption.Json:
                    converionResult = ObjectConverterService.ToJson(Post);
                    break;
                case ConversionOption.Html:
                    converionResult = ObjectConverterService.ToHtml(Post);
                    break;
                case ConversionOption.PlainText:
                    converionResult = ObjectConverterService.ToPlainText(Post);
                    break;
                default:
                    converionResult = ObjectConverterService.ToPlainText(Post);
                    break;
            }
            
            if (converionResult == null)
                throw new ArgumentNullException(nameof(converionResult), Resources.InvalidConversionResult);

            if (converionResult.ConversionResultStatus == ConversionResultStatus.Failed)
                _messageService.ShowErrorMessage(converionResult.ResultString);

            Clipboard.SetText(converionResult.ResultString);
        }

        private void PrepareAdditionalObjects()
        {
            if (Post.Author == null)
                Post.Author = Author;
            if (Post.Comments == null)
                Post.Comments = Comments;
        }

        public PostViewModel(Post post)
        {
            PropertyChanged -= OnPropertyChanged;
            PropertyChanged += OnPropertyChanged;

            Post = post;
            
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != PostProperty)
                return;
            GetPostAuthor();
            GetPostComments();
        }

        private async void GetPostAuthor()
        {
            IsBusy = true;
            Author = await GetAuthor();
        }

        private async void GetPostComments()
        {
            if (!IsBusy)
                IsBusy = true;

            Comments = await GetComments();
            IsBusy = false;
        }

        private async Task<Author> GetAuthor()
        {
            AuthorList authorList = null;

            try
            {
                authorList = await _bloggersPointService.RunGetJsonDataUsingIdTask<AuthorList>(AuthorDataResource, IdField, Post.UserId.ToString());
            }
            catch (Exception exception)
            {
                _messageService.ShowErrorMessage(Resources.ConnectivityErrorMessage);
                Log.Error(exception);
            }

            return authorList?[0];
        }

        private async Task<CommentsCollection> GetComments()
        {
            try
            {
                _comments = await _bloggersPointService.RunGetJsonDataUsingIdTask<CommentsCollection>(CommentsDataResource, PostIdField, Post.PostId.ToString());
            }
            catch (Exception exception)
            {
                _messageService.ShowErrorMessage(Resources.ConnectivityErrorMessage);
                Log.Error(exception);
            }
            return _comments;
        }
    }
}
