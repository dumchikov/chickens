using System.Web.Mvc;
using Chicken.Services;
using Chicken.Web.Models;
using System.Linq;

namespace Chicken.Web.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public PostsService _postsService;

        public ProfileController(CurrentAccount currentAccount, PostsService postService) : base(currentAccount)
        {
            _postsService = postService;
        }

        public ActionResult Index()
        {
            ViewBag.GroupName = "Курицы Украины";
            var currentAccount = ((CurrentAccount)ViewBag.CurrentAccount).Account;
            var posts = _postsService.GetUserPosts(currentAccount.Id).ToList();
            var postsModel = posts.Select(PostsViewModel.Map);

            var model = new ProfileModel
            {
                Avatar = currentAccount.Avatar,
                Email = currentAccount.Email,
                Name = string.Format("{0} {1}", currentAccount.FirstName, currentAccount.LastName),
                UserPosts = postsModel
            };
            
            return View(model);
        }

        [HttpPost]
        public JsonResult DeletePost(int id) {
            _postsService.Delete(id);
            return Json(new { Success = true });            
        }
    }
}
