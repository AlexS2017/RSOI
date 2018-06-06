using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CommonCode
{
    public class TokenInfo
    {
        public string sub { get; set; }

        public Guid UserId { get { return Guid.Parse(sub); } }

        public string Email { get; set; }
    }
}
