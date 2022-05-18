using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_product")]
    public class tb_product
    {
        public int id { get; set; }
      
        /// <summary>
        /// 商店ID
        /// </summary>
        public int? shopid { get; set; }
        /// <summary>
        /// 图像列表
        /// </summary>
        public string? images { get; set; }
        /// <summary>
        /// 产品称
        /// </summary>
        public string? productName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string? alias { set; get; }


        /// <summary>
        /// 价格
        /// </summary>
        public decimal? price { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string? unit { get; set; }
        /// <summary>
        /// 是否下架 0=下架，1=上架
        /// </summary>
        public int? isdown { get; set; }

        public bool? hotsort { get; set; }

        public int? sellcount { get; set; }

        public string? createdate { get; set; }

        public int createby { get; set; }

        public int? productType { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string? barcode { get; set; }
        /// <summary>
        /// 货号
        /// </summary>
        public string? productId { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int? reserve { get; set; }
        /// <summary>
        /// 批价
        /// </summary>
        public decimal? bulkprice { get; set; }
        /// <summary>
        /// 仓位id
        /// </summary>
        public int? positionid { get; set; }
      

    }
}
