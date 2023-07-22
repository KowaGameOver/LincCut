namespace LincCut.Mocks
{
    public class UrlInfoDto
    {
        public string Url { get; set; } = string.Empty;
        public string NewUrl { get; set; } = string.Empty;
        public int Counter { get; set; }
        public DateTime Expired_at { get; set; }
        public DateTime Created_at { get; set; }
    }
}
