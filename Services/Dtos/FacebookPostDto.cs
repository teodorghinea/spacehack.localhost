using System;

namespace Services.Dtos
{
    public  class FacebookPostDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string MediaFile { get; set; }
        public string MediaType { get; set; }
        public string Url { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public int Shares { get; set; }
        public int Reactions { get; set; }
    }
}
