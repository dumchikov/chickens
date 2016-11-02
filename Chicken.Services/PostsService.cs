using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using System.Linq;
using Group = Chicken.Domain.Models.Group;

namespace Chicken.Services
{
    public class PostsService : IChickenService
    {
        private readonly NotificationService _notificationService;

        private readonly IRepository<Post> _posts;

        private readonly IRepository<Group> _groups;

        public PostsService(IRepository<Post> posts, NotificationService notificationService, IRepository<Domain.Models.Group> groups, IRepository<Comment> comments)
        {
            _posts = posts;
            _notificationService = notificationService;
            _groups = groups;
        }

        public Post GetPost(int postId)
        {
            var chicken = _posts.GetById(postId);
            return chicken;
        }

        public Group GetGroup(Func<Group, bool> func)
        {
            var group = _groups.Query().SingleOrDefault(func);
            return group;
        }

        public IEnumerable<Post> GetPosts(Group group, int skip, int take, string search, out int totalResultCount, bool withSpam = false, bool withSuggestions = false)
        {
            var query = _posts.Query().Where(x => x.GroupId == group.Id);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var startsWithKeyword = string.Format("{0} ", search);
                var endsWithKeyword = string.Format(" {0}", search);
                query = query.Where(x => x.Text.Contains(startsWithKeyword) || x.Text.Contains(endsWithKeyword));
            }

            if (!withSpam) { query = query.Where(x => !x.IsSpam); }
            if (!withSuggestions) { query = query.Where(x => !x.IsSuggestion); }
            totalResultCount = query.Count();
            query = query.OrderByDescending(x => x.Date).Skip(skip).Take(take);
            return query;
        }

        public IEnumerable<Post> GetTop(int top, Group group)
        {
            var posts =
                _posts
                .Query()
                .Where(x => !x.IsSpam && x.GroupId == group.Id)
                .OrderByDescending(x => x.Comments.Count)
                .Take(top);
            return posts;
        }

        public IEnumerable<Post> GetUserPosts(int accountId)
        {
            var query = _posts.Query().Where(x => x.AccountId == accountId);
            return query;
        }

        public IEnumerable<Group> GetGroups()
        {
            return _groups.Query().OrderBy(x => x.Name);
        }

        public IEnumerable<Group> GetActiveGroups()
        {
            return _groups.Query().Where(x=>x.IsActive).OrderBy(x => x.Name);
        }

        public int GetPostsCount(int groupId)
        {
            return _posts.Query().Count(x => x.Group.Id == groupId);
        }

        public int GetSpamCount(int groupId)
        {
            return _posts.Query().Count(x => x.IsSpam && x.Group.Id == groupId);
        }

        public int GetActivePostsCount(int groupId)
        {
            return _posts.Query().Count(x => !x.IsSpam && x.Group.Id == groupId);
        }

        public void EditPost(Post post)
        {
            _posts.Edit(post);
            _posts.Save();
        }

        public async Task<IEnumerable<Post>> AddNewPosts(int groupId)
        {
            var posts = GetNewPosts(groupId)
                .Where(x =>
                !string.IsNullOrEmpty(x.Text)
                && x.Attachments != null
                && x.Attachments.Any())
                .ToList();

            var newPosts = _posts.Query().Where(x => x.IsNew && x.GroupId == groupId).ToList();
            foreach (var newPost in newPosts)
            {
                newPost.IsNew = false;
                _posts.Edit(newPost);
            }

            if (posts.Any())
            {
                foreach (var post in posts)
                {
                    ImageManager.SetAvatar(post);
                    post.IsNew = true;
                    _posts.Add(post);
                }

                await _posts.SaveAsync();
                _notificationService.Notify(posts.Count());
            }

            return posts;
        }

        private IEnumerable<Post> GetNewPosts(int groupId)
        {
            var group = _groups.GetById(groupId);
            if (group != null)
            {
                var posts = new List<Post>();
                var savedChickens = _posts.Query()
                                          .Where(x => x.Group.Id == group.Id)
                                          .Select(x => x.PostId)
                                          .ToList();

                for (var i = 0; i < 1000; i++)
                {
                    var responsePosts = VkApi.GetPosts(i * 100, 100, group);
                    var newPosts = responsePosts.Where(x => !savedChickens.Contains(x.PostId)).ToList();

                    if (!newPosts.Any())
                    {
                        break;
                    }

                    posts.AddRange(newPosts);
                }

                return posts;
            }

            return new List<Post>();
        }

        private static void ModifyText(Post post, Regex regex)
        {
            post.Text = regex.Replace(post.Text, string.Empty);
        }

        public void AddPost(Post post)
        {
            _posts.Add(post);
            _posts.Save();
        }

        public void Delete(int id)
        {
            var post = _posts.Query().SingleOrDefault(x => x.Id == id);
            if (post != null && post.IsSuggestion)
            {
                _posts.Delete(post);
                _posts.Save();
            }
        }
    }
}
