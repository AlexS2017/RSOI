using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DataEntities
{
    public class UserAction
    {
        [Key] 
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid UserProfileId { get; set; }

        public string UserInfo { get; set; }

        public string Action { get; set; }

        public Guid? EntityId { get; set; }

        public string Client { get; set; } = "public_api";
    }
}
