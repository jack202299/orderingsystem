using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_shopuser")]
    public class tb_shopuser
    {
       public int id { get; set; }

       public int shopid { get; set; }

       public int userid { get; set; }
    }
    

}
