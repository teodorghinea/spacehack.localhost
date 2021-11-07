using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Competitor : BaseEntity
    {
        public string Name { get; set; }
        public int Likes { get; set; }
        public int Followers { get; set; } 
        public string About { get; set; }
        public string Description { get; set; } 
        public string Biography { get; set; }
        public string CompanyOverview { get; set; }
        public string FeaturedPhoto { get; set; }
        public string FeaturedVideo { get; set; }
        public string GeneralInfo { get; set; }
        public string Mission { get; set; }
        public string KeyWords { get; set; }
        public List<FacebookPost> FacebookPosts { get; set; } = new List<FacebookPost>();

    }
}
