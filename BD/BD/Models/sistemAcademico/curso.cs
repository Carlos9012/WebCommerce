using System.ComponentModel.DataAnnotations;

namespace BD.Models.sistemAcademico
{
    public class curso
    {
        [Key]
        public int id_curso {  get; set; }
        public string nome { get; set;}
        public DateTime carga_h { get; set; }
        public double valor { get; set; }
    }
}
