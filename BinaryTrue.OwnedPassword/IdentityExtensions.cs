using System;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Shell.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;


namespace BinaryTrue.OwnedPassword
{

    /// <summary>
    /// Some helper methods to use with Episerver identity based sites. 
    /// You can simply use it in your owin Startup.cs
    /// 
    /// app.AddCustomCmsAspNetIdentity<ApplicationUser>();
    /// </summary>
    public static class IdentityExtensions
    {
        public static IAppBuilder AddCustomCmsAspNetIdentity<TUser>(this IAppBuilder app) where TUser : IdentityUser, IUIUser, new()
        {
            return app.AddCustomCmsAspNetIdentity<TUser>(new ApplicationOptions());
        }
        public static IAppBuilder AddCustomCmsAspNetIdentity<TUser>(this IAppBuilder app, ApplicationOptions applicationOptions) where TUser : IdentityUser, IUIUser, new()
        {
            applicationOptions.DataProtectionProvider = app.GetDataProtectionProvider();
            app.CreatePerOwinContext<ApplicationOptions>((Func<ApplicationOptions>)(() => applicationOptions));
            app.CreatePerOwinContext<ApplicationDbContext<TUser>>(new Func<IdentityFactoryOptions<ApplicationDbContext<TUser>>, IOwinContext, ApplicationDbContext<TUser>>(ApplicationDbContext<TUser>.Create));
            app.CreatePerOwinContext<ApplicationRoleManager<TUser>>(new Func<IdentityFactoryOptions<ApplicationRoleManager<TUser>>, IOwinContext, ApplicationRoleManager<TUser>>(ApplicationRoleManager<TUser>.Create));
            app.CreatePerOwinContext<ApplicationUserManager<TUser>>(new Func<IdentityFactoryOptions<ApplicationUserManager<TUser>>, IOwinContext, ApplicationUserManager<TUser>>(ApplicationUserManagerInitializer<TUser>.Create));
            app.CreatePerOwinContext<ApplicationSignInManager<TUser>>(new Func<IdentityFactoryOptions<ApplicationSignInManager<TUser>>, IOwinContext, ApplicationSignInManager<TUser>>(ApplicationSignInManager<TUser>.Create));
            app.CreatePerOwinContext<UIUserProvider>(new Func<IdentityFactoryOptions<UIUserProvider>, IOwinContext, UIUserProvider>(ApplicationUserProvider<TUser>.Create));
            app.CreatePerOwinContext<UIRoleProvider>(new Func<IdentityFactoryOptions<UIRoleProvider>, IOwinContext, UIRoleProvider>(ApplicationRoleProvider<TUser>.Create));
            app.CreatePerOwinContext<UIUserManager>(new Func<IdentityFactoryOptions<UIUserManager>, IOwinContext, UIUserManager>(ApplicationUIUserManager<TUser>.Create));
            app.CreatePerOwinContext<UISignInManager>(new Func<IdentityFactoryOptions<UISignInManager>, IOwinContext, UISignInManager>(ApplicationUISignInManager<TUser>.Create));
            ConnectionStringNameResolver.ConnectionStringNameFromOptions = applicationOptions.ConnectionStringName;
            return app;
        }
    }
}
