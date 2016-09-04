using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BloggersPoint.Core.Models
{
    [DataContract]
    public class Comments
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

    [Serializable, XmlRoot("CommentsCollection"), XmlType("CommentsCollection")]
    public class CommentsCollection : ObservableCollection<Comments>
    { }
}