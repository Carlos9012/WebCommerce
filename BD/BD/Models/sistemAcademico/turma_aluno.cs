using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.sistemAcademico
{
    public class turma_aluno
    {
        [Key]
        public int id_turma_aluno { get; set; }
        [ForeignKey("id_aluno")]
        public aluno aluno { get; set; }
        [ForeignKey("id_turma")]
        public turma turma { get; set;}
    }
}
