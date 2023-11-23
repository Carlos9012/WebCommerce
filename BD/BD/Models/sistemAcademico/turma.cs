using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.sistemAcademico
{
    public class turma
    {
        [Key]
        public int id_turma { get; set; }
        [ForeignKey("id_curso")]
        public curso curso { get; set; }
        public int referencia_turma { get; set; }
        public DateTime data_inicio { get; set; }
        public int vagas { get; set; }
    }
}
