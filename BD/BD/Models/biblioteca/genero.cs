using System.ComponentModel.DataAnnotations;

namespace BD.Models.biblioteca
{
    public class genero
    {
        [Key]
        public int id_genero { get; set; }
        public string nome { get; set;}
    }
}
