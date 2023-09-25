
using BaseProject.Models;
using System.Text;

namespace WebApp.Template.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser AppUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            AppUser = appUser;
        }

        /*İşlem sırasını belirlendiği metotdur. UML diyagramına göre :" Template method" olur.
         Template Metodu ve algoritmadaki sıralı operasyonları fiziksel olarak barındıran soyut sınıftır.*/
        public string Build()
        {
            if (AppUser == null) throw new ArgumentNullException(nameof(AppUser));

            var sb = new StringBuilder();
            sb.Append("<div class='card'>");
            sb.Append(SetPicture());
            sb.Append($@"<div class='card-body'>
                          <h5>{AppUser.UserName}</h5>
                          <p>{AppUser.Description}</p> ");
            sb.Append(SetFooter());
            sb.Append(" </div>");


            sb.Append("</div>");

            return sb.ToString();

        }
        protected abstract string SetFooter();
        protected abstract string SetPicture();

        


    }
}
