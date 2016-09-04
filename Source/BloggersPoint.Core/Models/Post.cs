using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BloggersPoint.Core.Models
{
    [DataContract]
    public class Post
    {
        [DataMember(Name = "userId")]
        public int UserId { get; set; }
        [DataMember(Name = "id")]
        public int PostId { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "body")]
        public string Body { get; set; }
        [DataMember(Name = "Author")]
        public Author Author{ get; set; }
        [DataMember(Name = "Commnts")]
        public CommentsCollection Comments { get; set; }

        public override string ToString()
        {
            var comments = CommentsToString();

            return
                $"Author{Environment.NewLine}UserName:{Author.UserName}{Environment.NewLine}E-Mail{Author.EMail}{Environment.NewLine}{Author.Phone}{Environment.NewLine}{Author.Website}{Environment.NewLine}{Environment.NewLine}" +
                $"Post{Environment.NewLine}Title:{Title}{Environment.NewLine}Body:{Body}{Environment.NewLine}{Environment.NewLine}" + comments;
        }

        private string CommentsToString()
        {
            var comments = $"Comments{Environment.NewLine}";

            return Comments == null ? comments : Comments.Aggregate(comments, (current, comment) => current + $"Name:{comment.Name}{Environment.NewLine}E-Mail:{comment.EMail}{Environment.NewLine}Body:{comment.Body}{Environment.NewLine}{Environment.NewLine}");
        }
    }


    [Serializable, XmlRoot("PostCollection"), XmlType("PostCollection")]
    public class PostCollection : ObservableCollection<Post>
    { }
}
