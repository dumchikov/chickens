using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chicken.Domain.Models;
using Chicken.Services;
using Chicken.Web.Models;
using Chicken.Domain.Interfaces;
using System.IO;

namespace Chicken.Web.Controllers
{
    public class MainController : BaseController
    {
        private const int DefaultTake = 16;

        private readonly PostsService _chickenService;

        private readonly CommentsService _commentsService;

        private readonly IRepository<Account> _accounts;

        private readonly IRepository<Comment> _comments;

        public MainController(
            PostsService chickenService,
            CommentsService commentsService,
            CurrentAccount currentAccount,
            IRepository<Account> accounts,
            IRepository<Comment> comments) : base(currentAccount)
        {
            _chickenService = chickenService;
            _commentsService = commentsService;
            CurrentAccount = currentAccount;
            _accounts = accounts;
            _comments = comments;
        }

        public ActionResult Index()
        {
            var allGroups = _chickenService.GetActiveGroups();
            return View(allGroups);
        }

        public ActionResult Top(string group)
        {
            var currentGroup = _chickenService.GetGroup(x => x.GroupDomainName == group);
            if (currentGroup == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            var posts = _chickenService.GetTop(30, currentGroup);
            var postsModel = posts.Select(PostsViewModel.Map);
            var allGroups = _chickenService.GetActiveGroups();
            ViewBag.GroupName = currentGroup.Name;
            var model = new MainViewModel
            {
                CurrentPage = 1,
                CurrentGroup = group,
                NumberOfPages = 1,
                GroupsSelectList = new SelectList(allGroups, "GroupDomainName", "Name", group),
                Posts = postsModel
            };

            ViewBag.IsTop = true;
            ViewBag.LinkToGroup = Url.RouteUrl("main", new { group = currentGroup.GroupDomainName });
            return View("Posts", model);
        }

        public ActionResult Posts(int page, string group, string search)
        {
            int totalResultCount;
            var skip = page != 1 ? DefaultTake * (page - 1) : 0;
            var currentGroup = _chickenService.GetGroup(x => x.GroupDomainName == group);

            if (currentGroup == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var allGroups = _chickenService.GetActiveGroups();
            var posts = _chickenService.GetPosts(currentGroup, skip, DefaultTake, search, out totalResultCount);
            var postsModel = posts.Select(PostsViewModel.Map);
            var numberOfPages = (int)Math.Ceiling((double)totalResultCount / DefaultTake);

            ViewBag.GroupName = currentGroup.Name;
            ViewBag.LinkToGroup = Url.RouteUrl("main", new { group = currentGroup.GroupDomainName });
            var model = new MainViewModel
            {
                CurrentPage = page,
                CurrentGroup = group,
                NumberOfPages = numberOfPages,
                GroupsSelectList = new SelectList(allGroups, "GroupDomainName", "Name", group),
                Posts = postsModel,
                Keyword = search
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Suggest(SuggestModel model)
        {
            var group = _chickenService.GetGroup(x => x.GroupDomainName == model.GroupDomainName);

            var post = new Post
            {
                Text = model.Description,
                IsNew = true,
                Date = DateTime.Now,
                GroupId = group.Id,
                Attachments = new List<Attachment>(),
                AccountId = CurrentAccount.Account.Id,
                IsSuggestion = true, 
            };

            if (model.PhotosUrls.Any()) {
                post.Attachments = new List<Attachment>();
            }

            foreach(var photoUrl in model.PhotosUrls) {
                var photo = new Photo { Photo604Url = photoUrl };
                var attachment = new Attachment { Photo = photo, Type = "photo" };
                post.Attachments.Add(attachment);
            }

            post.Avatar = model.PhotosUrls.Any() ? model.PhotosUrls.First() : null;
            _chickenService.AddPost(post);
            return Json(new { Success = true });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SavePhoto()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var id = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/storage/"), id + extension);
                    file.SaveAs(path);
                    return Json(new { Success = true, PhotoUrl = string.Concat("storage/", id, extension) });
                }
            }

            return Json(new { Success = false });
        }

        public FileResult GetPhoto(string guid)
        {
            return null;
        }

        public ActionResult Details(int id)
        {
            var post = _chickenService.GetPost(id);
            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var groupName = post.Group.Name;
            ViewBag.GroupName = groupName;
            ViewBag.LinkToGroup = Url.RouteUrl("main", new { group = post.Group.GroupDomainName });

            //var asd = CurrentAccount.Account.Comments.Select(x => x.Id).ToList();
            var model = ExtendedDetailsViewModel.Map(post);
            model.Comments = _commentsService.GetComments(id).Select(CommentViewModel.Map).ToList();
            return View(model);
        }
    }
}
