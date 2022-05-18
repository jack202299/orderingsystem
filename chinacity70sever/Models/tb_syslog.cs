using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
      [Table("tb_syslog")]
    public class tb_syslog
    {
        public int id { get; set; }
        public string createdate    { get; set; }
        public string fun { get; set; }
        public string errmsg { get; set; }
    }
}
