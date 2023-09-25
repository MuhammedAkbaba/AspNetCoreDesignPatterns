using BaseProject.Models;
using System.Text;

namespace WebApp.Template.UserCards
{
    /// <summary>
    /// Üye olmayan kullanıcı template
    /// </summary>
    public class DefaultUserCardTemplate : UserCardTemplate
    {

        protected override string SetFooter()
        {
            return string.Empty;
        }

        protected override string SetPicture()
        {
            var sb = new StringBuilder();
            sb.Append($@"<img src='/userpictures/defaultUser.png'  class='card-img-top'  height='300px' width='30px'");
            return sb.ToString();

        }

    }
}
