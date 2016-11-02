using System.Web.Mvc;
using Chicken.Services;

namespace Chicken.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(CurrentAccount currentAccount) : base(currentAccount)
        {
        }

        public ActionResult Index()
        {
            ViewBag.GroupName = "Курицы Украины";
            return View();
        }
    }
}
