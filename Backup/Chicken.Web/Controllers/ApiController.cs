using System.Linq;
using System.Web.Mvc;
using Chicken.Services;
using Chicken.Web.Models;

namespace Chicken.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly NotificationService _notificationService;

        private readonly PostsService _chickenService;

        private readonly CommentsService _commentsService;

        public ApiController(PostsService service, NotificationService notificationService, CommentsService commentsService)
        {
            _chickenService = service;
            _notificationService = notificationService;
            _commentsService = commentsService;
        }

        public JsonResult GetPosts(int groupId = 1, int skip = 0, int take = 50)
        {
            int totalResultCount;
            var currentGroup = _chickenService.GetGroup(x => x.Id == groupId);
            var posts = _chickenService.GetPosts(currentGroup, skip, take, null, out totalResultCount).ToList();
            var model = posts.Select(ListItemViewModel.Map).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetails(int id)
        {
            var post = _chickenService.GetPost(id);
            var model = DetailsViewModel.Map(post);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int id)
        {
            var comments = _commentsService.GetComments(id).ToList();
            var model = comments.Select(CommentViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void RegisterDevice(string id)
        {
            _notificationService.AddDevice(id);
        }
    }
}
