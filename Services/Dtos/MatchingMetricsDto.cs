using System.Collections.Generic;

namespace Services.Dtos
{
    public  class MatchingMetricsDto
    {
        public int Score { get; set; }
        public int OwnMatchingPostCount { get; set; }
        public List<CompetitorWithPosts> Competitors { get; set; } = new List<CompetitorWithPosts>();
    }

    public class CompetitorWithPosts
    {
        public CompetitorDto Competitor { get; set; }
        public List<FacebookPostDto> Posts { get; set; } = new List<FacebookPostDto>();
        public int MatchingPostsCount { get; set; }
        public int CompetitorScore { get; set; }
    }
}
