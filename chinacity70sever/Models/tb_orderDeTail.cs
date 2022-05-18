using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_orderdetail")]
    public class tb_orderdetail
    {
        public int id { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int orderid { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public int productID { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int quantity { set; get; }

        /// <summary>
        /// 折扣ID
        /// </summary>
        public string? discount { get; set; }

        public string? discountname { set; get; }
        /// <summary>
        /// 上次的价格
        /// </summary>
        public decimal? lastprice { set; get; }
    }
}
