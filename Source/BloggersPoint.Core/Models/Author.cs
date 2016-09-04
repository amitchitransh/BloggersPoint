using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BloggersPoint.Core.Models
{
    [DataContract]
    public class Author
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "username")]
        public string UserName { get; set; }
        [DataMember(Name = "email")]
        public string EMail { get; set; }
        [DataMember(Name = "phone")]
        public string Phone { get; set; }
        [DataMember(Name = "website")]
        public string Website { get; set; }    
    }

    [Serializable, XmlRoot("Authors"), XmlType("Authors")]
    public class AuthorList : List<Author>
    {

    }
}
