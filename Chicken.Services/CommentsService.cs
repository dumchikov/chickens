using System;
using System.Collections.Generic;
using System.Linq;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;

namespace Chicken.Services
{
    public class CommentsService
    {
        private readonly IRepository<Post> _posts;

        private readonly IRepository<Comment> _comments;

        public CommentsService(IRepository<Post> posts, IRepository<Comment> comments)
        {
            _posts = posts;
            _comments = comments;
        }

        public IEnumerable<Comment> GetAllComments()
        {
            var comments = _comments.Query();
            return comments;
        }

        public Comment GetComment(int id)
        {
            var comment = _comments.GetById(id);
            return comment;
        }

        public IEnumerable<Comment> GetComments(int id)
        {
            var post = _posts.GetById(id);
            var comments = post.Comments;

            if (!comments.Any())
            {
                var newComments = DownloadAllComments(post).Where(x => x.ProfileId > 0).ToList();
                comments = AddComments(newComments, post.Id);
            }

            return comments.OrderBy(x => x.Date);
        }

        private List<Comment> AddComments(List<Comment> comments, int postId)
        {
            var userIds = comments.Select(x => x.ProfileId).Distinct();
            var users = VkApi.GetUsers(userIds).ToList(); //TODO: fix retrieving of users
            foreach (var comment in comments)
            {
                var user = users.SingleOrDefault(x => x.ProfileId == comment.ProfileId);
                comment.User = user;
                comment.PostId = postId;
                _comments.Add(comment);
            }

            _comments.Save();
            return comments;
        }

        private static IEnumerable<Comment> DownloadAllComments(Post post)
        {
            int totalCommentsCount;
            var comments = VkApi.GetComments(100, 0, post, out totalCommentsCount).ToList();

            if (totalCommentsCount > 100)
            {
                var requestsNumber = Math.Ceiling((double)totalCommentsCount / 100);
                for (var i = 1; i < requestsNumber; i++)
                {
                    var commentsRes = VkApi.GetComments(100, i * 100, post, out totalCommentsCount);
                    comments.AddRange(commentsRes);
                }
            }

            comments = comments.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();
            return comments;
        }
    }
}
