using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceMessages
{
    public class AddUserProfileMsg : GetUserProfileMsg
    {
        public Guid Id { get; set; }
    }

    public class GetUserProfileMsg
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
