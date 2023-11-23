using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.socialMedia
{
    public class album
    {
        [Key]
        public int id_album { get; set; }
        public string nome { get; set; }
        [ForeignKey("id_user")]
        public user user { get; set; }
    }
}
