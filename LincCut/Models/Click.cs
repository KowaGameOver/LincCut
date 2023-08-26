using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LincCut.Models
{
    public class Click
    {
        [Key]
        public int ID { get; set; }
        public string IP { get; set; } = string.Empty;
        public string BROWSER { get; set; } = string.Empty;
        [ForeignKey(nameof(URL))]
        public int URL_INFO_ID { get; set; }
        public Url? URL { get; set; }
    }
}
