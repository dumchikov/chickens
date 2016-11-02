using System.Collections.Generic;
using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class ExtendedDetailsViewModel : DetailsViewModel
    {
        public string Date { get; set; }

        public int Likes { get; set; }

        public IList<CommentViewModel> Comments { get; set; }

        public new static ExtendedDetailsViewModel Map(Post post)
        {
            var model = (ExtendedDetailsViewModel) DetailsViewModel.Map(post);
            model.Likes = post.LikesCount;
            model.Date = string.Format("{0:dd/MM/yyyy HH:mm}", post.Date);
            //model.Comments = post.Comments.Select(CommentViewModel.Map);
            return model;
        }
    }

    public class DetailsViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public IList<string> Photos { get; set; }

        public static DetailsViewModel Map(Post post)
        {
            var model = new ExtendedDetailsViewModel
                {
                    Id = post.Id,
                    Photos = new List<string>(),
                    Text = post.Text
                };

            if (post.Attachments != null && post.Attachments.Any())
            {
                foreach (var attachment in post.Attachments)
                {
                    if (attachment.Photo != null)
                    {
                        model.Photos.Add(attachment.Photo.Photo604Url);
                    }
                }
            }

            return model;
        }
    }
}