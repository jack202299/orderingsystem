using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_ships")]
    public class tb_ships
    {
        public int id { set; get; }

        public int userid { set; get; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string? shipaddress { set; get; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string? shipperson { set; get; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string? shipTel { set; get; }

        public string company { set; get; }

        public string TaxID { set; get; }

        public bool ? isdefault { set; get; }
    }
}
