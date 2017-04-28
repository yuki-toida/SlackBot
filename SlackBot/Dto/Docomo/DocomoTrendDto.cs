namespace SlackBot.Dto.Docomo
{
    public class DocomoTrendDto
    {
        public int TotalResults { get; set; }
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string IssueDate { get; set; }
        public DocomoTrendArticleDto[] ArticleContents { get; set; }
    }

    public class DocomoTrendArticleDto
    {
        public long ContentId { get; set; }
        public int GenreID { get; set; }
        public DocomoTrendContentDto ContentData { get; set; }
    }

    public class DocomoTrendContentDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedDate { get; set; }
        public string SourceDomain { get; set; }
        public string SourceName { get; set; }
    }
}
