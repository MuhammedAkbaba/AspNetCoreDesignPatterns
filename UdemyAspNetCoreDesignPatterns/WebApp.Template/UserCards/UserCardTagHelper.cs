using BaseProject.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace WebApp.Template.UserCards
{
    //<user-card app-user= /> şeklinde olur
    public class UserCardTagHelper : TagHelper
    {

        public AppUser AppUser { get; set; }
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userCard;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                userCard = new PrimeUserCardTemplate();
            else
                userCard = new DefaultUserCardTemplate();


            userCard.SetUser(AppUser);

            output.Content.SetHtmlContent(userCard.Build());

        }

    }
}
