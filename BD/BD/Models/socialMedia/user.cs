using System.ComponentModel.DataAnnotations;

namespace BD.Models.socialMedia
{
    public class user
    {
        [Key]
        public int id_user { get; set; }
        public string nome { get; set; }
        public DateTime data_nascimento { get; set; }
        public int numero { get; set; }
        public string genero { get; set; }
        public string username { get; set;}
    }
}
