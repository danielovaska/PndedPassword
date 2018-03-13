using EPiServer.Shell.Security;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Configuration;
using System;

namespace BinaryTrue.OwnedPassword
{
    public static class ApplicationUserManagerInitializer <TUser> where TUser : IdentityUser, IUIUser, new()
    {
        public static ApplicationUserManager<TUser> Create(IdentityFactoryOptions<ApplicationUserManager<TUser>> options, IOwinContext context)
        {
            var userManager = ApplicationUserManager<TUser>.Create(options,  context);
           
            userManager.PasswordValidator = new OwnedPasswordValidator(new OwnedPasswordRepository())
            {
                RequiredLength = AppSettings.RequiredLength,
                RequireNonLetterOrDigit = AppSettings.RequireNonLetterOrDigit,
                RequireDigit = AppSettings.RequireDigit,
                RequireLowercase = AppSettings.RequireLowercase,
                RequireUppercase = AppSettings.RequireUppercase,
                MaxAllowedOwnedPasswords = AppSettings.MaxAllowedOwnedPasswords,
                RequireOwnedPasswordsCheck = AppSettings.RequireOwnedPasswordsCheck
            };
            return userManager;
        }
    }
}
