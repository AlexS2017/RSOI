using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
