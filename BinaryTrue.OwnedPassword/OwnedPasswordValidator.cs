using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNet.Identity;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Framework.Localization;

namespace BinaryTrue.OwnedPassword
{
public class OwnedPasswordValidator: PasswordValidator
{
    private readonly LocalizationService localizationService;
    private readonly IOwnedPasswordRepository _ownedPasswordRepository;
    public OwnedPasswordValidator(IOwnedPasswordRepository ownedPasswordRepository) :base()
    {
        _ownedPasswordRepository = ownedPasswordRepository;
        localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();
    }
    private ILogger _log = LogManager.Instance.GetLogger(typeof(OwnedPasswordValidator).ToString());
    public string BaseUrl { get; set; } = "https://api.pwnedpasswords.com/range/";
    public const string DefaultErrorMessage = "Your password occurs in hacked databases {0} times. Try another password!";
    public int MaxAllowedOwnedPasswords { get; set; } = 0;
    public const string OwnedPasswordErrorKey = "/OwnedPasswordError";
    static HttpClient client = new HttpClient();
    public override Task<IdentityResult> ValidateAsync(string password)
    {
        IdentityResult resultToReturn = IdentityResult.Success;
        var baseResult = base.ValidateAsync(password).Result;
        if(baseResult.Succeeded)
        {
            try
            {
                var ownedPasswordsCount = _ownedPasswordRepository.GetOwnedCount(password);
                if (ownedPasswordsCount > MaxAllowedOwnedPasswords)
                {
                    resultToReturn = IdentityResult.Failed(string.Format(localizationService.GetString(OwnedPasswordErrorKey, DefaultErrorMessage), ownedPasswordsCount));
                }
            }
            catch(Exception ex)
            {
                _log.Error("Failed to call owned passwords service.",ex);
            }
        }
        else
        {
            resultToReturn = baseResult;
        }
        return Task.FromResult(resultToReturn);
    }
}
    public class OwnedPasswordRepository : IOwnedPasswordRepository
    {
        static HttpClient client = new HttpClient();
        public string BaseUrl { get; set; } = "https://api.pwnedpasswords.com/range/";
        public int GetOwnedCount(string password)
        {
            var hashedPassword = Hash(password);
            var searchResultsString = client.GetStringAsync(BaseUrl + hashedPassword.Substring(0, 5)).Result;
            var resultsArray = searchResultsString.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            var key = hashedPassword.Substring(5);
            foreach (var resultString in resultsArray)
            {
                var values = resultString.Split(':');
                if (key == values[0])
                {
                    var ownedPasswords = Int32.Parse(values[1]);
                    return ownedPasswords;
                }
            }
            return 0;
        }
        public static string Hash(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
