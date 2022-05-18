using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;


namespace chinacity70sever.Controllers
{
    public class ShopController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly china70Context context1;
        public ShopController(china70Context context, IWebHostEnvironment hostingEnvironment)
        {
            context1 = context;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 添加铺面
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpPost,Route("Shop/AddOrEditShop")]
        [Auther]
        public IActionResult AddOrEditShop([FromBody]tb_shop shop)
        {
           // var data=context1.tb_shop.Count(x=>x.userid==shop.userid);            

            if(shop.id==0) context1.tb_shop.Add(shop);
            else context1.tb_shop.Update(shop);
            var irow=context1.SaveChanges();
            if (irow > 0) return Json(new { code = 0, msg = "保存成功" });
            else return Json(new { code = 1, msg = "保存失败" });
        }

        /// <summary>
        /// select the shop info for shop id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("Shop/QueryShopInfWithID")]
        [LimitAttr]
        public IActionResult QueryShopInfWithID(int Id)
        {
            var data = (from p in context1.tb_shop
                       where p.id == Id
                       select new
                       {
                           p.id,
                           p.kindid,
                           p.shopimg,
                           p.shopname,
                           p.userid,
                           p.bankaccount,
                           p.blfage,
                           p.lastDate,
                           person = (from x in context1.tb_users where x.id == p.userid select x.name).FirstOrDefault(),
                           p.tel,
                           p.shopaddress,
                           p.shoptypid,
                           

                       }).FirstOrDefault();


            if (data == null) return Json(new { code = 0, msg = "查询为空" });
            return Json(new { code = 1,msg ="ok", data });
        }
        /// <summary>
        /// 查询自己的商铺
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet,Route("Shop/QueryShopInfo")]
        [LimitAttr]
        public IActionResult QueryShopInfo(int UserId)
        {
             var data=context1.tb_shop.FirstOrDefault(x=>x.userid==UserId);
            if (data == null) return Json(new { code = 0, msg = "没有创建有店铺" });
            var days =  Convert.ToDateTime(data.lastDate)-DateTime.Now;
            data.usdays = days.Days;
            context1.tb_shop.Update(data);
            context1.SaveChanges();
            var pcount=context1.tb_Products.Count(x=>x.shopid==data.id);
            var icount=context1.tb_shoptype.FirstOrDefault(x=>x.id==data.shoptypid).icount;
            var countmsg = string.Format("{0}/{1}",pcount,icount);
            return Json(new { code = 1, msg = "查询成功",data , countmsg });
        }
        /// <summary>
        /// 模糊查询店铺
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("Shop/QueryShopList")]
        [LimitAttr]
        public IActionResult QueryShopList([FromBody]QueryShopListParam param)
        {
            var data = context1.tb_shop.Where(x => x.shopname.Contains(param.shopname) && x.kindid==param.kindsid && x.blfage==false);
            if(param.kindsid==0) data=context1.tb_shop.Where(x=>x.shopname.Contains(param.shopname) && x.blfage==false);
            var icount=data.ToList().Count();
            var result=data.Skip((param.page-1)*param.pagesize).Take(param.pagesize).ToList();
            return Json(new {code=0,msg="查询成功",icount,result});

        }

        /// <summary>
        /// query shops
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("Shop/QueryShopls")]
        public IActionResult QueryShopls([FromBody]QueryShopListParam param)
        {
            try
            {
                var data = from p in context1.tb_shop
                           where p.shopname.Contains(param.shopname) && p.kindid == param.kindsid && p.blfage==false
                           select new
                           {
                               p.id,
                               p.userid,
                               p.person,
                               p.shopaddress,
                               p.shopname,
                               p.lastDate,
                               kind=(from x in context1.kinds where x.id== p.kindid select x.kindsname).FirstOrDefault(),
                               shoptype=(from x in context1.tb_shoptype where x.id==p.shoptypid select x.typename).FirstOrDefault(),
                               p.tel,
                               p.bankaccount,
                               p.usdays
                           };
                var icount=data.ToList().Count();
                var result=data.Skip((param.page - 1) * param.pagesize).Take(param.pagesize).ToList();
                return Json(new {code=0,msg="ok",icount,result,});
            }
            catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Shop/QueryShopls", ex.Message);
                return Json(new {code=500,msg=ex.Message});
            }
        }

        /// <summary>
        /// 查询所有铺面
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Shop/QueryShopListAll")]
        [LimitAttr]
        public IActionResult QueryShopListAll()
        {
            var data = context1.tb_shop.ToList();
            return Json(data);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Shop/qrcodeget"),Auther]
        [LimitAttr]
        public IActionResult  qrcodeget(int id)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(String.Concat(dbManager.Gethosturl("myshop"),"?shopid="+id), QRCodeGenerator.ECCLevel.L);
            BitmapByteQRCode qRCode =new(qrCodeData);
            var qrCodeImage = qRCode.GetGraphic(20);           
            var base64=Convert.ToBase64String(qrCodeImage);
            return Json(new { image = string.Concat("data:image/jpeg;base64,", base64) });
        }

        /// <summary>
        /// 接收图片
        /// </summary>
        /// <param name="uploadprama"></param>
        /// <returns></returns>
        [HttpPost, Route("Shop/UploadImg")]
        [LimitAttr]
        [Auther]
        public IActionResult UploadImg([FromBody]uploadprama uploadprama)
        {
            var stry = DateTime.Now.Year.ToString();
            var strm = DateTime.Now.Month.ToString();
            var strd = DateTime.Now.Day.ToString();
            var fold =  stry + "/" + strm + "/" + strd + "/";
            var path = _hostingEnvironment.WebRootPath + @"/shopimgs/" + fold;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var filename = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
            var byts = Convert.FromBase64String(uploadprama.imgbase64.Replace("data:image/jpeg;base64,", ""));
            System.IO.File.WriteAllBytes(path + filename, byts);

            // var hosturl = dbManager.Gethosturl("hosturl") + "/shopimgs/" + fold + filename;
            var hosturl = "/shopimgs/" + fold + filename;
            return Json(new { imgurl = hosturl });

        }
        /// <summary>
        /// shop invalid by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet,Route("Shop/ShopInvalid")]
        public IActionResult ShopInvalid(int Id)
        {
            try
        {
                var data=context1.tb_shop.FirstOrDefault(x=>x.id == Id);
                if (data == null) return Json(new { code = 400, msg = "找不到此铺面" });
                data.blfage = true; //作废
                context1.tb_shop.Update(data);
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "作废成功" });
                else return Json(new { code = 1, msg = "作废失败" });

            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("Shop/ShopInvalid", ex.Message);
                return Json(new {code=500,msg=ex.Message});
            }
        }

       
    }
}
