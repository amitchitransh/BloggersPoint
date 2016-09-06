using System;
using System.Collections.ObjectModel;
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
        public Author Author { get; set; }

        [DataMember(Name = "Comments")]
        public CommentCollection Comments { get; set; }

        public override string ToString()
        {
            var postString = string.Empty;

            //try
            //{
                if (Author != null)
                    postString =
                        $"Author{Environment.NewLine}UserName:{Author.UserName}{Environment.NewLine}E-Mail{Author.EMail}{Environment.NewLine}{Author.Phone}{Environment.NewLine}{Author.Website}{Environment.NewLine}{Environment.NewLine}";

                postString = postString +
                             $"Post{Environment.NewLine}Title:{Title}{Environment.NewLine}Body:{Body}{Environment.NewLine}{Environment.NewLine}";

                if (Comments != null)
                    postString = postString + Comments;

            //    throw new ArgumentNullException();
            //}
            //catch (Exception exception)
            //{

            //    System.IO.File.WriteAllText(@"C:\users\public\error.txt", exception.InnerException.ToString()); 
            //}

            return postString;
        }

    }


    [Serializable, XmlRoot("PostCollection"), XmlType("PostCollection")]
    public class PostCollection : ObservableCollection<Post>
    { }
}
