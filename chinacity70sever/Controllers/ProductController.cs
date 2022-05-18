using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace chinacity70sever.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly china70Context context1;
        public ProductController(china70Context context, IWebHostEnvironment hostingEnvironment)
        {
            context1 = context;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="tb_Product"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/ProductAddOrEdit")]
        [LimitAttr]
        [Auther]
        public IActionResult ProductAddOrEdit([FromBody]tb_product tb_Product) {

            try
            {
                if (checkproducticount((int)tb_Product.shopid)) return Json(new
                {
                    code = 2,
                    msg = "上传产品数量已达到上限，如需上传更多产品请升级店铺等级"
                });
                if (tb_Product.id == 0) context1.tb_Products.Add(tb_Product);
                else context1.tb_Products.Update(tb_Product);
                var irows = context1.SaveChanges();
                if (irows == 0) return Json(new { code = 0, msg = "保存失败" });
                else return Json(new { code = 1, msg = "保存成功" });
            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("Product/ProductAddOrEdit", ex.Message);
                return Json(new {code=508,msg=ex.Message });
            }
            //var data=dbManager.ProductAddOrEdit(tb_Product);
            //return Json(data);

        }
        /// <summary>
        /// checking shop product count by shop id
        /// </summary>
        /// <param name="ShopId"></param>
        /// <returns></returns>
        public bool checkproducticount(int ShopId)
        {
            try
            {
                var count = context1.tb_Products.Count(x => x.shopid == ShopId);
                var data = (from p in context1.tb_shoptype
                              join r in context1.tb_shop on p.id equals r.shoptypid
                              where r.id == ShopId
                              select new {p.icount,r.id }).FirstOrDefault();
                var maxid = context1.tb_shoptype.OrderByDescending(x => x.icount).FirstOrDefault().id;
                if (data == null) return true;
                if (maxid == data.id) return false;
                else if(count == data.icount) return true;
                else return false;
            }catch(Exception ex) {
                var log = new tb_syslog();
                log.errmsg = ex.Message;
                log.fun = "checkproducticount";
                log.createdate = DateTime.Now.ToString();
                log.id = 0;
                context1.tb_Syslogs.Add(log);
                context1.SaveChanges();
                return false; 
            }
       }


        /// <summary>
        /// 根据商铺ID
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/QueryProductShopId")]
        [LimitAttr]
        public IActionResult QueryProductShopId([FromBody]queryproductParam param)
        {
          
            var icount=context1.tb_Products.Count(x=>x.shopid==param.shopid && x.productName.Contains(param.productpar) && x.isdown == 0);
            //var totalPage = (icount + param.pagesize - 1) / param.pagesize;
            var data=context1.tb_Products.Where(x=>x.shopid == param.shopid && x.productName.Contains(param.productpar) && x.isdown == 0).Skip((param.page-1)*param.pagesize).Take(param.pagesize).OrderByDescending(x=>x.id).ToList();
            foreach (var im in data)
            {
                im.images = dbManager.Gethosturl("hosturl") + im.images;
            }
            return Json(new {code=0,msg="ok", totalPage= icount, data });

        }

        /// <summary>
        /// 根据商铺ID查询产品表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("Product/QueryProductWithShopId")]
        [LimitAttr]
        public IActionResult QueryProductWithShopId([FromBody]productParam param)
        {

            if (param.shopid == 0) return Json(new { code = 404, msg = "no shop" });
            var shopinfo = context1.tb_shop.FirstOrDefault(x => x.id == param.shopid);
            var dy = Convert.ToDateTime(shopinfo.lastDate) - DateTime.Now;
            if (dy.Days <= 0) return Json(new { code = -1, msg = "店铺使用期限已到，请联系管理员充值" });

            if (param.producttypeid == 0)
            {
                var icount = context1.tb_Products.Count(x => x.shopid == param.shopid && x.productName.Contains(param.ProductName) && x.isdown==0);

                var data = context1.tb_Products.Where(x => x.shopid == param.shopid && x.productName.Contains(param.ProductName) && x.isdown == 0).Skip((param.page - 1) * param.pageSize).Take(param.pageSize).OrderByDescending(x=>x.id).ToList();

                foreach (var im in data)
                {
                    im.images =im.images.IndexOf("http")==-1? dbManager.Gethosturl("hosturl") + im.images:im.images;
                    var span = DateTime.Now - Convert.ToDateTime(im.createdate);
                    im.hotsort = span.Days < 7 ? true : false;
                }
                return Json(new { code = 0, data = data, icount });
            }
            else { 
            var icount = context1.tb_Products.Count(x => x.shopid == param.shopid && x.productType == param.producttypeid && x.productName.Contains(param.ProductName) && x.isdown == 0);

            var data = context1.tb_Products.Where(x => x.shopid == param.shopid && x.productType == param.producttypeid && x.productName.Contains(param.ProductName) && x.isdown == 0).Skip((param.page - 1) * param.pageSize).Take(param.pageSize).OrderByDescending(x=>x.id).ToList();

            foreach (var im in data)
            {
                   im.images = im.images.IndexOf("http") == -1 ? dbManager.Gethosturl("hosturl") + im.images : im.images;
                    var span = DateTime.Now - Convert.ToDateTime(im.createdate);
                im.hotsort = span.Days <7 ? true : false;
                
            }
            return Json(new { code = 0, data = data, icount });
        }
        }
        /// <summary>
        /// 根据分类查询
        /// </summary>
        /// <param name="kindsParam"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/QueryProductList")]
        [LimitAttr]
        public IActionResult QueryProductList([FromBody]ProductQueryKindsParam kindsParam)
        {
            if (kindsParam.kindsid == 0)
            {

                var icount = context1.tb_Products.Count(x => x.hotsort == true && x.productName.Contains(kindsParam.productname));
               // var totalPage = (icount + kindsParam.pagesize - 1) / kindsParam.pagesize;
                var tempdata = (from p in context1.tb_Products
                               join r in context1.tb_shop on p.shopid equals r.id
                               where p.hotsort == true && p.productName.Contains(kindsParam.productname)
                               select new { p.shopid, p.productName,p.alias, p.images, p.price, p.sellcount, p.id, r.shopname, num=0,p.unit }).ToList();
                            var data = tempdata.Skip((kindsParam.page - 1) * kindsParam.pagesize).Take(kindsParam.pagesize).ToList();
                return Json(new { totalPage= icount, data });
            }
            else
            {
                var icount = context1.tb_Products.Count(x =>　x.productName.Contains(kindsParam.productname));
               // var totalPage = (icount + kindsParam.pagesize - 1) / kindsParam.pagesize;
                var tempdata = (from p in context1.tb_Products
                                join r in context1.tb_shop on p.shopid equals r.id
                                where  p.productName.Contains(kindsParam.productname)
                                select new { p.shopid, p.productName,p.alias, p.images, p.price, p.sellcount, p.id, r.shopname, num = 0,p.unit }).ToList();
                 var data = tempdata.Skip((kindsParam.page - 1) * kindsParam.pagesize).Take(kindsParam.pagesize).ToList();
                return Json(new { totalPage= icount, data });
            }
        }

        


        /// <summary>
        /// 查询单个产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Product/QueryProductInfo")]
        [LimitAttr]
        public IActionResult QueryProductInfo(int id)
        {
            var data = context1.tb_Products.FirstOrDefault(x => x.id == id);

            if (data == null) return Json( new { code = 0, msg = "查询失败" });
            data.images= dbManager.Gethosturl("hosturl") + data.images;
            return Json( new { code = 1, data });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/QueryProductInfoByid")]
        public IActionResult QueryProductInfoByid([FromBody]prodctinfoParam param)
        {
            try
            {
                int temprice =0;
                var temporder=context1.tb_Orders.Where(x=>x.userid==param.userid && x.shopid==param.shopid).OrderByDescending(x=>x.id).FirstOrDefault();
                //   temprice = temporder == null ? 0 : temprice;
                if (temporder != null)
                {
                    var tempordertail = context1.orderDeTails.Where(x => x.productID == param.proid && x.orderid == temporder.id).FirstOrDefault();
                    if (tempordertail != null) temprice =(int)tempordertail.lastprice;
                    
                }
                
              
                var data = (from p in context1.tb_Products
                            where p.id == param.proid
                            select new
                            {
                               p.id,
                               p.barcode,
                               p.productName,
                               p.alias,
                               p.unit,
                               p.price,
                               p.bulkprice,                             
                               lastprice=temprice,
                               quantity=p.reserve
                            }).FirstOrDefault();

                return Json(new { code = 0, msg = "ok", data });

            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Product/QueryProductInfoByid", ex.Message);
                return Json( new { code = 501, msg = ex.Message });
            }
        }

        /// <summary>
        /// 接收图片
        /// </summary>
        /// <param name="uploadprama"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/UploadImg")]
        [LimitAttr]
        [Auther]
        public IActionResult UploadImg([FromBody]uploadprama uploadprama)
        {
            var stry = DateTime.Now.Year.ToString();
            var strm=DateTime.Now.Month.ToString();
            var strd=DateTime.Now.Day.ToString();
            var fold =uploadprama.shopid.ToString()+"/"+ stry+"/"+strm+"/"+strd+"/";
            var path = _hostingEnvironment.WebRootPath + @"/images/"+fold;
            if(!Directory.Exists(path)) Directory.CreateDirectory(path);
            var filename=Guid.NewGuid().ToString().Replace("-","")+".jpg";
            var byts = Convert.FromBase64String(uploadprama.imgbase64.Replace("data:image/jpeg;base64,",""));
            System.IO.File.WriteAllBytes(path+filename,byts);

            // var hosturl =dbManager.Gethosturl("hosturl") +"/images/"+fold+filename;
            var hosturl ="/images/" + fold + filename;
            return Json(new {imgurl=hosturl });
            
        }
        /// <summary>
        /// delee img by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet,Route("Product/DelImgByUrl"),LimitAttr,Auther]
        public IActionResult DelImgByUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return Json(new { code = 409, msg = "参数不能为空" });
                if (url.IndexOf("http") == -1)
                {
                    var path1 = string.Concat(_hostingEnvironment.WebRootPath,  url);
                    if (System.IO.File.Exists(path1)) System.IO.File.Delete(path1);
                }
                else
                {
                string[] str = url.Replace("//", "|").Split('|');
                var path =string.Concat( _hostingEnvironment.WebRootPath ,"/", str[2]);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
             
                return Json(new { code = 0, msg = "delete sueccss" });
            }catch(Exception ex)
            {
                return Json(new { code = 409, msg =ex.Message });
            }
        }
        /// <summary>
        /// delete product row with id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("Product/DelProduct")]
        [LimitAttr]
        [Auther]
        public IActionResult DelProduct(int Id)
        {
            var data= context1.tb_Products.FirstOrDefault(x=>x.id==Id);
            context1.tb_Products.Remove(data);
            var imgfile = _hostingEnvironment.WebRootPath + data.images;
            if(System.IO.File.Exists(imgfile)) System.IO.File.Delete(imgfile);
            var irows = context1.SaveChanges();
            if (irows > 0) return Json(new { code = 0, msg = "delete success" });
            return Json(new { code = 1, msg = "delete fail" });
        }
        /// <summary>
        /// select product upload count by date
        /// </summary>
        /// <param name="monthParam"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/QueryProductUpload")]
        [LimitAttr]
        public IActionResult QueryProductUpload([FromBody]QueryProductUpload monthParam)
        {
            var strd =Convert.ToDateTime( monthParam.querydate[0]);
            var endd =Convert.ToDateTime( monthParam.querydate[1]);
            var span =( endd - strd).Days;
            var str=new List<string>();
            for(int i = 0; i < span; i++)
            {
              var dd=  strd.AddDays(i).ToString("yyyy-M-d");
               str.Add(dd);
            }
            var data =( from p in context1.tb_Products
                       
                       where str.Contains( p.createdate)
                       && p.shopid == monthParam.shopid
                       group p by new { p.createdate, p.createby } into g
                       select new {pp=g.Key,count=g.Count() }).ToList();
            var sumicount = 0;
            var list=new List<dynamic>();
            foreach (var item in str) {
                var temp = (from p in data
                            join r in context1.tb_users on p.pp.createby equals r.id
                            where p.pp.createdate ==item
                           select new { r.name, p.count }).ToList();
                var tol = context1.tb_Products.Count(x =>x.createdate==item && x.shopid==monthParam.shopid );
                sumicount+=tol;
                var vk = new {date=string.Concat(item,"拍照上传数量",tol),sublist=temp };
                list.Add(vk);
            }
            var result = new {list,tol=sumicount };
            return Json(result);
        }
        /// <summary>
        /// zw translate hr
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        [HttpGet,Route("Product/CNtoHR"),LimitAttr,Auther]
        public IActionResult CNtoHR(string txt)
        {
            try
            {
                var url = dbManager.Gethosturl("tral");
                var path = string.Format(url, txt);
                var wc = new System.Net.WebClient();
                var str = wc.DownloadString(path);
                var joroot = Newtonsoft.Json.JsonConvert.DeserializeObject(str) as JObject;
                var strk1 = joroot["sentences"][0]["trans"].ToString();
                return Json(new { code = 0, msg = "ok", result = strk1 });
            } catch (Exception ex)
            {
                new Syslog(this.context1).writelog("Product/CNtoHR", ex.Message);
                
                return Json(new { code = 400, msg = ex.Message});
            }
        }

        /// <summary>
        /// 生号条码流水号
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("Product/GetNumberWithDate")]
        public IActionResult GetNumberWithDate()
        {
            var dte = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return Json(new {code=0,number=dte}); 
        }

        /// <summary>
        /// 通过产品名称模糊查询
        /// </summary>
        /// <param name="nameParam"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/getProductsByName")]
        public IActionResult getProductsByName([FromBody]getProductsByNameParam nameParam)
        {
            try
            {
                var data = (from p in context1.tb_Products
                            where p.productName.Contains(nameParam.proname) && p.shopid==nameParam.shopid && p.isdown==0
                            select new { value = p.id, text = p.productName }).ToList();
                return Json(new { code = 0, msg = "ok", data });
            }catch(Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        /// <summary>
        /// query product by barcode
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("Product/QueryProductsByBarcode"),Auther,LimitAttr]
        public IActionResult QueryProductsByBarcode([FromBody]queryProductParam param)
        {
            try
            {
                var data = (from p in context1.tb_Products
                            where p.shopid == param.shopid && p.barcode == param.barcode
                            select new
                            {
                                productID=p.id,
                                productName=p.productName,
                                price=p.price,
                                barcode=p.barcode,
                                bulkprice=p.bulkprice,
                                unit=p.unit,
                                quantity=1,
                                discount=1,
                                discountname= "无折扣",
                                amount=p.price,
                            }).FirstOrDefault();

               

                return Json(new { code = 0, msg ="ok",data });

            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Product/QueryProductsByBarcode", ex.Message);
                return Json(new {code=500, msg=ex.Message});
            }
        }
       
    }
}
