using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_producttype")]
    public class tb_producttype
    {
        public int id { get; set; }
        public string protype { get; set; }
        public int kindsid { get; set; }
    }
}
