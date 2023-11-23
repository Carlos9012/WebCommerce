using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.sistemAcademico
{
    public class disciplina_curso
    {
        [Key]
        public int id_disciplina_curso { get; set; }
        [ForeignKey("id_curso")]
        public curso curso { get; set; }
        public string nome_disciplina { get; set; }
    }
}
