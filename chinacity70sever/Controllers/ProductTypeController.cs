using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly china70Context context1;
        public ProductTypeController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// add or edit producttype
        /// </summary>
        /// <param name="tb_Producttype"></param>
        /// <returns></returns>
        [HttpPost,Route("ProductType/AddorEditProductType"), LimitAttr, Auther]
        public IActionResult AddorEditProductType([FromBody]tb_producttype tb_Producttype)
        {
            try
            {
                if (tb_Producttype == null) return NotFound();
                if (tb_Producttype.id == 0) context1.tb_Producttypes.Add(tb_Producttype);
                else context1.tb_Producttypes.Update(tb_Producttype);
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "保存成功" });
                else return Json(new { code = 1, msg = "保存失败" });
            }catch (Exception ex)
            {
                var log = new tb_syslog();
                log.errmsg = ex.Message;
                log.fun = "ProductType/AddorEditProductType";
                log.createdate = DateTime.Now.ToString();
                log.id = 0;
                context1.tb_Syslogs.Add(log);
                context1.SaveChanges();
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// select the product type by kind id
        /// </summary>
        /// <param name="shopid"></param>
        /// <returns></returns>
        [HttpGet,Route("ProductType/QueryProductTypeList"),LimitAttr,Auther]
        public IActionResult QueryProductTypeList(int kindsid)
        {
            try
            {
                var data = context1.tb_Producttypes.Where(r => r.kindsid == kindsid).ToList();
                if (data == null) return Json(new { code = 0, msg = "没有查询到这个分类" });
                return Json(new { code = 1, msg = "成功查询", data });
            }catch(Exception ex)
            {
                var log = new tb_syslog();
                log.errmsg = ex.Message;
                log.fun = "ProductType/QueryProductTypeList";
                log.createdate = DateTime.Now.ToString();
                log.id = 0;
                context1.tb_Syslogs.Add(log);
                context1.SaveChanges();
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// get proudct type by shop id
        /// </summary>
        /// <param name="shopid"></param>
        /// <returns></returns>
        [HttpGet,Route("ProductType/QueryProductTypeListByShopId")]
        public IActionResult QueryProductTypeListByShopId(int shopid)
        {
            try
            {
                var data = (from p in context1.tb_Producttypes
                           join r in context1.tb_shop on p.kindsid equals r.kindid
                           where r.id == shopid
                           select new { p.id,p.protype}).ToList();
                return Json(new { code = 0, msg = "ok", data });
            }catch(Exception ex)
            {
                var log = new tb_syslog();
                log.errmsg = ex.Message;
                log.fun = "ProductType/QueryProductTypeList";
                log.createdate = DateTime.Now.ToString();
                log.id = 0;
                context1.tb_Syslogs.Add(log);
                context1.SaveChanges();
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// select class by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("ProductType/QueryProductTypeInfo"),LimitAttr,Auther]
        public IActionResult QueryProductTypeInfo(int Id)
        {
            try
            {
               var data=context1.tb_Producttypes.FirstOrDefault(x=>x.id == Id);
                return Json(new { code = 1, msg = "ok",data });

            }catch(Exception ex)
            {
                var log = new tb_syslog();
                log.errmsg = ex.Message;
                log.fun = "ProductType/QueryProductTypeInfo";
                log.createdate = DateTime.Now.ToString();
                log.id = 0;
                context1.tb_Syslogs.Add(log);
                context1.SaveChanges();
                return BadRequest(ex.Message);
            }
        }
    }
}
