using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public enum LanguageEnum : int
    {
        English = 1,
        Russian = 2,
    }

    public class Language : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }

        public string Name { get; set; }
    }
}