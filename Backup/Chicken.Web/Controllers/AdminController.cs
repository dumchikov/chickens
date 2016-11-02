using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Chicken.Services;
using System.Linq;
using Chicken.Web.Models.Admin;

namespace Chicken.Web.Controllers
{
    [Authorize(Users = "admin")]
    public class AdminController : Controller
    {
        private readonly PostsService _chickenService;

        public AdminController(PostsService chickenService)
        {
            _chickenService = chickenService;
        }

        public ActionResult Index(int groupId = 1)
        {
            var allGroups = _chickenService.GetGroups();
            dynamic model = new ExpandoObject();
            model.totalCount = _chickenService.GetPostsCount(groupId);
            model.spamCount = _chickenService.GetSpamCount(groupId);
            model.groups = allGroups.Select(x => new {text = x.Name, value = x.Id}).ToList();
            model.groupId = groupId;
            return View(model);
        }

        public JsonResult GetPosts(int groupId = 1, int skip = 0, int take = 100)
        {
            int totalResultCount;
            var currentGroup = _chickenService.GetGroup(x => x.Id == groupId);
            var posts = _chickenService.GetPosts(currentGroup, skip, take, null, out totalResultCount, true).ToList();
            var model = posts.Select(AdminPostViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Update(int groupId)
        {
            var posts = await _chickenService.AddNewPosts(groupId);
            var model = posts.OrderByDescending(x => x.Date).Select(AdminPostViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(IList<AdminPostViewModel> models)
        {
            if (models != null)
            {
                foreach (var model in models)
                {
                    var post = _chickenService.GetPost(model.Id);
                    post.IsSpam = model.IsSpam;
                    post.Text = model.Text;
                    _chickenService.EditPost(post);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
