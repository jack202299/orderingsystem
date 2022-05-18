using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_sendtask")]
    public class tb_sendtask
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string tomail { get; set; }
        public string createdate { get; set; }
        public bool blfage  { get; set; }
    }
}
