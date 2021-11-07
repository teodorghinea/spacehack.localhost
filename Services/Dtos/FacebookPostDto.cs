using System;

namespace Services.Dtos
{
    public  class FacebookPostDto : LightFacebookPostDto
    {
        public string KeyWords { get; set; }
        public string Url { get; set; }
        public int Reactions { get; set; }
        public Guid? CompetitorId { get; set; }
    }
}
