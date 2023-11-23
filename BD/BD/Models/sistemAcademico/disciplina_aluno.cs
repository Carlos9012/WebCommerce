using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.sistemAcademico
{
    public class disciplina_aluno
    {
        [Key]
        public int id_disciplina_aluno { get; set; }
        [ForeignKey("fk_curso")]
        public curso curso { get; set; }
        public double nota { get; set; }
        public string status { get; set; }
        
    }
}
