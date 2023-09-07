using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();
            if (User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault() != null)
            {
                var hasClaim = User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault();

                settings.DatabaseType = Enum.Parse<EDatabaseType>(hasClaim.Value);
            }
            else
            {
                settings.DatabaseType = settings.GetDefaultDatabaseType;
            }

            return View(settings);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int databaseType)
        {
            var hasUser = await _userManager.FindByNameAsync(User.Identity.Name);

            ///yeni bir claim oluşturma
            var newClaim = new Claim(Settings.claimDatabaseType, databaseType.ToString());

            ///user ait claim listesi
            var claim = await _userManager.GetClaimsAsync(hasUser);

            ///claim listesinin içerisinden  type tipi claimDatabaseType(databasetype) olan kayıt var mı 
            var hasDatabaseTypeClaim = claim.FirstOrDefault(x => x.Type == Settings.claimDatabaseType);
            if (hasDatabaseTypeClaim != null)
            {
                ///mevcut claim tekrar güncelleme yapılıyor 
                await _userManager.ReplaceClaimAsync(hasUser, hasDatabaseTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(hasUser, newClaim);
            }

            ///Kullanıcının claim bilgileri değiştirdik ama cookie'de hayla eski bilgiler olabilir.
            ///Bu yüzden kullanıcıyı çıkış yaptırıp tekrar giriş yaptırıcaz ama kullanıcıya bunu hissettirmeden 
            ///Aşağıda olduğu gibi
            
            await _signInManager.SignOutAsync();
            ///cookie de bazı var olan değerler vardır bunları da almak için aşağıdaki gibi kod yazmamız gerek
            ///bu var olan değerler ve yeni değerleride ekleyerek yeniden cookie oluşturmak için
            ///örnek beni hatırla seçili ise ve diğer benzeri bilgiler
            var authenticateResult = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(hasUser, authenticateResult.Properties);


            return RedirectToAction(nameof(Index));
        }

    }
}
