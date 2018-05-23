using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DataEntities
{
    public class ImageRating
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public int Rate { get; set; }

        public Guid ImageId { get; set; }

        public Guid UserId { get; set; }
    }
}
