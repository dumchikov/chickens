using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Chicken.Domain.Models;
using Newtonsoft.Json.Linq;

namespace Chicken.Services
{
    public class VkApi
    {
        //https://oauth.vk.com/authorize?client_id=4967199&scope=offline,groups,wall,photos&display=page&v=5.7&response_type=token

        private const string Domain = "https://api.vk.com";
        private const string Secret = "yGxH1Oe4k3ffoHkGzBLO";

        public static IEnumerable<Comment> GetComments(int count, int offset, Post post, out int totalCount)
        {
            try
            {
                var requestString = string.Format("/method/wall.getComments?" +
                        "v=5.29&" +
                        "extended=1&" +
                        "count={0}&" +
                        "offset={1}&" +
                        "post_id={2}&" +
                        "owner_id=-{3}&" +
                        "access_token={4}",
                        count,
                        offset,
                        post.PostId,
                        post.Group.OwnerId,
                        post.Group.AccessToken);
                var sig = GetMD5Hash(requestString + Secret);
                var url = string.Format("{0}{1}&sig={2}", Domain, requestString, sig);
                var response = GetResponse(url);
                totalCount = response.SelectToken("count").ToObject<int>();
                var items = response.SelectToken("items");
                var comments = items.ToObject<IEnumerable<Comment>>().ToList();
                return comments;
            }
            catch (Exception ex)
            {
                totalCount = 0;
                return new List<Comment>();
            }
        }

        public static IEnumerable<Post> GetPosts(int skip, int take, Group group)
        {
            var requestString = string.Format("/method/wall.get?" +
                                    "v=5.29&" +
                                    "count={0}&" +
                                    "offset={1}&" +
                                    "filter=all&" +
                                    "domain={2}&" +
                                    "access_token={3}",
                                    take,
                                    skip,
                                    group.GroupDomainName,
                                    group.AccessToken);

            var sig = GetMD5Hash(requestString + Secret);
            var url = string.Format("{0}{1}&sig={2}", Domain, requestString, sig);
            var response = GetResponse(url);
            var items = response.SelectToken("items");
            var posts = items.ToObject<IEnumerable<Post>>().ToList();
            posts.ForEach(x => x.UpdateLikesAndComments());
            posts.ForEach(x => x.SetGroup(group));
            return posts;
        }

        public static IEnumerable<User> GetUsers(IEnumerable<int> userIds)
        {
            var requestString = string.Format("/method/users.get?" +
                        "v=5.29&" +
                        "user_ids={0}&" +
                        "fields=photo_100,screen_name",
                        string.Join(",", userIds));

            var url = string.Format("{0}{1}", Domain, requestString);
            var response = GetResponse(url);
            var users = response.ToObject<IEnumerable<User>>();
            return users;
        }

        private static JToken GetResponse(string url)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            webClient.Headers["Accept-Language"] = "ru-RU";
            var response = webClient.DownloadString(url);
            var responseToken = JObject.Parse(response).SelectToken("response");
            return responseToken;
        }

        private static string GetMD5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
