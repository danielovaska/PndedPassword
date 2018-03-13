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
                RequiredLength =  6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                MaxAllowedOwnedPasswords = 0
            };
            return userManager;
        }
    }
}
