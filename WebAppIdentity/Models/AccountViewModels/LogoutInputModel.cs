using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Models.AccountViewModels
{
    public class LogoutInputModel
    {
        public string LogoutId { get; set; }

        public string ReturnUrl { get; set; }
    }
}
