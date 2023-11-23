
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.handbool
{
    public class historico_partidas
    {
        [Key]
        public int id_historico { get; set; }
        [ForeignKey("id_partida")]
        public partida partida { get; set; }
        public string status { get; set; }
    }
}
