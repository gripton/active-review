using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

using ARR.Data.Entities;

using PracticalCode.WebSecurity.Infrastructure.Data;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class WebSecurityDataProvider : IWebSecurityDataProvider
    {
        public WebSecurityUser GetUser(string username)
        {
            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiUrl"]) };

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var user = new WebSecurityUser();

            var response = client.GetAsync("/api/account/" + username).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var account = response.Content.ReadAsAsync<Account>().Result;
                user = AutoMapper.Mapper.Map(account, user);
            }

            return user;
        }

        public void UpdateUser(WebSecurityUser user)
        {
            const string format = "/api/account/{0}/security";

            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiUrl"]) };

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var account = AutoMapper.Mapper.Map<Account>(user);
            var response = client.PutAsJsonAsync(string.Format(format, account.Id), account).Result;

        }

        public void CreateUser(WebSecurityUser user)
        {
            throw new NotImplementedException();
        }

        public string DataFilePath
        {
            get{throw new NotImplementedException();}
            set{throw new NotImplementedException();}
        }

        public void DeleteUser(WebSecurityUser user)
        {
            throw new NotImplementedException();
        }

        public WebSecurityOrganization GetOrganization(string name)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string username)
        {
            throw new NotImplementedException();
        }

        

        
    }
}