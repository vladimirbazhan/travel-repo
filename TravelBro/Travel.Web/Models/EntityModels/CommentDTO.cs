﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class CommentDTO
    {
        public CommentDTO(Comment c)
        {
            Id         = c.Id;
            Published  = c.Published;
            Text       = c.Text;
            AuthorId   = c.Author.Id;
            Author     = new ApplicationUserDTO(c.Author);
            ReplyToId  = c.ReplyTo == null ? 0 : c.ReplyTo.Id;
        }

        public int    Id { get; set; }
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public int    ReplyToId { get; set; }

        public DateTime           Published { get; set; }
        public ApplicationUserDTO Author { get; set; }
    }
}