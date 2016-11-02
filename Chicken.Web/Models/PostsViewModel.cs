using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class PostsViewModel
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public string ShortText { get; set; }
        public string Date { get; set; }
        public bool IsNew { get; set; }
        public bool IsSuggestion{ get; set; }

        public static PostsViewModel Map(Post post)
        {
            var model = new PostsViewModel
            {
                Id = post.Id,
                Comments = post.CommentsCount,
                Likes = post.LikesCount,
                Photo = post.Avatar,
                ShortText = post.Text.Length <= 200 ? post.Text : $"{post.Text.Substring(0, 200)}[...]",
                Date = string.Format("{0:dd/MM/yyyy}", post.Date),
                IsNew = post.IsNew,
                IsSuggestion = post.IsSuggestion
            };

            return model;
        }
    }
}