using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_orders")]
    public class tb_orders
    {
        public int id { get; set; } 
        /// <summary>
        /// 订单名称
        /// </summary>
        public string ordername { get; set; }
        /// <summary>
        /// 商店ID
        /// </summary>
        public int shopid { get; set; }
        /// <summary>
        /// 订货人ID
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 收货地址ID
        /// </summary>
        public int shipsid { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? createdate { get; set; }
        /// <summary>
        /// 0=等出货，1=已出货 2=作废
        /// </summary>
        public int blfalge { set; get; } = 0;

        public string Remark { get; set; }  

        public decimal meney { get; set; }
        /// <summary>
        /// 产品总数
        /// </summary>
        public int productcount { get; set; }
        /// <summary>
        /// 支付状态 100=未支付，101=已支付
        /// </summary>
        public int paystatus { get; set; }
        /// <summary>
        /// 支付方式1=现金，2=银行卡
        /// </summary>
        public int paymethod { get; set; }
        /// <summary>
        /// 1=零售单,2=批发单
        /// </summary>
        public int ordertype { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        public decimal? actuallyreceived { get; set; }
    }
}
