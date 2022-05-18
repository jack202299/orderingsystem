using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class PositionController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly china70Context context1;
        public PositionController(china70Context context, IWebHostEnvironment hostingEnvironment)
        {
            context1 = context;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        ///save position data
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [HttpPost,Route("Position/AddorEditPosition")]
        public IActionResult AddorEditPosition([FromBody]tb_position position)
        {
            try
            {
                if (position.id == 0) context1.tb_position.Add(position);
                else context1.tb_position.Update(position);
                var irows = context1.SaveChanges();
                return irows > 0 ? Json(new { code = 0, msg = "save success" }) : Json(new { code = 1, msg = "save fail" });
            }catch (Exception ex)
            {
                return Json(new {code=404,msg=ex.Message});
            }

        }
        /// <summary>
        /// select the position by shop id
        /// </summary>
        /// <param name="shopid"></param>
        /// <returns></returns>
        [HttpGet,Route("Position/QueryPositions"),Auther]
        public IActionResult QueryPositions(int shopid)
        {
            var data = from p in context1.tb_position
                       where p.shopid == shopid
                       select new {value= p.id,text= p.position };
            return Json(data);
        }
        /// <summary>
        /// query the postion list
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost, Route("Position/QueryPostion")]
        public IActionResult QueryPostion([FromBody]queryPostionParam query)
        {
            try
            {
                if (query == null) return Json(new { code = 400, msg = "参数缺失" });
                var data = from p in context1.tb_position
                           where p.shopid == query.shopid && p.position.Contains(query.custtxt)
                           select p;
                var icount = data.Count();
                var result = data.Skip((query.page - 1) * query.pagesize).Take(query.pagesize).ToList();
                return Json(new { code = 0, msg = "ok", icount, result });

            }
            catch (Exception ex)
            {
                new Syslog(this.context1).writelog("Position/QueryPostion", ex.Message);
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        /// <summary>
        /// delete the postion by row id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Position/deletePost")]
        public IActionResult deletePost(int id)
        {
            try
            {
                var row=context1.tb_position.FirstOrDefault(p => p.id == id);
                context1.tb_position.Remove(row);
                var irows = context1.SaveChanges();
                return irows>0? Json(new { code = 0, msg = "删除成功" }):Json(new {code=1,msg="删除失败"});

            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("Position/deletePost", ex.Message);
                return Json(new {code=500, msg=ex.Message});
            }
        }
    }
}
