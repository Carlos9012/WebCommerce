using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.handbool
{
    public class partida
    {
        [Key]
        public int id_partida { get; set; }
        public time visitante { get; set; }
        public time anfitriao { get; set; }
        public DateTime data { get; set; }
        [ForeignKey("id_estadio")]
        public estadio estadio { get; set; }

    }
}
