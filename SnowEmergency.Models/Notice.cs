using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SnowEmergency.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string NoticeType { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Body { get; set; }
        public string IdType { get; set; }
        public string Version { get; set; }
    }
}
