using System;
using System.Net.Http;
using System.Net.Http.Headers;

using ARR.Data.Entities;

using PracticalCode.WebSecurity.Infrastructure.Data;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class WebSecurityDataProvider : IWebSecurityDataProvider
    {
        public WebSecurityDataProvider()
        {

        }

        public void CreateUser(WebSecurityUser user)
        {
            throw new NotImplementedException();
        }

        public string DataFilePath
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteUser(WebSecurityUser user)
        {
            throw new NotImplementedException();
        }

        public WebSecurityOrganization GetOrganization(string name)
        {
            throw new NotImplementedException();
        }

        public WebSecurityUser GetUser(string username)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49882/");

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
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49882/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var account = AutoMapper.Mapper.Map<Account>(user);             

            string format = "/api/account/{0}/security";

            var response = client.PutAsJsonAsync<Account>(string.Format(format, account.Id), account).Result;
            
        }

        public bool UserExists(string username)
        {
            throw new NotImplementedException();
        }
    }
}