using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceMessages
{
    public class LoginResponse
    {
        public TokenData Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }

    public class TokenData
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
    }
}
