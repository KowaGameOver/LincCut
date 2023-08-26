namespace LincCut.Mocks
{
    public class UrlInfoDto
    {
        public string ORIGINAL_URL { get; set; } = string.Empty;
        public string SHORT_SLUG { get; set; } = string.Empty;
        public int MAX_CLICKS { get; set; }
        public DateTime EXPIRED_AT { get; set; }
        public DateTime CREATED_AT { get; set; }
    }
}
