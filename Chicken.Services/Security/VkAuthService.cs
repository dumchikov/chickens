using System.Net;
using Newtonsoft.Json;

namespace Chicken.Services.Security
{
    public class VkAuthService
    {
        private const string AuthorizeUrl = "https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&response_type=code&v=5.1";

        private const string AccessTokenUrl = "https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&code={2}&redirect_uri={3}";

        private readonly string _appId;

        private readonly string _secret;

        public VkAuthService(string appId, string secret)
        {
            this._appId = appId;
            this._secret = secret;
        }

        public string GetAuthenticationUrl(string redirectUrl, string permissions = null)
        {
            var url = string.Format(AuthorizeUrl, _appId, permissions, redirectUrl);
            return url;
        }

        public VkAccessToken GetAccessToken(string code, string redirectUrl)
        {
            var url = string.Format(AccessTokenUrl, _appId, _secret, code, redirectUrl);
            var webClient = new WebClient();
            var response = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<VkAccessToken>(response);
        }
    }
}
