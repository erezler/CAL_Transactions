using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAL_Transactions.Models
{
    public class LoginResponse
    {
        public int id;
        public string username;
        public string password;
        public string token;
        public DateTime insertDate;
        public DateTime lastLogin;
    }
}