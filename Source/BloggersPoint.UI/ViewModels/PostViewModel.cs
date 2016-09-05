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
using BloggersPoint.Core.Converters;

namespace BloggersPoint.UI.ViewModel
{
    public class PostViewModel : ViewModelBase
    {
        private Author _author;
        private bool _isBusy;
        private Post _post;
        private string _copyResultMessage;
        private ICommand _copyJsonCommand;
        private ICommand _copyPlainTextCommand;
        private ICommand _copyHtmlCommand;
        private CommentCollection _comments;
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
        private const string CopyResultMessageProperty = "CopyResultMessage";

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

        public string CopyResultMessage
        {
            get
            {
                return _copyResultMessage;
            }

            set
            {
                _copyResultMessage = value;
                OnPropertyChanged(CopyResultMessageProperty);
            }

        }
        public CommentCollection Comments
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
            CopyResultMessage = string.Empty;

            PrepareAdditionalObjects();

            IObjectConverter objectConverter = null;

            switch (conversionOption)
            {
                case ConversionOption.Json:
                    objectConverter = new JsonConverter();
                    break;
                case ConversionOption.Html:
                    objectConverter = new HtmlConverter();
                    break;
                case ConversionOption.PlainText:
                    objectConverter = new PlainTextConverter();
                    break;
                default:
                    objectConverter = new PlainTextConverter();
                    break;
            }
            objectConverter.Convert(Post);
            Clipboard.SetText(objectConverter.Convert(Post).ResultString);

            CopyResultMessage = $"{conversionOption} data copied to clipboard.";
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
            CopyResultMessage = string.Empty;
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

        private async Task<CommentCollection> GetComments()
        {
            try
            {
                _comments = await _bloggersPointService.RunGetJsonDataUsingIdTask<CommentCollection>(CommentsDataResource, PostIdField, Post.PostId.ToString());
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
