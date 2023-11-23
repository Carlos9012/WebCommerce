using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.socialMedia
{
    public class timeline
    {
        [Key]
        public int id_timeline { get; set; }
        [ForeignKey("id_user")]
        public user usuario { get; set; }
        public string descricao { get; set; }
        [ForeignKey("id_foto")]
        public List<foto> foto { get; set; } = new();
        public DateTime date_publicacao { get; set; }
    }
}
