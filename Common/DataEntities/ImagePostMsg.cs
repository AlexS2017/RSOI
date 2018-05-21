using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common
{
    public class ImagePostMsg
    {
        [Key] //attribute
        public Guid Id { get ; set; }

        public string ImageTitle { get; set; }

        public string Description { get; set; }

        public string HashTag { get; set; }

        public DateTime DateCreated { get; set; }

        public decimal AverageRate { get; set; }

        public byte[] Image { get; set; }

        public Guid UserId { get; set; }
    }
}
