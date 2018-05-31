﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceMessages
{
    public class GetActionStatMsg
    {
        public DateTime DateCreated { get; set; }

        public Guid UserId { get; set; }

        public string UserInfo { get; set; }

        public string Action { get; set; }

        public Guid? EntityId { get; set; }
    }
}