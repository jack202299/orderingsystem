using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace chinacity70sever.Controllers
{
    public class PermissionController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly china70Context context1;
        public PermissionController(china70Context context, IWebHostEnvironment hostingEnvironment)
        {
            context1 = context;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// menu add or edit
        /// </summary>
        /// <param name="tb_Menu"></param>
        /// <returns></returns>
        [HttpPost,Route("Permission/AddorEditMenu")]
       public IActionResult AddorEditMenu([FromBody]tb_menu tb_Menu)
        {
            try
            {
                if(tb_Menu.id==0) context1.tb_Menus.Add(tb_Menu);
                else context1.tb_Menus.Update(tb_Menu);
                var irows=context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "save success" });
                else return Json(new { code = 1, msg = "save fail" });
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Permission/AddorEditMenu", ex.Message);
                return Json(new {code=404,msg=ex.Message});
            }
        }
        /// <summary>
        /// get menu list
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Permission/QueryMeunList")]
        public IActionResult QueryMeunList()
        {
            try
            {
                var rootdata = context1.tb_Menus.Where(x=>x.partid==0).ToList();
                var vs = new List<dynamic>();
                foreach(var item in rootdata)
                {
                    var childdata = from p in context1.tb_Menus
                                    where p.partid == item.id
                                    select new { p.id, p.menuguid, name = p.menuname };


                    var vk = new { id = item.id, name = item.menuname,  item.menuguid, children = childdata };
                    vs.Add(vk);
                }
                return Json(new { code = 0, msg = "ok",data= vs });
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Permission/QueryMeunList", ex.Message);
                return Json(new { code = 404, msg = ex.Message });
            }
        }
        /// <summary>
        /// get guid
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Permission/GetGuid")]
        public IActionResult GetGuid()
        {
            var data=Guid.NewGuid().ToString().Replace("-","");
            return Json(new { code = 0, data });
        }

        /// <summary>
        /// get the menus
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Permission/getMenus")]
        public IActionResult getMenus()
        {
            var data = from p in context1.tb_Menus
                       where p.partid == 0
                       select new
                       {
                           text = p.menuname,
                           value = p.id
                       };
            return Json(new { code = 0, data });
        }
        /// <summary>
        /// get menu with check fild
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Permission/getMenusForCheck")]
        public IActionResult getMenusForCheck()
        {
            var data = from p in context1.tb_Menus
                       where p.partid == 0
                       select new
                       {                        
                          text=p.menuname,
                          sub=(from x in context1.tb_Menus where x.partid==p.id select new {x.menuguid,x.menuname,bl=false}).ToList()
                       };
            var str =new int[11, 33];
           
            return Json(new { code = 0,data });
        }
        /// <summary>
        /// save permission
        /// </summary>
        /// <param name="addor"></param>
        /// <returns></returns>
        [HttpPost,Route("Permission/AddorEditPermission")]
        public IActionResult AddorEditPermission([FromBody]AddorEditPermissionParam addor)
        {
            try
            {
                if (addor == null) return Json(new { code = 400, msg = "paramter can not empty" });
                var data=context1.tb_Permissions.Where(x=>x.userid==addor.userid).ToList();
                if (data.Count > 0) context1.tb_Permissions.RemoveRange(data);
                foreach(var item in addor.vs)
                {
                    var row = new tb_permission { userid = addor.userid, menuid = item, is_active = false };
                    context1.tb_Permissions.Add(row);
                }
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "setup success" });
                return Json(new { code = 1, msg = "setup fail" });
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Permission/AddorEditPermission", ex.Message);
                return Json(new { code = 404, msg = ex.Message });
            }
        }
        /// <summary>
        /// query permission
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("Permission/QueryPermissionByUserId")]
        public IActionResult QueryPermissionByUserId(int Id)
        {
            var data = (from p in context1.tb_Permissions where p.userid == Id select p.menuid).ToList();
            return Json(new { code = 0, msg = "ok", data });
        }
        /// <summary>
        /// check perssion
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("Permission/checkPerssion")]
        public IActionResult checkPerssion([FromBody]checkPerssionParam param)
        {
            if (param == null) return Json(new { code = 400, msg = "paramter can not empty" });

            var check = context1.tb_users.FirstOrDefault(x => x.id == param.userid);
            if (check.Identitytype == "admin") return Json(new { code = 1, msg = "ok" });
            var data = context1.tb_Permissions.Count(x => x.userid == param.userid && x.menuid == param.guid);
            if (data == 0) return Json(new { code = 0, msg = "无此功能权限" });
            return Json(new { code = 1, msg ="ok"});
        }
    }
}
