using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class ShopTypeController : Controller
    {
        private readonly china70Context context1;
        public ShopTypeController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// add or edit shop type
        /// </summary>
        /// <param name="shoptype"></param>
        /// <returns></returns>
        [HttpPost,Route("ShopType/AddOrEditShopType")]
        [LimitAttr]
        [Auther]
        public IActionResult AddOrEditShopType([FromBody]tb_shoptype shoptype)
        {
            if (shoptype == null) return Json(new { code = 504, msg = "参数不能为空" });
            if (shoptype.id == 0) context1.tb_shoptype.Add(shoptype);
            else context1.tb_shoptype.Update(shoptype);
            var irows = context1.SaveChanges();
            if (irows > 0) return Json(new { code = 0, msg = "保存成功" });
            return Json(new { code = 1, msg = "保存失败" });
        }
        /// <summary>
        /// select shoptype by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [LimitAttr]
        [HttpGet,Route("ShopType/QueryShoptype")]
        public IActionResult QueryShoptype(int Id)
        {
            var data = context1.tb_shoptype.FirstOrDefault(r => r.id == Id);
            if (data == null) return Json(new { code = 1, msg = "查询失败" });
            return Json(new {code=0,msg="查询", data });
        }
        /// <summary>
        /// select all shop type
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("ShopType/QueryShopTypeList")]
        public IActionResult QueryShopTypeList()
        {
            var data = (from p in context1.tb_shoptype select p).ToList();
            return Json(new { code = 0, msg = "查询成功", data });
        }
    }
}
