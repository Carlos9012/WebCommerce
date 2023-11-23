using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models.socialMedia
{
    public class seguidores
    {
        [Key]
        public int id_seguidores { get; set; }  
        [ForeignKey("fk_user")]
        public user usuario { get; set; }
        [ForeignKey("fk_seguidor")]  
        public user seguidor { get; set; }

        
    }
}
