using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LincCut.Models
{
    public class Click
    {
        [Key]
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        [ForeignKey("UrlInfos")]
        public int UrlInfo_id { get; set; }
        public UrlInfo UrlInfos { get; set; }
        public string Language { get; set; } = string.Empty;
    }
}
