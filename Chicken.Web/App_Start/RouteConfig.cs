using System.Web.Mvc;
using System.Web.Routing;

namespace Chicken.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "details",
                url: "post/{id}",
                defaults: new
                    {
                        controller = "main",
                        action = "details"
                    }
            );

            routes.MapRoute(
                name: "admin",
                url: "admin/{action}/{id}",
                defaults: new
                {
                    controller = "admin",
                    action = "index",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "login",
                url: "login/{action}/{id}",
                defaults: new
                {
                    controller = "login",
                    action = "index",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "account",
                url: "account/{action}/{id}",
                defaults: new
                {
                    controller = "account",
                    action = "index",
                    id = UrlParameter.Optional
                });


            routes.MapRoute(
                name: "profile",
                url: "profile/{action}/{id}",
                defaults: new
                {
                    controller = "profile",
                    action = "index",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "payment",
                url: "payment/{action}/{id}",
                defaults: new
                {
                    controller = "payment",
                    action = "index",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "api",
                url: "api/{action}/{id}",
                defaults: new
                {
                    controller = "api",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "top",
                url: "{group}/top",
                defaults: new
                {
                    controller = "main",
                    action = "top",
                }
            );

            routes.MapRoute(
                name: "suggest",
                url: "suggest",
                defaults: new
                {
                    controller = "main",
                    action = "suggest"                   
                }
            );

            routes.MapRoute(
                name: "savephoto",
                url: "savephoto",
                defaults: new
                {
                    controller = "main",
                    action = "savephoto"
                });

            routes.MapRoute(
                name: "picture",
                url: "picture/{id}",
                defaults: new
                {
                    controller = "images",
                    action = "get",
                    id = UrlParameter.Optional

                }
            );

            routes.MapRoute(
                name: "main",
                url: "{group}/{page}",
                defaults: new
                {
                    controller = "main",
                    action = "posts",
                    page = 1
                }
            );

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "main",
                    action = "index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}