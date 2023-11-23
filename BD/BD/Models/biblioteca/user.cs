using System.ComponentModel.DataAnnotations;

namespace BD.Models.biblioteca
{
    public class user
    {
        [Key]
        public int id_user {  get; set; }
        public string name { get; set; }
    }
}
