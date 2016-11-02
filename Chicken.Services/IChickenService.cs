using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chicken.Domain.Models;

namespace Chicken.Services
{
    interface IChickenService
    {
        Post GetPost(int postId);

        Group GetGroup(Func<Group, bool> func);

        IEnumerable<Post> GetPosts(Group group, int skip, int take, string search, out int totalResultCount, bool withSpam, bool withSuggestions);

        IEnumerable<Post> GetTop(int top, Group group);

        void AddPost(Post post);

        void EditPost(Post post);

        Task<IEnumerable<Post>> AddNewPosts(int cityId);
    }
}
