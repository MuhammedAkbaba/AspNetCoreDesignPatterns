using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace WebApp.Command.Models
{
    public static class DataSeeding
    {
        public static async void SeedUser(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateAsyncScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (!userManager.Users.Any())
                {
                    ///.Wait(); metot çalışacak kodu bloklar yani kod işini bitirene kadar alt satırdaki koda geçmez.
                    ///await  ise : 
                    ///await userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@gmail.com" }, "Password123*");
                    ///yukardaki koddaki await keyword ise bloklama yapmaz kodu ayrı bir yerde işlemine devam ederken alt satırdaki kodları çalıştırmaya 
                    ///devam eder aynı zamanda o kodunda takibini yapar ki kod işlemini bitirene kadar.
                    ///
                    userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@gmail.com" }, "Password123*").Wait();
                    userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@gmail.com" }, "Password123*").Wait();
                    userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@gmail.com" }, "Password123*").Wait();
                }
            }
        }
    }
}
