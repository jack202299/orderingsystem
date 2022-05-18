namespace chinacity70sever.Models
{
    public class tb_shoptype
    {
        public int id { get; set; }
        public string typename { get; set; }
        /// <summary>
        /// 店铺商品数量上限
        /// </summary>
        public int icount { get; set; }
        /// <summary>
        /// 每月价格
        /// </summary>
        public int price { get; set; }



    }
}
