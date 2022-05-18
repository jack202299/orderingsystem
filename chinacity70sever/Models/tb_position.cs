using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_position")]
    public class tb_position
    {
        public int id { get; set; }
        public string position { get; set; }
        public int shopid { get; set; }

    }
}
