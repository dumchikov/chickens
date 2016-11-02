using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chicken.Domain.Models;
using Chicken.Services;
using Chicken.Web.Models;

namespace Chicken.Web.Controllers
{
    public class MainController : BaseController
    {
        private const int DefaultTake = 16;

        private readonly PostsService _chickenService;

        private readonly CommentsService _commentsService;

        public MainController(
            PostsService chickenService,
            CommentsService commentsService,
            CurrentAccount currentAccount) :base(currentAccount)
        {
            _chickenService = chickenService;
            _commentsService = commentsService;
            CurrentAccount = currentAccount;
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
            var postsModel = posts.Select(ExtendedListItemViewModel.Map);
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
            var postsModel = posts.Select(ExtendedListItemViewModel.Map); //var postsModel = ExtendedListItemViewModel.GetStub();
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

        public ActionResult Details(int id)
        {
            var post = _chickenService.GetPost(id);
            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var groupName = post.Group.Name;
            ViewBag.GroupName = groupName;
            ViewBag.LinkToGroup = Url.RouteUrl("main", new {group = post.Group.GroupDomainName});

            //var asd = CurrentAccount.Account.Comments.Select(x => x.Id).ToList();
            var model = ExtendedDetailsViewModel.Map(post);
            model.Comments = _commentsService.GetComments(id).Select(CommentViewModel.Map).ToList();
            return View(model);
        }
    }
}
