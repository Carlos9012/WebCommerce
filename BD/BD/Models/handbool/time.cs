using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.handbool
{
    public class time
    {
        [Key]
        public int id_time { get; set; }
        [ForeignKey("id_jogador")]
        public jogador capitao { get; set; }
        [ForeignKey("id_estadio")]
        public estadio estadio { get; set; }
        public int vitorias { get; set; }
        public int gols { get; set; }
    }
}
