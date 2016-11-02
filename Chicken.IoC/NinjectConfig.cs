using System.Web;
using Chicken.DAL;
using Chicken.Domain.Interfaces;
using Chicken.Services;
using Ninject;
using Ninject.Web.Common;

namespace Chicken.IoC
{
    public static class NinjectConfig
    {
        public static NinjectDependencyResolver GetNinjectDependencyResolver()
        {
            var ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ChickenDbContext>().ToSelf().InRequestScope();
            ninjectKernel.Bind(typeof(IRepository<>)).To(typeof(EFRepository<>)).InRequestScope();
            ninjectKernel.Bind(typeof (PostsService)).ToSelf().InRequestScope();
            ninjectKernel.Bind(typeof(CommentsService)).ToSelf().InRequestScope();
            ninjectKernel.Bind(typeof(NotificationService)).ToSelf().InRequestScope();
            ninjectKernel.Bind(typeof(AccountService)).ToSelf().InRequestScope();
            ninjectKernel
                .Bind<CurrentAccount>()
                         .ToMethod(x => x.Kernel
                                 .Get<AccountService>()
                                 .Get(
                                 HttpContext.Current.User.Identity.IsAuthenticated 
                                 ? int.Parse(HttpContext.Current.User.Identity.Name) 
                                 : 0)).InRequestScope();
            return new NinjectDependencyResolver(ninjectKernel);
        }
    }
}