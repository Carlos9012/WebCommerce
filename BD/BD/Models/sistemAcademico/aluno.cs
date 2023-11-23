using System.ComponentModel.DataAnnotations;

namespace BD.Models.sistemAcademico
{
    public class aluno
    {
        [Key]
        public int id_aluno { get; set; }
        public string name { get; set; }
        public int cpf { get; set; }
        public DateTime data { get; set; }

    }
}
