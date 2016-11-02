using System.Collections.Generic;

namespace Chicken.Web.Models
{
    public class ProfileModel
    {
        public string Avatar { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<PostsViewModel> UserPosts { get; set; }
    }
}