using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.handbool
{
    public class jogador
    {
        [Key]
        public int id_jogador { get; set; }
        [ForeignKey("id_time")]
        public time time { get; set; }
        public string nome { get; set; }
        public double altura { get; set; }
        public DateTime data_nascimento { get; set; }
        public string genero { get; set; }
    }
}
