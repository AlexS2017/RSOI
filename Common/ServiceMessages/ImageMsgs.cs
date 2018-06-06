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

        public Guid UserId { get; set; }

        public string Client { get; set; }
    }

    public class AddImageResponse
    {
        public bool IsSuccess { get; set; }

        public Guid? ImageId { get; set; }
    }

    public class GetImageMsg : AddImageMsg
    {
        public DateTime DateCreated { get; set; }

        public decimal AverageRate { get; set; }
    }

    public class GetHomeImageMsg
    {
        public string ImageTitle { get; set; }

        public string Description { get; set; }

        public string HashTag { get; set; }

        public string Message { get; set; }

        public List<Guid> ImageList { get; set; }
    }

    public class HomePageMsg
    {
        public string ErrorMessage { get; set; }

        public string Message { get; set; }
    }

    public class AddImageCommentMsg
    {
        public string Comment { get; set; }

        public int Rate { get; set; }

        public decimal AvgRate { get; set; }

        public Guid ImageId { get; set; }

        public string AllCommentsInfo { get; set; }

        public Guid UserId { get; set; }

        public string Client { get; set; }
    }

    public class AddImageCommentResponse
    {
        public bool IsSuccess { get; set; }

        public Guid? CommentId { get; set; }
    }

    public class GetComments
    {
        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public string User { get; set; }

        public Guid UserId { get; set; }
    }

    public class ImagePageInfo
    {
        public int Rate { get; set; }

        public decimal AvgRate { get; set; }

        public List<GetComments> Comments { get; set; }
    }

    public class GetImgInfoMsg
    {
        public Guid ImageId { get; set; }

        public Guid? UserId { get; set; }
    }

    public class SaveImgRating :  GetImgInfoMsg
    {
        public int Rate { get; set; }
    }
}
