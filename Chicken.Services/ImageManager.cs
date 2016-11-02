using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Chicken.Domain.Models;

namespace Chicken.Services
{
    public class ImageManager
    {
        public static void SetAvatar(Post post)
        {
            if (post.Attachments != null)
            {
                string avatar;

                var photos =
                    post
                    .Attachments
                    .Where(x => x.Photo != null && x.Photo.Photo604Url != null)
                    .Select(x => x.Photo.Photo604Url)
                    .ToList();

                if (!photos.Any())
                {
                    return;
                }

                if (photos.Count() == 1)
                {
                    avatar = photos.First();
                    post.IsSpam = true;
                }
                else
                {
                    avatar =
                        photos.
                        Select(x => new
                        {
                            Photo = x,
                            Delta = GetPhotoDelta(x)
                        })
                        .OrderBy(x => x.Delta)
                        .First().Photo;
                }

                post.Avatar = avatar;
            }
        }

        private static int GetPhotoDelta(string photo)
        {
            var img = GetImageFromUrl(photo);
            var delta = Math.Abs(img.Height - img.Width);
            return delta;
        }

        private static Image GetImageFromUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                return ByteArrayToImage(webClient.DownloadData(url));
            }
        }

        private static Image ByteArrayToImage(byte[] fileBytes)
        {
            using (var stream = new MemoryStream(fileBytes))
            {
                return Image.FromStream(stream);
            }
        }
    }
}
