using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_clientage")]
    public class tb_clientage
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int shopid { get; set; }
    }
}
