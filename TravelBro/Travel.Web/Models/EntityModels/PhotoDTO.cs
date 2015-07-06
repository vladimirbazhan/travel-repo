using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public class PhotoDTO
    {
        public PhotoDTO(Photo p)
        {
            Published = p.Published;
            ImagePath = p.ImagePath;
            IsMain = p.IsMain;
            AuthorId = p.Author.Id;
        }

        public DateTime Published { get; set; }
        public string ImagePath { get; set; }
        public bool IsMain { get; set; }
        public string AuthorId { get; set; }

    }
}