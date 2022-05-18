using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_shop")]
    public class tb_shop
    {
        public int id { get; set; }
        public int userid { get; set; }

        public string shopname { get; set; }

        public string shopaddress { get; set; }

        public string? tel { get; set; }

        public string? person { get; set; }
        /// <summary>
        /// 剩余天数
        /// </summary>
        public int usdays { get; set; }

        public string lastDate { get; set; }

        public int kindid { get; set; }

        public int shoptypid { get; set; }

        public string? shopimg { get; set; }

        public bool? blfage { get; set; }
        public string? bankaccount {set;get;}
    }
}
