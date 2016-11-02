
using System.Web.Mvc;
using Chicken.Services;

namespace Chicken.Web.Controllers
{
    public class BaseController : Controller
    {
        protected CurrentAccount CurrentAccount;

        public BaseController(CurrentAccount currentAccount)
        {
            CurrentAccount = currentAccount;

            ViewBag.CurrentAccount = currentAccount;
        }
    }
}
