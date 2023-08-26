using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    [Index(nameof(SHORT_SLUG), IsUnique = true)]
    public class Url
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string ORIGINAL_URL { get; set; } = string.Empty;
        public string SHORT_SLUG { get; set; } = string.Empty;
        public int MAX_CLICKS { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EXPIRED_AT { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CREATED_AT { get; set; }
        [ForeignKey(nameof(USER))]
        public int USER_ID { get; set; }
        public User? USER { get; set; }
    }
}
