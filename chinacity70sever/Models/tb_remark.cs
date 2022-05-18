using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{

    [Table("tb_remark")]
    public class tb_remark
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string images   { get; set; }

    }
}
