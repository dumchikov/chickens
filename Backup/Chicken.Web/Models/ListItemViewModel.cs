using System;
using System.Collections.Generic;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class ExtendedListItemViewModel : ListItemViewModel
    {
        public string ShortText { get; set; }

        public string Date { get; set; }

        public bool IsNew { get; set; }

        public new static ExtendedListItemViewModel Map(Post post)
        {
            var model = (ExtendedListItemViewModel)ListItemViewModel.Map(post);
            model.ShortText =
                post.Text.Length <= 200
                ? post.Text
                : string.Format("{0}[...]", post.Text.Substring(0, 200));
            model.Date = string.Format("{0:dd/MM/yyyy}", post.Date);
            model.IsNew = post.IsNew;
            return model;
        }

        public static IEnumerable<ExtendedListItemViewModel> GetStub()
        {
            var post1 = new ExtendedListItemViewModel
                {
                    Comments = 100,
                    Likes = 15,
                    Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                    Id = 1,
                    Photo = "https://cs7055.vk.me/c7001/v7001690/e2c6/mJIXRMtWRf0.jpg",
                    ShortText = "text"
                };
            var post2 = new ExtendedListItemViewModel
            {
                Comments = 80,
                Likes = 85,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 2,
                Photo = "https://cs7055.vk.me/c7001/v7001611/d8d9/RXgwm1ZuuVs.jpg",
                ShortText = "text"
            };
            var post3 = new ExtendedListItemViewModel
            {
                Comments = 70,
                Likes = 17,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 3,
                Photo = "https://cs7055.vk.me/c7001/v7001811/dc70/8tMWc-tmBjM.jpg",
                ShortText = "text"
            };
            var post4 = new ExtendedListItemViewModel
            {
                Comments = 60,
                Likes = 73,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 4,
                Photo = "https://cs7055.vk.me/c7001/v7001632/d591/oZ_FF0wk8OM.jpg",
                ShortText = "text"
            };
            var post5 = new ExtendedListItemViewModel
            {
                Comments = 50,
                Likes = 54,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 5,
                Photo = "https://cs7055.vk.me/c7001/v7001250/fb68/0jhI3Sekus0.jpg",
                ShortText = "text"
            };
            var post6 = new ExtendedListItemViewModel
            {
                Comments = 40,
                Likes = 42,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 6,
                Photo = "https://cs7055.vk.me/c7001/v7001539/daed/T0fqUJogpTk.jpg",
                ShortText = "text"
            };
            var post7 = new ExtendedListItemViewModel
            {
                Comments = 30,
                Likes = 11,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 7,
                Photo = "https://cs7055.vk.me/c7001/v7001009/e07e/xU6DtkyS4-I.jpg",
                ShortText = "text"
            };
            var post8 = new ExtendedListItemViewModel
            {
                Comments = 20,
                Likes = 22,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 8,
                Photo = "https://cs7055.vk.me/c7001/v7001488/dfad/5vd_ES7genw.jpg",
                ShortText = "text"
            };
            var post9 = new ExtendedListItemViewModel
            {
                Comments = 10,
                Likes = 20,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 9,
                Photo = "https://cs7055.vk.me/c7001/v7001621/dee0/K5n7GEfICfE.jpg",
                ShortText = "text"
            };
            var post10 = new ExtendedListItemViewModel
            {
                Comments = 55,
                Likes = 30,
                Date = string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Id = 10,
                Photo = "https://cs7055.vk.me/c7001/v7001585/d68d/8RjWx8Oz6r8.jpg",
                ShortText = "text"
            };

            return new List<ExtendedListItemViewModel> { post1, post2, post3, post4, post5, post6, post7, post8, post9, post10 };
        }
    }

    public class ListItemViewModel
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }

        public static ListItemViewModel Map(Post post)
        {
            var model = new ExtendedListItemViewModel
                {
                    Id = post.Id,
                    Comments = post.CommentsCount,
                    Likes = post.LikesCount,
                    Photo = post.Avatar
                };

            return model;
        }
    }
}