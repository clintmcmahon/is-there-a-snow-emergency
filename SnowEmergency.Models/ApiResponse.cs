using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SnowEmergency.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("notices")]
        public List<NoticeApiResponse> NoticeApiResponse {get;set;}
    }

    public class NoticeApiResponse
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("noticetype")]
        public string NoticeType { get; set; }

        [JsonPropertyName("publishDate")]
        public string PublishDate { get; set; }
        [JsonPropertyName("expireDate")]
        public string ExpireDate { get; set; }
        [JsonPropertyName("html")]
        public string HTML { get; set; }
        [JsonPropertyName("id")]
        public string IdType { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
