using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class ClientageController : Controller
    {
        private readonly china70Context context1;
        public ClientageController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// get user info
        /// </summary>
        /// <param name="clientageParam"></param>
        /// <returns></returns>
        [HttpPost,Route("Clientage/QueryMyClientage"),LimitAttr,Auther]
        public IActionResult QueryMyClientage([FromBody]QueryMyClientageParam clientageParam)
        {
            try
            {
                var data =( from p in context1.tb_Clientages
                           join r in context1.tb_shop on p.shopid equals r.id
                           join cc in context1.tb_users on p.userid equals cc.id
                           where p.shopid == clientageParam.shopid && (cc.name.Contains(clientageParam.strname) || cc.tel.Contains(clientageParam.strname))
                           select new {cc.id, cc.name,cc.tel,
                               icount=(context1.tb_Orders.Count(x=>x.userid==cc.id && x.blfalge==1)),
                               image=(from xx in context1.tb_Remarks where xx.userid==cc.id select xx.images).FirstOrDefault()
                           }).ToList();
                return Json(new { code = 0, msg = "ok", data });

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
