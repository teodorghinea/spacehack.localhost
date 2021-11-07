using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class LightFacebookPostDto
    {

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public int Shares { get; set; }
        public string MediaFile { get; set; }
        public string MediaType { get; set; }
    }
}
