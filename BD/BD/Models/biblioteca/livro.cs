using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.biblioteca
{
    public class livro
    {
        [Key]
        public int id_livro { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string editora { get; set; }
        public int n_pag { get; set; }
        public double valor_multa { get; set; }
        [ForeignKey("id_genero")]
        public genero genero { get; set; }
        [ForeignKey("id_user")]
        public user user { get; set; }
    }
}
