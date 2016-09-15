using System;
using System.Threading.Tasks;
using BloggersPoint.Core.Models;
using BloggersPoint.Core.Services;

namespace BloggersPoint.UI.Tests.Mock
{

    public class FakeBloggersPointService: IBloggersPointService
    {
        
        private readonly Post _post = new Post
        {
            PostId = 1,
            UserId = 1,
            Title = "test title",
            Body = "test body"
        };

        private readonly Author _author = new Author
        {
            Id = 1,
            EMail = "test@test.com",
            Name = "test user",
            UserName = "testuser",
            Website = "www.test.com",
            Phone = "022453245"
        };

        private readonly Comment _comment = new Comment
        {
            Id = 1,
            PostId = 1,
            EMail = "test@test.com",
            Name = "testuser1",
            Body = "test comment"
        };

        private readonly CommentCollection _comments = new CommentCollection();
        private readonly PostCollection _posts = new PostCollection();
        private readonly AuthorList _authors = new AuthorList();

        public FakeBloggersPointService()
        {
            _posts.Add(_post);
            _comments.Add(_comment);
            _authors.Add(_author);
        }

        public async Task<T> RunGetJsonDataTask<T>(string jsonResource)
        {
            return await Task.Run(() => RequestDataFromServer<T>(jsonResource));
        }

        public async Task<T> RunGetJsonDataUsingIdTask<T>(string jsonResource, string idField, string id)
        {
            return await Task.Run(() => RequestDataFromServer<T>(jsonResource, idField, id));
        }

        public T RequestDataFromServer<T>(string resourceName)
        {   
            return (T)Convert.ChangeType(_posts, typeof(T));
        }

        public T RequestDataFromServer<T>(string resourceName, string idField, string id)
        {
            object dataObject = null;

            switch (typeof(T).Name)
            {
                case "AuthorList":
                    dataObject = Convert.ChangeType(_authors, typeof(T));
                    break;
                case "CommentCollection":
                    dataObject = Convert.ChangeType(_comments, typeof(T));
                    break;
            }
            return (T)dataObject;
        }
    }
}
