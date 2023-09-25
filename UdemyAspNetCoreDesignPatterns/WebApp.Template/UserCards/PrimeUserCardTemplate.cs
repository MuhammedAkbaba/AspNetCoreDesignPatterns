using BaseProject.Models;
using System.Text;

namespace WebApp.Template.UserCards
{
    public class PrimeUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();
            sb.Append("<a href=\"#\" class=\"card-link\">Mesaj Gönder</a>");
            sb.Append("<a href=\"#\" class=\"card-link\">Diğer Bilgiler</a>");
            return sb.ToString();
        }

        protected override string SetPicture()
        {
            var sb = new StringBuilder();
            sb.Append($@"<img src='{AppUser.PictureUrl}' class='card-img-top'  height='20px' width='50px'");
            return sb.ToString();
        }
    }
}
