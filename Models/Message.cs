using FileKeyReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChatManager.Models
{
    public class Message
    {
        public Message()
        {
            WrittenDate = DateTime.Now;
        }

        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        [Display(Name = "Date d'écriture")]
        [DataType(DataType.Date)]
        public DateTime WrittenDate { get; set; }
        public string Content { get; set; }
    }
}
