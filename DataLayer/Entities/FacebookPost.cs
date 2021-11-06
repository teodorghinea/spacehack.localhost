using DataLayer.Entities.Enums;
using System;

namespace DataLayer.Entities
{
    public class FacebookPost : BaseEntity
    {
        public int PostNumber { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string MediaFile { get; set; }
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public int Shares { get; set; }
        public int Reactions { get; set; }
    }
}
