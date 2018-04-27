using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common
{
    public class TextMessage
    {
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
