using System;
using System.Linq;
using System.Web.Security;
using YourPicture.Domain.Interfaces.DAL;
using YourPicture.Domain.Interfaces.Security;
using YourPicture.Domain.Models;

namespace YouPicture.Service.Security
{
    public class WebSecurity : IWebSecurity
    {
        private readonly IRepository<User> _users;

        private readonly VkontakteService _vkontakteService;

        public WebSecurity(IRepository<User> users)
        {
            this._users = users;
            this._vkontakteService = new VkontakteService("3886778", "YKh2M1I1GbebSDseGEIS", "http://xxx.com/account/vklogin");
        }
       
        private AuthorizationResult Login(Func<User, bool> userQuery)
        {
            var user = this._users.SingleOrDefault(userQuery);
            var success = false;

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                success = true;
            }

            return new AuthorizationResult(success);
        }

        private void CreateVkUser(VkRegistrationModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                VkId = model.VkId,
                Gender = model.Gender,
                Birthday = model.Birthday,
                Avatar = model.BigPhoto
            };

            this.CreateUser(user);
        }

        private User CreateUser(User user)
        {
            this._users.Save(user);
            return user;
        }

        public AuthorizationResult Login(string login, string password)
        {
            return this.Login(x => x.Login == login && x.Password == password);
        }

        public AuthorizationResult VkLogin(string code)
        {
            var accessToken = this._vkontakteService.GetAccessToken(code);
            var loginResult = this.Login(x => x.VkId == accessToken.UserId);

            if (!loginResult.Success)
            {
                var userInfo = this._vkontakteService.GetUserInfo(accessToken.UserId, accessToken.AccessToken);
                this.CreateVkUser(userInfo);
            }

            return this.Login(x => x.VkId == accessToken.UserId);
        }

        public RegistrationResult CreateAccount(RegistrationModel model)
        {
            var loginExist = this._users.Any(x => x.Login == model.Login);
            var emailExist = this._users.Any(x => x.Email == model.Email);
            var result = new RegistrationResult();

            if (!emailExist && !loginExist)
            {
                CreateUser(new User {Login = model.Login, Password = model.Password, Email = model.Email});
                result.Success = true;
                return result;
            }
            if (emailExist)
            {
                result.Errors.Add("Email", "Email is already exist.");
            }
            if (loginExist)
            {
                result.Errors.Add("Login", "Login is already exist.");
            }

            return result;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}