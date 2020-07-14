using CAL_Transactions.Models;
using CAL_Transactions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CAL_Transactions.Controllers
{
    public class TransactionsController : ApiController
    {
        [Route("api/Login")] 
        [HttpPost] 
        public async Task<LoginResponse> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var service = new LoginService(); // I didn't use dependency injection for lack of time. I would have used Unity here.
            return await service.Login(loginRequest);
        }

        [Route("api/GetMerchantsTransactions")]
        [HttpGet]
        public async Task<IEnumerable<MerchantTotalTransactions>> GetMerchantsTransactions()
        {
            string accessToken = HttpContext.Current.Request.Headers["Authorization"];

            var service = new MerchantsService(accessToken);
            return await service.GetTotalTransactions();
        }

        [Route("api/SaveMonthlyPayments")]
        [HttpPost]
        public async Task<IEnumerable<MonthlyPayments>> SaveMonthlyPayments()
        {
            string accessToken = HttpContext.Current.Request.Headers["Authorization"];

            var service = new MerchantsService(accessToken);
            return await service.SaveMonthlyPayments();
        }
    }
}
