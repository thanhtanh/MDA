using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DatingUniversalApp.Models
{

    public class ApiConnector : IDisposable
    {

        public static readonly string ServerPath = "http://localhost:30995/API";

        private static readonly JsonSerializerSettings customJsonSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        protected HttpClient client;
        public static string LoginToken { get; set; }

        public ApiConnector()
        {
            this.client = new HttpClient();
        }

        public async Task<T> Request<T>(string service, string action, List<KeyValuePair<string, string>> parameters)
        {
            var url = string.Format("{0}/{1}/{2}", ServerPath, service, action);

            if (parameters == null)
            {
                parameters = new List<KeyValuePair<string, string>>();
            }

            // Automatically add User-token if available
            if (!string.IsNullOrEmpty(ApiConnector.LoginToken))
            {
                parameters.Add(new KeyValuePair<string, string>("token", ApiConnector.LoginToken));
            }

            var encodedParameter = new FormUrlEncodedContent(parameters);

            var response = await this.client.PostAsync(url, encodedParameter);
            response.EnsureSuccessStatusCode();

            var rawString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(rawString, customJsonSettings);
        }

        public async Task<T> Request<T>(string service, string action, object model)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            var typeInfo = model.GetType().GetTypeInfo();

            foreach (var property in typeInfo.DeclaredProperties)
            {
                var value = property.GetValue(model);

                if (value != null)
                {

                    param.Add(new KeyValuePair<string, string>(
                        property.Name,
                        value.ToString()));
                }
            }

            return await this.Request<T>(service, action, param);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        #region Inner Classes, Interfaces & Enums

        public static class ServerServiceTypes
        {

            public const string HomeService = "Home";
            public const string IdentityService = "Identity";
            public const string CommunityService = "Community";
            public const string ProfileService = "Profile";

        }

        public static class ServerHomeServicePath
        {

            public const string GetPlainInfoLists = "GetPlainInfoLists";

        }

        public static class ServerIdentityServicePath
        {

            public const string LogIn = "LogIn";
            public const string RecoverPassword = "RecoverPassword";
            public const string ResetPassword = "ResetPassword";
            public const string Register = "Register";

        }

        public static class ServerCommunityServicePath
        {

            public const string FriendList = "FriendList";
            public const string StatusUpdate = "StatusUpdate";
            public const string PostStatus = "NewStatusUpdate";

        }

        public static class ServerProfileServicePath
        {

            public const string GetFullProfileInfo = "GetFullProfileInfo";
            public const string UpdateProfile = "UpdateProfile";

        }

        #endregion

    }



}
