using CAL_Transactions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace CAL_Transactions.Services
{
    public class LoginService
    {
        private string baseURL;

        public LoginService()
        {
            baseURL = ConfigurationManager.AppSettings["CAL_API_BASEURL"].ToString();
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                string urlParameters = baseURL + "/Authenticate";

                // Get external API response
                HttpResponseMessage response = await client.PostAsJsonAsync(urlParameters, loginRequest);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<LoginResponse>();
                }
            }
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}