using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class ShipsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly china70Context context1;
        public ShipsController(china70Context context, IWebHostEnvironment hostingEnvironment)
        {
            context1 = context;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 添加收货地址
        /// </summary>
        /// <param name="ships"></param>
        /// <returns></returns>
        [HttpPost,Route("Ships/AddorEditShips"),Auther,LimitAttr]
        public IActionResult AddorEditShips([FromBody]tb_ships ships)
        {
            if(ships.id== 0) context1.tb_ships.Add(ships);
            else context1.tb_ships.Update(ships);
            var irwos = context1.SaveChanges();
            if (irwos > 0) return Json(new { code = 0, msg = "保存成功" });
            return Json(new { code = 1, msg = "保存失败" });
        }

        /// <summary>
        /// 根据ID获取收货地址
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("Ships/QueryShipsinfo"),Auther,LimitAttr]
        public IActionResult QueryShipsinfo(int Id)
        {
            var data=context1.tb_ships.FirstOrDefault(x=>x.id==Id);
           // var result = new {id=data.id,name=data.shipperson,address=data.shipaddress };
            return Json(data);
        }
        /// <summary>
        /// 查询收货人列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet,Route("Ships/QueryShipsList"),Auther,LimitAttr]
        public IActionResult QueryShipsList(int UserId)
        {
            var data = (from p in context1.tb_ships
                       where p.userid == UserId  //context1.tb_ships.Where(x=>x.userid==UserId).ToList();
                       select new { 
                         id=p.id,
                         name=p.shipperson,
                         tel=p.shipTel,
                         address=p.shipaddress,
                         p.TaxID,
                         p.company,
                         p.isdefault
                       }).ToList();
            return Json(data);
        }
        /// <summary>
        /// get the ship with user id and default
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="isdefautl"></param>
        /// <returns></returns>
        [HttpGet,Route("Ships/QueryShipsInfobyId"),Auther,LimitAttr]
        public IActionResult QueryShipsInfobyId(int UserId,bool isdefault)
        {
            try
            {
                var data = context1.tb_ships.FirstOrDefault(x => x.userid == UserId && x.isdefault == isdefault);
                return Json(new { code = 0, msg = "ok", data });
            }catch(Exception ex)
            {
                return Json(new {code=409,msg=ex.Message});
            }

        }
        /// <summary>
        /// update address status
        /// </summary>
        /// <param name="updateAddress"></param>
        /// <returns></returns>
        [HttpPost,Route("Ships/UpdateAddress"),LimitAttr,Auther]
        public IActionResult UpdateAddress([FromBody]UpdateAddressParam updateAddress)
        {
            try
            {
                var data=context1.tb_ships.Where(r=>r.userid == updateAddress.userid).ToList();
                foreach(var item in data)
                {
                  if(item.id==updateAddress.id)  item.isdefault = true;
                  else item.isdefault= false;
                }
                context1.tb_ships.UpdateRange(data);
                var irows = context1.SaveChanges();
                return Json(new {code=0,msg="ok"});
            }catch(Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }

        /// <summary>
        /// 接收图片
        /// </summary>
        /// <param name="uploadprama"></param>
        /// <returns></returns>
        [HttpPost, Route("Ships/UploadImg")]
        [LimitAttr]
        [Auther]
        public IActionResult UploadImg([FromBody]uploaduserimageprama uploadprama)
        {
            var stry = DateTime.Now.Year.ToString();
            var strm = DateTime.Now.Month.ToString();
            var strd = DateTime.Now.Day.ToString();
            var fold =uploadprama.userid+"/"+ stry + "/" + strm + "/" + strd + "/";
            var path = _hostingEnvironment.WebRootPath + @"/userimages/" + fold;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var filename = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
            var byts = Convert.FromBase64String(uploadprama.imgbase64.Replace("data:image/jpeg;base64,", ""));
            System.IO.File.WriteAllBytes(path + filename, byts);

            // var hosturl = dbManager.Gethosturl("hosturl") + "/shopimgs/" + fold + filename;
            var hosturl = "/userimages/" + fold + filename;
            var data = context1.tb_Remarks.FirstOrDefault(r => r.userid == uploadprama.userid);
            if (data != null)
            {
                System.IO.File.Delete(_hostingEnvironment.WebRootPath+data.images);
                data.images = hosturl;
            }
            else
            {
                var remark = new tb_remark { userid = uploadprama.userid, images = hosturl };
                context1.tb_Remarks.Add(remark);
            }
            context1.SaveChanges();
             data = context1.tb_Remarks.FirstOrDefault(r => r.userid == uploadprama.userid);
            return Json(new { data });

        }

        /// <summary>
        /// save user remark 
        /// </summary>
        /// <param name="_Remark"></param>
        /// <returns></returns>
        [HttpPost,Route("Ships/AddorEditUserRemark"),LimitAttr,Auther]
        public IActionResult AddorEditUserRemark([FromBody]tb_remark _Remark)
        {
            try
            {
                if(_Remark.id==0) context1.tb_Remarks.Add(_Remark);
                else context1.tb_Remarks.Update(_Remark);
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "保存成功" });
                return Json(new { code = 1, msg = "保存失败" });
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// get the user remark
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Ships/QueryUserRemark"),LimitAttr,Auther]
        public IActionResult QueryUserRemark(int UserId)
        {
            var data=context1.tb_Remarks.FirstOrDefault(context1 => context1.userid == UserId);

            if (data == null) return Json(new {code=404,msg="查询为空"});
            data.images = string.Concat(dbManager.Gethosturl("hosturl"), data.images);
            return Json(new { code = 0, msg = "ok", data });

        }


    }
}
