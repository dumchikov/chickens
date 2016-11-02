using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using Chicken.Services;
using Chicken.Services.Security;

namespace Chicken.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<Account> _accounts;

        private readonly VkAuthService _vkAuthService;

        public AccountController(IRepository<Account> accounts)
        {
            _accounts = accounts;
            _vkAuthService = new VkAuthService("3886778", "XRxEAyihyEZOlyFcHTpU");
        }

        public ActionResult SendVkAuthRequest()
        {
            var redirectUrl = string.Format("http://koko.in.ua/account/vklogin?url={0}", Request.Headers["Referer"]);
            var url = _vkAuthService.GetAuthenticationUrl(redirectUrl, "email");
            return Redirect(url);
        }

        public ActionResult VkLogin(string code, string url)
        {
            var redirectUrl = string.Format("http://koko.in.ua/account/vklogin?url={0}", Request.Headers["Referer"]);
            var accessToken = _vkAuthService.GetAccessToken(code, redirectUrl);
            var account = _accounts.Query().FirstOrDefault(x => x.ProfileId == accessToken.UserId);
            if (account == null)
            {
                var userInfo = VkApi.GetUsers(new[] { accessToken.UserId }).First();

                account = new Account
                    {
                        ProfileId = userInfo.ProfileId,
                        Avatar = userInfo.Avatar,
                        Email = accessToken.Email,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        IsAdmin = false
                    };

                _accounts.Add(account);
                _accounts.Save();
            }

            FormsAuthentication.SetAuthCookie(account.Id.ToString(), true);
            return Redirect(url);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(Request.Headers["Referer"]);
        }
    }
}
