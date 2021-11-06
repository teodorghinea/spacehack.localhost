using DataLayer.Entities.Enums;
using System;

namespace Services.Dtos
{
    public class FacebookPostAddDto
    {
        public string Content { get; set; }
        public string MediaFile { get; set; }
        public MediaType MediaType { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public int Shares { get; set; }
        public int Reactions { get; set; }
        public string Url { get; set; }
        public string Date { get; set; }
        public Guid CompetitorId { get; set; }
    }
}
