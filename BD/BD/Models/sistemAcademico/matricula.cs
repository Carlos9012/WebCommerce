using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.sistemAcademico
{
    public class matricula
    {
        [Key]
        public int id_matricula { get; set; }
        [ForeignKey("id_aluno")]
        public aluno aluno { get; set; }
        public float media_global_curso { get; set; }
    }
}
