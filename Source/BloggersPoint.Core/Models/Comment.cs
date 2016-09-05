using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BloggersPoint.Core.Models
{
    [DataContract]
    public class Comment
    {
        [DataMember(Name = "postId")]
        public int PostId { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "email")]
        public string EMail { get; set; }
        [DataMember(Name = "body")]
        public string Body { get; set; }
    }

    [Serializable, XmlRoot("CommentCollection"), XmlType("CommentCollection")]
    public class CommentCollection : ObservableCollection<Comment>
    {
        public override string ToString()
        {
            var comments = $"Comments{Environment.NewLine}";

            return this.Aggregate(comments, (current, comment) => current + $"Name:{comment.Name}{Environment.NewLine}E-Mail:{comment.EMail}{Environment.NewLine}Body:{comment.Body}{Environment.NewLine}{Environment.NewLine}");
        }
    }
}