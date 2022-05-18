using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_shoppingcart")]
    public class tb_shoppingcart
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int shopid { get; set; }

        public int proid { get; set; }

       public  int num { get; set; }
    }
}
