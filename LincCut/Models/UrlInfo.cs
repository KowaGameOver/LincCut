using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    public class UrlInfo
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string NewUrl { get; set; } = string.Empty;
        public int Counter { get; set; }
    }
}
