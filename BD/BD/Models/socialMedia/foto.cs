using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.socialMedia
{
    public class foto
    {
        [Key]
        public int id_foto { get; set; }
        [ForeignKey("id_user")]
        public user user { get; set; }
        [ForeignKey("id_album")]
        public album album { get; set; }
        public string url { get; set; }
    }
}
