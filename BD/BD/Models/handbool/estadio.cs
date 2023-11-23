using System.ComponentModel.DataAnnotations;

namespace BD.Models.handbool
{
    public class estadio
    {
        [Key]
        public int id_estadio { get; set; }
        public string name { get; set; }
        public string endereco { get; set; }
    }
}
