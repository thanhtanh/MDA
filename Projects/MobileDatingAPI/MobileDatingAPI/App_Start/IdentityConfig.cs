using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MobileDatingAPI.Models;
using System.Text;

namespace MobileDatingAPI
{
    public class EmailService : IIdentityMessageService
    {
        private EmailAdpater adapter = new EmailAdpater();

        public Task SendAsync(IdentityMessage message)
        {
            this.adapter.SendEmail(message.Subject, message.Body, message.Destination);

            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            //manager.UserTokenProvider = new CompactUserTokenProvider<ApplicationUser>();

            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class CompactUserTokenProvider<TUser> : IUserTokenProvider<TUser, string> where TUser : class, global::Microsoft.AspNet.Identity.IUser<string>
    {

        // Note: only upper characters are valid
        private static readonly char[] AvailableCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                                                               'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                                                               'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7',
                                                               '8', '9', '0', };
        private const int TokenLength = 5;

        private static Dictionary<string, Dictionary<string, string>> PurposeDictionary;
        protected Random random = new Random();

        public CompactUserTokenProvider()
        {
            PurposeDictionary = new Dictionary<string, Dictionary<string, string>>();
        }

        public Task<string> GenerateAsync(string purpose, UserManager<TUser, string> manager, TUser user)
        {
            if (!PurposeDictionary.ContainsKey(purpose))
            {
                PurposeDictionary.Add(purpose, new Dictionary<string, string>());
            }

            var result = new Task<string>(() =>
            {
                var tokenDictionary = PurposeDictionary[purpose];
                var token = this.GenerateToken();

                tokenDictionary.Add(token, user.Id);

                return token;
            });

            result.Start();

            return result;
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<TUser, string> manager, TUser user)
        {
            var result = new Task<bool>(() => { return true; });

            return result;
        }

        public Task NotifyAsync(string token, UserManager<TUser, string> manager, TUser user)
        {
            var result = new Task(() => { });

            result.Start();

            return result;
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser, string> manager, TUser user)
        {
            var result = new Task<bool>(() =>
            {
                if (!PurposeDictionary.ContainsKey(purpose)) { return false; }

                var purposeDict = PurposeDictionary[purpose];

                if (!purposeDict.ContainsKey(token.ToUpper())) { return false; }

                return purposeDict[token].Equals(user.Id);
            });

            result.Start();

            return result;
        }

        protected string GenerateToken()
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < TokenLength; i++)
            {
                result.Append(AvailableCharacters[random.Next(AvailableCharacters.Length)]);
            }

            return result.ToString();
        }
    }
}
