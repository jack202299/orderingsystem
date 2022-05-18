using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_loginlog")]
    public class tb_loginlog
    {
        public int id   { get; set; }
        public string guid { get; set; }

        public int   userid { get; set; }

        public DateTime createdate { get; set; }
        /// <summary>
        /// 时间秒
        /// </summary>
        public int express { get; set; }
    }
}
