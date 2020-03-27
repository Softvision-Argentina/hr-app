using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.User
{
    public class SuccessAuthData
    {
        public string Token { get; set; }
        public ReadedUserViewModel User { get; set; }
    }
}
