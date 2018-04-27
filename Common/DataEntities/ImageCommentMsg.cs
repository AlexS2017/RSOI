using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common
{
    public class ImageCommentMsg
    {
        [Key] 
        public Guid Id { get; set; }

        public string Comment { get; set; }

        public DateTime DateCreated { get; set; }

        public int Rate { get; set; }

        public Guid ImagePostMsgId { get; set; }

        public ImagePostMsg ImagePostMsg { get; set; }
    }
}
