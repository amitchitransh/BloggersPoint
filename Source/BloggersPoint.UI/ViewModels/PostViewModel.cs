using BloggersPoint.UI.Common;
using BloggersPoint.Core.Models;
using System.Threading.Tasks;
using BloggersPoint.Core.Services;
using System;
using BloggersPoint.UI.Services;
using BloggersPoint.Properties;
using NLog;
using System.ComponentModel;
using System.IO;
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
        private string _htmlSourcePath;
        private string _copyResultMessage;
        private ICommand _copyHtmlCommand;
        private ICommand _copyPlainTextCommand;
        private ICommand _copyJsonCommand;
        private int _selectedIndex;
        private string _objectAsPlainText;
        private string _objectAsHtml;
        private string _objectAsJson;
        private CommentCollection _comments;
        private readonly IBloggersPointService _bloggersPointService;
        public readonly IMessageService MessageService;
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
        private const string HtmlSourcePathProperty = "HtmlSourcePath";
        private const string ObjectAsHtmlProperty = "ObjectAsHtml";
        private const string ObjectAsPlainTextProperty = "ObjectAsPlainText";
        private const string ObjectAsJsonProperty = "ObjectAsJson";
        private const string SelectedIndexProperty = "SelectedIndex";

        private const string HtmlCopiedSuccessfullyMessage = "Html data copied to clipboard.";
        private const string PlainTextCopiedSuccessfullyMessage = "Plain Text data copied to clipboard.";
        private const string JsonCopiedSuccessfullyMessage = "Json data copied to clipboard.";

        private const int JsonTabIndex = 1;
        private const int HtmlTabIndex = 2;
        private const int PlainTextIndex = 3;

        private static readonly string ConvertedFileTemporaryLocation =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\";

        private static readonly string HtmlFileProtocol = "file://";

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

        public string ObjectAsHtml
        {
            get { return _objectAsHtml; }
            set
            {
                _objectAsHtml = value;
                OnPropertyChanged(ObjectAsHtmlProperty);
            }
        }

        public string ObjectAsPlainText
        {
            get { return _objectAsPlainText; }
            set
            {
                _objectAsPlainText = value;
                OnPropertyChanged(ObjectAsPlainTextProperty);
            }
        }

        public string ObjectAsJson
        {
            get { return _objectAsJson; }
            set
            {
                _objectAsJson = value;
                OnPropertyChanged(ObjectAsJsonProperty);
            }
        }

        public ICommand CopyHtmlCommand
        {
            get
            {
                if (_copyHtmlCommand != null)
                    return _copyHtmlCommand;

                _copyHtmlCommand = new RelayCommand(i => CopyAsHtml(), null);
                return _copyHtmlCommand;
            }
        }

        public ICommand CopyPlainTextCommand
        {
            get
            {
                if (_copyPlainTextCommand != null)
                    return _copyPlainTextCommand;

                _copyPlainTextCommand = new RelayCommand(i => CopyAsPlainText(), null);
                return _copyPlainTextCommand;
            }
        }

        public ICommand CopyJsonCommand
        {
            get
            {
                if (_copyJsonCommand != null)
                    return _copyJsonCommand;

                _copyJsonCommand = new RelayCommand(i => CopyAsJson(), null);
                return _copyJsonCommand;
            }
        }

        public string HtmlSourcePath
        {
            get
            {
                return _htmlSourcePath;
            }

            set
            {
                _htmlSourcePath = value;
                OnPropertyChanged(HtmlSourcePathProperty);
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
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                PopulateTab(_selectedIndex);
                OnPropertyChanged(SelectedIndexProperty);
            }
        }

        private void CopyAsHtml()
        {
            Clipboard.SetText(ObjectAsHtml);
            CopyResultMessage = HtmlCopiedSuccessfullyMessage;
        }

        private void CopyAsPlainText()
        {
            Clipboard.SetText(ObjectAsPlainText);
            CopyResultMessage = PlainTextCopiedSuccessfullyMessage;
        }

        private void CopyAsJson()
        {
            Clipboard.SetText(ObjectAsJson);
            CopyResultMessage = JsonCopiedSuccessfullyMessage;
        }

        private ConversionResult ConvertObject(ConversionOption conversionOption)
        {
            CopyResultMessage = string.Empty;

            PrepareAdditionalObjects();

            IObjectConverter objectConverter;

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

            return objectConverter.Convert(Post);
        }

        private void PrepareAdditionalObjects()
        {
            if (Post.Author == null)
                Post.Author = Author;
            if (Post.Comments == null)
                Post.Comments = Comments;
        }

        public PostViewModel(Post post, IBloggersPointService bloggersPointService, IMessageService messageService)
        {
            PropertyChanged -= OnPropertyChanged;
            PropertyChanged += OnPropertyChanged;
            _bloggersPointService = bloggersPointService;
            MessageService = messageService;

            Post = post;
            CopyResultMessage = string.Empty;
        }

        private void PopulateTab(int selectedTabIndex)
        {
            CopyResultMessage = string.Empty;
            switch (selectedTabIndex)
            {
                case JsonTabIndex:
                    PopulateJsonTab();
                    break;
                case HtmlTabIndex:
                    PopulateHtmlTab();
                    break;
                case PlainTextIndex:
                    PopulatePlainTextTab();
                    break;
            }
            
        }

        private void PopulateHtmlTab()
        {
            var conversionResult = ConvertObject(ConversionOption.Html); 

            if (conversionResult.ConversionResultStatus == ConversionResultStatus.Failed)
                return;

            ObjectAsHtml = conversionResult.ResultString;
            var conversionTemporaryLocation = ConvertedFileTemporaryLocation + Post.PostId + ".html";
            using (var streamWriter = new StreamWriter(conversionTemporaryLocation))
            {
                streamWriter.WriteLine(ObjectAsHtml);
                streamWriter.Close();
            }
            HtmlSourcePath = HtmlFileProtocol + conversionTemporaryLocation;
        }

        private void PopulateJsonTab()
        {
            var conversionResult = ConvertObject(ConversionOption.Json);

            if (conversionResult.ConversionResultStatus == ConversionResultStatus.Failed)
                return;

            ObjectAsJson = conversionResult.ResultString;
        }

        private void PopulatePlainTextTab()
        {
            var conversionResult = ConvertObject(ConversionOption.PlainText);

            if (conversionResult.ConversionResultStatus == ConversionResultStatus.Failed)
                return;

            ObjectAsPlainText = conversionResult.ResultString;
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
                MessageService.ShowErrorMessage(Resources.ConnectivityErrorMessage);
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
                MessageService.ShowErrorMessage(Resources.ConnectivityErrorMessage);
                Log.Error(exception);
            }
            return _comments;
        }
    }
}

