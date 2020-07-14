using CAL_Transactions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CAL_Transactions.Services
{
    public class MerchantsService
    {
        private string baseURL;
        private string token;

        public MerchantsService(string accessToken)
        {
            baseURL = ConfigurationManager.AppSettings["CAL_API_BASEURL"].ToString();
            if(!string.IsNullOrWhiteSpace(accessToken) && accessToken.StartsWith("Bearer ") && accessToken.Length > 7)
            {
                token = accessToken.Split(new string[] { "Bearer " }, StringSplitOptions.None)[1];
            }
        }

        private async Task<TransactionsResponse> getRawTransactions()
        {
            string urlParameters = baseURL + "/Transactions/GetTransactions";
            using (HttpClient client = new HttpClient())
            {
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                // Add Bearer token to request
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                // Get external API response
                HttpResponseMessage response = await client.GetAsync(urlParameters);
                if (response.IsSuccessStatusCode)
                {
                    var merchantTransactions = await response.Content.ReadAsAsync<TransactionsResponse>();
                    return merchantTransactions;
                }
            }
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }

        public async Task<IEnumerable<MerchantTotalTransactions>> GetTotalTransactions()
        {
            var merchantTransactions = await getRawTransactions();
            return merchantTransactions.transactions.GroupBy(tran => tran.merchantName)
                .Select(merch => new MerchantTotalTransactions()
                {
                    MerchantName = merch.Key,
                    TotalTransactions = merch.Sum(item => item.amount)
                });
        }

        public async Task<IEnumerable<MonthlyPayments>> SaveMonthlyPayments()
        {
            var merchantTransactions = await getRawTransactions();
            var payments = merchantTransactions.transactions.GroupBy(tran => tran.debitDate)
                .Select(monthly => new MonthlyPayments()
                {
                    debitDate = monthly.Key,
                    totalPayment = monthly.Sum(item => item.amount)
                }).ToList();
            using (HttpClient client = new HttpClient())
            {
                string urlParameters = baseURL + "/SaveMonthlyTransactions";
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                // Add Bearer token to request
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                // Save data using external API
                HttpResponseMessage response = await client.PostAsJsonAsync<IEnumerable<MonthlyPayments>>(urlParameters, payments);
                
                if (response.IsSuccessStatusCode)
                {
                    return payments;
                }
            }
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}