using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_menu")]
    public class tb_menu
    {
        public int id { get; set; }

        public string menuguid { get; set; }

        public string menuname { get; set; }

        public int partid { get; set; }
    }
}
