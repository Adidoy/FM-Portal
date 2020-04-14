using System.Web.Mvc;

namespace PUPFMIS.Areas.EndUsers
{
    public class EndUsersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "end-users";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "end-users_default",
                "end-users/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "PUPFMIS.Areas.EndUsers.Controllers" }
            );
        }
    }
}