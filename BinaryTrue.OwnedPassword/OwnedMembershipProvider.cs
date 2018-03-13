using System;
using System.Web.Security;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;


namespace BinaryTrue.OwnedPassword
{
    /// <summary>
    /// Example membership provider if you use the old provider based authentication. Use it by modifying you membership provider in web.config.
    /// </summary>
    //public class OwnedMembershipProvider:System.Web.Providers.DefaultMembershipProvider
    //{
    //    private readonly LocalizationService localizationService;
    //    private readonly IOwnedPasswordRepository _ownedPasswordRepository;
    //    public const string DefaultErrorMessage = "Your password occurs in hacked databases {0} times. Try another password!";
    //    public int MaxAllowedOwnedPasswords { get; set; } = 0;
    //    public const string OwnedPasswordErrorKey = "/OwnedPasswordError";
    //    public OwnedMembershipProvider(IOwnedPasswordRepository ownedPasswordRepository) : base()
    //    {
    //        _ownedPasswordRepository = ownedPasswordRepository;
    //        localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();
    //    }
    //    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    //    {
    //        var ownedPasswordsCount = _ownedPasswordRepository.GetOwnedCount(password);
    //        if (ownedPasswordsCount > MaxAllowedOwnedPasswords)
    //        {
    //            status = MembershipCreateStatus.InvalidPassword;
    //            return (MembershipUser)null;
    //        }
    //        return base.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
    //    }
    //    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    //    {
    //        var ownedPasswordsCount = _ownedPasswordRepository.GetOwnedCount(newPassword);
    //        if (ownedPasswordsCount > MaxAllowedOwnedPasswords)
    //        {
    //            throw new ArgumentException(string.Format(localizationService.GetString(OwnedPasswordErrorKey, DefaultErrorMessage), ownedPasswordsCount));
    //        } 
    //        return base.ChangePassword(username, oldPassword, newPassword);
    //    }

    //}
}
