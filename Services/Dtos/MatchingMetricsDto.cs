using System.Collections.Generic;

namespace Services.Dtos
{
    public  class MatchingMetricsDto
    {
        public decimal Score { get; set; }
        public int OwnMatchingPostCount { get; set; }
        public List<string> PostKeywords { get; set; } = new List<string>();
        public List<CompetitorWithPosts> Competitors { get; set; } = new List<CompetitorWithPosts>();
    }

    public class CompetitorWithPosts
    {
        public CompetitorDto Competitor { get; set; }
        public List<MetricsFacebookPost> Posts { get; set; } = new List<MetricsFacebookPost>();
        public int MatchingPostsCount { get; set; }
        public decimal CompetitorScore { get; set; }
        public List<string> ArticlesKeywords { get; set; } = new List<string>();
    }
}
