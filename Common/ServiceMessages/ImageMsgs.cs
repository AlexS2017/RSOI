using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceMessages
{
    public class AddImageMsg
    {
        public string ImageTitle { get; set; }

        public string Description { get; set; }

        public string HashTag { get; set; }

        public byte[] Image { get; set; }
    }

    public class AddImageCommentMsg
    {
        public string Comment { get; set; }

        public int Rate { get; set; }

        public Guid ImageId { get; set; }
    }
}
