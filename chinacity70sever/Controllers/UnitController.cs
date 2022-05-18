using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class UnitController : Controller
    {
        private readonly china70Context context1;
        public UnitController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// add or edit unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        [HttpPost,Route("Unit/UnitAddorEdit")]
        [LimitAttr]
        public IActionResult UnitAddorEdit([FromBody]tb_unit unit)
        {
            if(unit.id==0) context1.tb_Units.Add(unit);
            else context1.tb_Units.Update(unit);
            var irows = context1.SaveChanges();
            return Json(new { code = 0, msg = "保存成功" });
        }

        [HttpGet,Route("Unit/QueryUnitList")]
        [LimitAttr]
        public IActionResult QueryUnitList()
        {
            var data=context1.tb_Units.ToList();
            return Json(data);
        }
    }
}
