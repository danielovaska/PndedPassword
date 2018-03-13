# PndedPassword
Code samples for using Troy Hunt Pnded passwords api for Episerver

To use:

1. Install nuget package on your Episerver 11 site
2. In Startup.cs replace the commented line below for identity based authentication:
```
//app.AddCmsAspNetIdentity<ApplicationUser>();
  
app.AddCustomCmsAspNetIdentity<ApplicationUser>();
```
3. Set your prefered password strength in appSettings
```
<add key="OwnedPassword:RequiredLength" value="0" />
<add key="OwnedPassword:RequireNonLetterOrDigit" value="false" />
<add key="OwnedPassword:RequireDigit" value="false" />
<add key="OwnedPassword:RequireLowercase" value="false" />
<add key="OwnedPassword:RequireUppercase" value="false" />
<add key="OwnedPassword:RequireOwnedPasswordsCheck" value="true" />
<add key="OwnedPassword:MaxAllowedOwnedPasswords" value="0" />
```
```
3. If you don't have identity based authentication you can still create a custom membership provider similar to:
```
    public class OwnedMembershipProvider : System.Web.Providers.DefaultMembershipProvider
    {
        private readonly LocalizationService localizationService;
        private readonly IOwnedPasswordRepository _ownedPasswordRepository;
        public const string DefaultErrorMessage = "Your password occurs in hacked databases {0} times. Try another password!";
        public int MaxAllowedOwnedPasswords { get; set; } = 0;
        public const string OwnedPasswordErrorKey = "/OwnedPasswordError";
        public OwnedMembershipProvider(IOwnedPasswordRepository ownedPasswordRepository) : base()
        {
            _ownedPasswordRepository = ownedPasswordRepository;
            localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();
        }
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var ownedPasswordsCount = _ownedPasswordRepository.GetOwnedCount(password);
            if (ownedPasswordsCount > MaxAllowedOwnedPasswords)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return (MembershipUser)null;
            }
            return base.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
        }
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var ownedPasswordsCount = _ownedPasswordRepository.GetOwnedCount(newPassword);
            if (ownedPasswordsCount > MaxAllowedOwnedPasswords)
            {
                throw new ArgumentException(string.Format(localizationService.GetString(OwnedPasswordErrorKey, DefaultErrorMessage), ownedPasswordsCount));
            }
            return base.ChangePassword(username, oldPassword, newPassword);
        }

    }
```
