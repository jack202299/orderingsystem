using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class KindsController : Controller
    {
        private readonly china70Context context1;
        public KindsController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// 查询所有类型
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Kinds/QuerKindsList")]
        [LimitAttr]
        public IActionResult QuerKindsList()
        {
            var data=context1.kinds.ToList();
            return Json(data);
        }
        /// <summary>
        /// add or edit kinds
        /// </summary>
        /// <param name="kinds"></param>
        /// <returns></returns>
        [HttpPost,Route("Kinds/AddOrEditKind"),LimitAttr,Auther]
        public IActionResult AddOrEditKind([FromBody]tb_kinds kinds)
        {
            try
            {
                if (kinds == null) return Json(new {code=400,msg="参数不能为空"});
                if(kinds.id==0)  context1.kinds.Add(kinds);
                else context1.kinds.Update(kinds);
                var irows = context1.SaveChanges();
                return Json(new { code = 0, msg = "保存成功" });

            }catch(Exception ex)
            {
                return Json(new { code = 11, msg = ex.Message });
            }
        }
    }
}
