
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.biblioteca
{
    public class emprestimo
    {
        [Key]
        public int id_emprestimo { get; set; }
        public DateTime data { get; set; }
        public double valor { get; set; }
        public string descricao { get; set; }

        [ForeignKey("id_livro")]
        public livro livro { get; set; }
    }
}
