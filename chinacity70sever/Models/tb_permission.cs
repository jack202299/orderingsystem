using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_permission")]
    public class tb_permission
    {
        public int id { get; set; }
        public string menuid { get; set; }
        public bool is_active { get; set; }

        public int userid { get; set; }
    }
}
