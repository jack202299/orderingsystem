using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class ShoppingcartController : Controller
    {
        private readonly china70Context context1;
        public ShoppingcartController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// add products to shopping cart
        /// </summary>
        /// <param name="shoppingcart"></param>
        /// <returns></returns>
        [HttpPost,Route("Shoppingcart/ShopCartAdd")]
        [LimitAttr]
        [Auther]
        public IActionResult ShopCartAdd([FromBody]tb_shoppingcart shoppingcart)
        {
            var data = context1.tb_shoppingcarts.FirstOrDefault(x => x.userid == shoppingcart.userid && x.proid == shoppingcart.proid && x.shopid == shoppingcart.shopid);
            if (data == null) 
                 context1.tb_shoppingcarts.Add(shoppingcart);
            else
            {
                data.num= data.num+shoppingcart.num;
                context1.tb_shoppingcarts.Update(data);
            }
               
            var irows=context1.SaveChanges();
            if (irows > 0) return Json(new { code = 0, msg = "已加入购物车，请到购物车查看" });
            return Json(new { code = 1, msg = "保存失败" });
        }

        /// <summary>
        /// get to the product list from shopping cart with user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet,Route("Shoppingcart/QueryProductShopcarts")]
        [LimitAttr]
        public IActionResult QueryProductShopcarts(int UserId)
        {
            //var tepdata=from p in context1.tb_shoppingcarts join r in context1.tb_Products on p.proid equals r.id
            //            join x in context1.tb_shop on p.shopid equals x.id
            //            where p.userid==UserId

            var temp=(from p in context1.tb_shop where  (
                from x in context1.tb_shoppingcarts
                where x.userid == UserId select x.shopid).Contains(p.id)
                select p).ToList();
            List<dynamic> products = new List<dynamic>();
            foreach(var item in temp)
            {
                var tempdata = (from p in context1.tb_shoppingcarts
                                join x in context1.tb_Products
                                on p.proid equals x.id
                                where p.shopid == item.id
                                select new
                                {
                                    p.id,
                                    p.num,
                                    x.unit,
                                    proid = x.id,
                                    images =string.Concat( dbManager.Gethosturl("hosturl"), x.images),
                                    x.price,
                                    x.productName
                                }).ToList();
                int meney = 0;
                foreach (var k in tempdata)
                {
                    meney += k.num * (int)k.price;
                }
                var kk = new { shopid = item.id, shopcd = item.shopname, meny = meney, plist = tempdata };
                products.Add(kk);

            }
            
            
            return Json(new { products });
        }
        /// <summary>
        /// delete product in shopping cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Shoppingcart/DelProductShopcart")]
        [Auther]
        [LimitAttr]
        public IActionResult DelProductShopcart(int id)
        {
            var data=context1.tb_shoppingcarts.FirstOrDefault(x=>x.id==id);
            context1.tb_shoppingcarts.Remove(data);
            var irows=context1.SaveChanges();
            if (irows > 0) return Json(new { code = 0, msg = "删除成功" });
            else return Json(new { code = 1, msg = "删除失败" });

        }
        /// <summary>
        /// update the num
        /// </summary>
        /// <param name="shopcartparam"></param>
        /// <returns></returns>
        [HttpPost,Route("Shoppingcart/UpateProductNum")]
        [Auther]
        [LimitAttr]
        public IActionResult UpateProductNum([FromBody]productShopcartparam shopcartparam)
        {
            var data = context1.tb_shoppingcarts.FirstOrDefault(x => x.id == shopcartparam.Id);
            data.num=shopcartparam.num;
            var irows=context1.SaveChanges();
            return Json(new { code = 0, msg = "ok" });
        }
    }
}
