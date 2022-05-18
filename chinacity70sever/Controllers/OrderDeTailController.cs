using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace chinacity70sever.Controllers
{
    public class OrderDeTailController : Controller
    {
        private readonly china70Context context1;
        public OrderDeTailController(china70Context context)
        {
            context1 = context;
        }


        /// <summary>
        /// get the data to print
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpGet,Route("OrderDeTail/QueryOrderPrint")]
        public string QueryOrderPrint(int orderid,int ordertype)
        {
            try
            {
                var data = (from p in context1.orderDeTails
                            join x in context1.tb_Products on p.productID equals x.id
                            where p.orderid == orderid
                            select new
                            {
                                x.id,
                                x.productName,
                                x.alias,
                                x.barcode,
                                price = ordertype == 1 ? x.price : x.bulkprice,
                                p.quantity,
                                position = (from p in context1.tb_position where p.id == x.positionid select p.position).FirstOrDefault(),
                                discountname = double.Parse(p.discount) < 0.2 ? p.discountname : "无折扣",
                                p.discount,
                                amount = (ordertype == 1 ? x.price : x.bulkprice) * p.quantity * (double.Parse(p.discount) > 0.2 ? decimal.Parse(p.discount) : 1),
                                x.unit
                            }).ToList();


                var pdata = (from p in context1.tb_Orders
                             join x in context1.tb_shop
                               on p.shopid equals x.id
                             join r in context1.tb_users on p.userid equals r.id

                             where p.id == orderid
                             select new
                             {
                                 p.id,
                                 p.ordername,
                                 p.createdate,
                                 p.meney,
                                 p.Remark,
                                 r.name,
                                 x.shopname,
                                 paymethod = p.paymethod == 1 ? "现金" : "银行卡",
                                 r.tel,

                             }).ToList();

                var d = new { Detail = data, Master = pdata };

                return Newtonsoft.Json.JsonConvert.SerializeObject(d);
            }
            catch (Exception ex)
            {
                new Syslog(this.context1).writelog("OrderDeTail/QueryOrderPrint", ex.Message);
                return ex.Message;
            }           
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("OrderDeTail/QueryPosData")]
        public string QueryPosData(string id)
        {
            try
            {
                var data = (from p in context1.orderDeTails join r in context1.tb_Products on p.productID equals r.id
                            join o in context1.tb_Orders on p.orderid equals o.id
                           where o.ordername == id
                           select new
                           {
                               r.productName,
                               r.price,
                               p.quantity,
                               amount=r.price*p.quantity
                           }).ToList();
                var pdata=(from p in context1.tb_Orders join s in context1.tb_shop on p.shopid equals s.id
                          where p.ordername ==id
                          select new
                          {
                              s.shopname,
                              p.ordername,
                              p.meney,
                              p.actuallyreceived

                          }).ToList();
                var d = new { Detail = data, Master = pdata };
                return Newtonsoft.Json.JsonConvert.SerializeObject(d);
            }
            catch(Exception ex)
            {
                new Syslog(this.context1).writelog("OrderDeTail/QueryPosData", ex.Message);
                return ex.Message;
            }
        }


        /// <summary>
        /// select the order datail by order id
        /// </summary>
        /// <param name="orderid"></param>
        ///  <param name="ordertype">1=零售单 2=批发单</param>
        /// <returns></returns>
        [HttpGet,Route("OrderDeTail/QueryOrderDatailByOrId")]
        public IActionResult QueryOrderDatailByOrId(int orderid,int ordertype)
        {
            try
            {
                var data = (from p in context1.orderDeTails
                            join x in context1.tb_Products on p.productID equals x.id
                            where p.orderid == orderid
                            select new
                            {
                                x.id,
                                x.productName,
                                x.alias,
                                images = dbManager.Gethosturl("hosturl") + x.images,
                                price = ordertype == 1 ? x.price : x.bulkprice,
                                x.unit,
                                x.barcode,
                                position = (from p in context1.tb_position where p.id == x.positionid select p.position).FirstOrDefault(),
                                p.quantity,
                                p.discountname,
                                p.discount,
                                amount = (ordertype == 1 ? x.price : x.bulkprice) * p.quantity * (double.Parse(p.discount) > 0.2 ? decimal.Parse(p.discount) : 1),
                            }).ToList();


                return Json(data);
            }catch (Exception e)
            {
                new Syslog(this.context1).writelog("OrderDeTail/QueryOrderDatailByOrId", e.Message);
                return Json(new { code = 500, msg = e.Message });
            }
        }



        /// <summary>
        /// 获取订单产品明细
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpGet,Route("OrderDeTail/QueryOrderDelTail")]
        [LimitAttr]
        public IActionResult QueryOrderDelTail(int orderid)
        {
            //var data=context1.orderDeTails.Where(x=>x.orderid==orderid).ToList();
            try
            {
                var data = (from p in context1.orderDeTails
                            join x in context1.tb_Products on p.productID equals x.id
                            where p.orderid == orderid
                            select new
                            {
                                x.id,
                                x.productName,
                                images = dbManager.Gethosturl("hosturl") + x.images,
                                x.price,
                                p.quantity
                            }).ToList();

                var pdata = (from p in context1.tb_Orders
                             join x in context1.tb_shop
                              on p.shopid equals x.id
                             join r in context1.tb_users on p.userid equals r.id

                             where p.id == orderid
                             select new
                             {
                                 p.id,
                                 p.ordername,
                                 p.createdate,
                                 p.meney,
                                 p.Remark,
                                 x.person,
                                 x.shopname,
                                 images = dbManager.Gethosturl("hosturl") + (from r in context1.tb_Remarks where r.userid == p.userid select r.images).FirstOrDefault(),
                                 sublist = data
                             }).FirstOrDefault();
                return Json(pdata);
            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("OrderDeTail/QueryOrderDelTail", ex.Message);
                return Json(new {code=500,msg=ex.Message});
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("OrderDeTail/DelRow")]
        [Auther]
        [LimitAttr]
        public IActionResult DelRow(int id)
        {
            try
            {
                var data = context1.orderDeTails.FirstOrDefault(x => x.id == id);
                if (data != null)
                    context1.orderDeTails.Remove(data);
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "删除成功" });
                else return Json(new { code = 0, msg = "删除失败" });
            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("OrderDeTail/DelRow", ex.Message);
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        /// <summary>
        /// 改产品数量
        /// </summary>
        /// <param name="idsparam"></param>
        /// <returns></returns>
        [HttpPost,Route("OrderDeTail/UpdateQu")]
        [LimitAttr]
        public IActionResult UpdateQu([FromBody]idparam idsparam)
        {
            try
            {
                if (idsparam.idsparams.Count == 0) return Json(new { code = 0, msg = "没有可更新的" });
                foreach (var item in idsparam.idsparams)
                {
                    var row = context1.orderDeTails.FirstOrDefault(x => x.id == item.id);
                    row.quantity = item.quty;
                    context1.orderDeTails.Update(row);

                }
                var irows = context1.SaveChanges();
                return Json(new { code = 0, msg = "ok" });
            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("OrderDeTail/UpdateQu", ex.Message);
                return Json(new { code = 500, msg = ex.Message });
            }

        }
        /// <summary>
        /// get edit data
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
       [HttpGet,Route("OrderDeTail/QueryOrderAndDetailByPc")]
        public IActionResult QueryOrderAndDetailByPc(int orderid)
        {
            try
            {
                //var mastkdata = context1.tb_Orders.FirstOrDefault(x => x.id == orderid);
                var detaildata = (from p in context1.tb_Products
                                  join o in context1.orderDeTails on p.id equals o.productID
                                  where o.orderid == orderid
                                  select new
                                  {
                                      o.lastprice,
                                      o.quantity,
                                      p.id,
                                      p.barcode,
                                      p.unit,
                                      p.bulkprice,
                                      p.productName,
                                      p.price,
                                      p.images,
                                      o.discount,
                                      o.discountname
                                  }).ToList();

                var data = (from p in context1.tb_Orders
                            where p.id == orderid
                            select new
                            {
                                p.id,
                                p.shopid,
                                p.ordername,
                                p.ordertype,
                                p.meney,
                                p.userid,
                                p.productcount,

                            }).FirstOrDefault();

                return Json(new { code = 0, msg = "ok", data, subdata = detaildata });
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("OrderDeTail/QueryOrderAndDetailByPc", ex.Message);  
                return Json(new {code=500,msg=ex.Message});
            }
        }


        /// <summary>
        /// 查询所有折扣
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("OrderDeTail/QueryDiscount")]
        public IActionResult QueryDiscount()
        {
            var data =( from p in context1.tb_Discounts
                       select new
                       {
                           value = p.Discount,
                           text = p.DiscountName
                       }).ToList();
            
            return Json(data);
        }


    }
}
