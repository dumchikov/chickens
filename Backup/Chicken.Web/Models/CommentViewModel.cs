using System;
using System.Text.RegularExpressions;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class CommentViewModel
    {
        private static readonly Regex _regex = new Regex(@"\[id\d*\|\w*\]");

        public string Avatar { get; set; }

        public string Link { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Date { get; set; }

        public static CommentViewModel Map(Comment comment)
        {
            return new CommentViewModel
                {
                    Date = string.Format("{0:dd/MM/yyyy HH:mm}", comment.Date),
                    Text = _regex.Replace(comment.Text, Regex.Match(comment.Text, @"(?<=\[id\d*\|)\w*(?=\])").Value),
                    Avatar = "http://prihozhanka.ru/styles/prosilver/theme/images/no_avatar.gif",
                    //Link = comment.User.Link,
                    Name = comment.User.FirstName
                };
        }
    }
}