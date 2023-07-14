using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    [Index("NewUrl", IsUnique = true)]
    public class UrlInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; } = string.Empty;
        public string NewUrl { get; set; } = string.Empty;
        public int Counter { get; set; }
        public DateTime Expired_at { get; set; }
        public DateTime Created_at { get; set; }
    }
}
