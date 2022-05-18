using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_kinds")]
    public class tb_kinds
    {

        public int id { get; set; }
        /// <summary>
        /// 行业类别
        /// </summary>
        public string kindsname { get; set; }

       
    }
}
