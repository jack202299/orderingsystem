using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace chinacity70sever.Controllers
{
    public class OrderController : Controller
    {
        private readonly china70Context context1;
        public OrderController(china70Context context)
        {
            context1 = context;
        }
        /// <summary>
        /// 增加订单
        /// </summary>
        /// <param name="orderparam"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/AddOrEditOrder")]
        [Auther]
        [LimitAttr]
        public IActionResult AddOrEditOrder([FromBody]orderparam orderparam)
        {
            try
            {
                if (orderparam == null) return Json(new { code = 2, msg = "参数不能为空" });
                var orderdata = context1.tb_Orders.FirstOrDefault(x => x.id == orderparam.id);
                int idd = 0;
                string orderid = "";
                if (orderdata != null)
                {
                    orderdata.shipsid = orderparam.shipsid;
                    orderdata.ordername = orderparam.ordername;
                    orderdata.userid = orderparam.userid;
                    orderdata.blfalge = orderparam.blfalge;
                    //orderdata.createdate = DateTime.Now;
                    orderdata.shopid = orderparam.shopid;
                    orderdata.Remark = orderparam.Remark;
                    orderdata.meney = orderparam.meney;
                    orderdata.productcount = orderparam.productcount;
                    orderdata.ordertype = orderparam.ordertype;
                    orderdata.paystatus = 100;
                    orderdata.paymethod = orderparam.paymethod;
                    context1.tb_Orders.Update(orderdata);
                    context1.SaveChanges();
                    idd = orderdata.id;
                    orderid = orderdata.ordername;
                }
                else
                {
                    var order = new tb_orders();
                    order.shipsid = orderparam.shipsid;
                    order.ordername = DateTime.Now.ToString("yyyyMMddHHmmssfff");// orderparam.ordername;
                    order.userid = orderparam.userid;
                    order.blfalge = orderparam.blfalge;
                    order.createdate = DateTime.Now;
                    order.shopid = orderparam.shopid;
                    order.Remark = orderparam.Remark;
                    order.meney = orderparam.meney;
                    order.ordertype = orderparam.ordertype;
                    order.productcount=orderparam.productcount;
                    order.paymethod = orderparam.paymethod;
                    order.paystatus = 100;
                    context1.tb_Orders.Add(order);
                    context1.SaveChanges();
                    idd = order.id;
                    orderid = order.ordername;
                }

                var orderdeltal = context1.orderDeTails.Where(x => x.orderid == orderparam.id).ToList();
                if (orderdeltal != null) context1.orderDeTails.RemoveRange(orderdeltal);
                foreach (var item in orderparam.orderDeTails)
                {
                    var data = context1.tb_Products.FirstOrDefault(r => r.id == item.productID);
                    data.sellcount += item.quantity;
                    data.reserve-=item.quantity;
                    item.orderid = idd;
                }
                context1.orderDeTails.AddRange(orderparam.orderDeTails);
                var datt = context1.tb_shoppingcarts.Where(x => x.shopid == orderparam.shopid).ToList();
                if (datt != null) context1.tb_shoppingcarts.RemoveRange(datt);
                var irows = context1.SaveChanges();
                if (irows > 0)
                {

                    var hosturl = dbManager.Gethosturl("hosturl");
                    var user = context1.tb_users.FirstOrDefault(x => x.id == orderparam.userid);
                    var ti = new StringBuilder();
                    ti.AppendFormat("{0}的订单", user.name);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("<span>尊敬的老板，您有{0}来自{1}订单编号：{2},请注意查收</span><br>", DateTime.Now.ToString("yyyy-MM-dd"), user.name, orderid);
                    sb.AppendFormat("<a href='{0}printpages.html?id={1}' target='_blank'>点击这里查看订单</a><br>", hosturl, idd);
                    var su = (from p in context1.tb_users where (from x in context1.tb_shop where x.id == orderparam.shopid select x.userid).Contains(p.id) select p).FirstOrDefault();


                    // dbManager.SendMail(ti.ToString(), sb.ToString(), su.useremail);

                    Task.Run(() =>
                    {
                        var msg = dbManager.SendMail(ti.ToString(), sb.ToString(), su.useremail);
                        // if (msg.code == 1) return Json(msg);

                    });
                    var tc = context1.tb_Clientages.FirstOrDefault(x => x.userid == orderparam.userid && x.shopid == orderparam.shopid);
                    if (tc == null)
                    {
                        tc=new tb_clientage { shopid = orderparam.shopid ,userid=orderparam.userid,id=0};
                        context1.tb_Clientages.Add(tc);
                        
                    }
                    var sendinfo = new tb_sendtask
                    {
                        id = 0,
                        tomail = su.useremail,
                        blfage = true,
                        body = sb.ToString(),
                        title = ti.ToString(),
                        createdate = DateTime.Now.ToString()
                    };
                    context1.tb_Sendtasks.Add(sendinfo);
                    context1.SaveChanges();
                    return Json(new { code = 0, msg = "订单提交成功" });
                }
                else return Json(new { code = 1, msg = "保存失败" });
            }catch (Exception ex)
            {
                var log = new tb_syslog();
                log.errmsg = ex.Message;
                log.fun = "Order/AddOrEditOrder";
                log.createdate = DateTime.Now.ToString();
                log.id = 0;
                context1.tb_Syslogs.Add(log);
                context1.SaveChanges();
                return Json(new { code = 1,msg=ex.Message});
            }

        }
        /// <summary>
        /// 查询商店订单
        /// </summary>
        /// <param name="shopIdParam"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/QueryOrderList")]
        [LimitAttr]
        public IActionResult QueryOrderList([FromBody]OrderListWithShopIdParam shopIdParam) 
        {
           // var data = context1.tb_Orders.Where(x => x.shopid == shopIdParam.shopid && x.blfalge==shopIdParam.blfalge ).ToList();
            var data=(from p in  context1.tb_Orders 
                     where p.shopid==shopIdParam.shopid && p.blfalge==shopIdParam.blfalge
                     && p.createdate==Convert.ToDateTime( shopIdParam.orderDate)
                     select new { p.id,p.ordername,p.createdate,p.meney,p.Remark,p.shopid,
                         person = (from r in context1.tb_users where r.id==p.userid select r.name).FirstOrDefault(),
                         blfalge=p.blfalge==0?"等出货":"已出货" }).OrderByDescending(x=>x.id).ToList();
            return Json(new { code = 0, msg = "查询成功",  data });
        }
        /// <summary>
        /// select ordery by month and shop
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost, Route("Order/QueryOrderByMonths"), LimitAttr, Auther]
        public IActionResult QueryOrderByMonths([FromBody] OrderQueryByMonthPararm order)
        {
            try
            {
                var strd = Convert.ToDateTime(order.strd);
                var endd = Convert.ToDateTime(order.endd);
                var us = (from p in context1.tb_users where p.name.Contains(order.txt) select p.id).ToList();
                var data = (from p in context1.tb_Orders
                            where p.shopid == order.shopid   &&  (p.createdate >= strd && p.createdate <= endd) && p.blfalge!=2
                            && (us.Contains(p.userid) || p.ordername.Contains(order.txt))
                            select new {
                                p.id,
                                p.ordername,
                                p.createdate,
                                p.meney,
                                p.actuallyreceived,
                                p.Remark,
                                p.shopid,
                                p.productcount,
                                p.paymethod,
                                p.paystatus,
                                p.userid, 
                                p.ordertype,
                                person = (from r in context1.tb_users where r.id == p.userid select r.name).FirstOrDefault(),
                                blfalge = p.blfalge == 0 ? "等出货" : "已出货"
                            }).OrderByDescending(x => x.id).ToList();

                var icount = data.Count();
                var result = data.Skip((order.page - 1) * order.pagesize).Take(order.pagesize).ToList();
                return Json(new {code=0,msg="ok",icount,result});

            }
            catch (Exception ex)
            {
                return Json(new {code=500,msg=ex.Message});
            }
        }

        /// <summary>
        /// get the order list with user
        /// </summary>
        /// <param name="userIdParam"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/QueryOrderListforUser")]
        [LimitAttr]
        public IActionResult QueryOrderListforUser([FromBody]OrderListWithUserIdParam userIdParam)
         {
            // var icount = context1.tb_Orders.Count(x => x.userid == userIdParam.userId && x.blfalge == userIdParam.blfalge);
            // var data=context1.tb_Orders.Where(x=>x.userid==userIdParam.userId && x.blfalge==userIdParam.blfalge).ToList();
            try
            {
                var stdate = Convert.ToDateTime(userIdParam.orderDate.Split('|')[0]);
                var etdate = Convert.ToDateTime(userIdParam.orderDate.Split('|')[1]);
                var data = (from p in context1.tb_Orders
                            join r in context1.tb_shop on p.shopid equals r.id
                            where p.userid == userIdParam.userId && p.blfalge == userIdParam.blfalge
                           && p.createdate >= stdate && p.createdate <= etdate
                            select new
                            {
                                p.id,
                                p.ordername,
                                p.blfalge,
                                p.meney,
                                p.shopid,
                                p.Remark,
                                p.createdate,
                                p.userid,
                                r.shopname,
                                person = (from r in context1.tb_users where r.id == p.userid select r.name).FirstOrDefault(),
                            }).OrderByDescending(x => x.id).ToList();
                return Json(new { data });
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Order/QueryOrderListforUser", ex.Message);
                return Json(new {code=500,msg= ex.Message });
            }
         }
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/UpdateOrderStatus")]
        [LimitAttr]
        [Auther]
        public IActionResult UpdateOrderStatus([FromBody]OrderStatusParam order)
        {
            if (order == null) return Json(new { code = 0,msg = "没有参数" });
            var data=context1.tb_Orders.FirstOrDefault(x=>x.id==order.OrderId);
            data.blfalge = order.blfage;
            context1.tb_Orders.Update(data);
            var irows=context1.SaveChanges();
            return Json(new { code = 1, msg = "保存成功" });
        }
        /// <summary>
        /// sum money by every month
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/QuerySumByMonth")]
        [LimitAttr]
        public IActionResult QuerySumByMonth([FromBody]QueryProductUpload query)
        {
            if (query.querydate.Count() == 0) return Json(new { code = 501, msg = "Parameters cannot be missing" });
            var strd = Convert.ToDateTime(query.querydate[0]);
            var endd = Convert.ToDateTime(query.querydate[1]);
           
            var data = context1.tb_Orders.Where(x=>x.shopid==query.shopid && x.blfalge==1).ToList();
            var temp=from p in data
                       where Convert.ToDateTime(p.createdate) >= strd && Convert.ToDateTime(p.createdate) < endd
                       group p by new { o = Convert.ToDateTime(p.createdate).ToString("yyyy-M-d") } into g
                       select new {date= g.Key.o, m = g.Sum(x => x.meney),icount=g.Count() };

            return Json(temp);

        }
        /// <summary>
        /// get print page the url and id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("Order/QueryPrintPage")]
        [LimitAttr]
        public IActionResult QueryPrintPage(int Id,int ordertype)
        {
            var usid = context1.tb_Orders.FirstOrDefault(x => x.id == Id);
            if (usid.userid == 0) return Json(new { code = 400, msg = "Pos订单不能打印发票" });
            var page = dbManager.Gethosturl("printpage")+ "?id="+Id+ "&ordertype="+ordertype;
            return Json(page);
        }

        /// <summary>
        /// get the print invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Order/QueryPrintInvoice")]
        public IActionResult QueryPrintInvoice(int id,int vd)
        {
            var usid = context1.tb_Orders.FirstOrDefault(x => x.id == id);
            if (usid.userid == 0) return Json(new { code = 400, msg = "Pos订单不能打印发票" });
            var page = dbManager.Gethosturl("hosturl") + "printinvoice.html?id=" + id+"&vd="+vd;
            return Json(page);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("Order/QueryPosPager")]
        public IActionResult QueryPosPager(string id)
        {
            var page = dbManager.Gethosturl("hosturl") + "posprint.html?id=" + id;
            return Json(page);
        }
        /// <summary>
        /// delete order by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("Order/DelOrder")]
        [LimitAttr]
        [Auther]       
        public IActionResult DelOrder(int Id)
        {
            var data=context1.tb_Orders.FirstOrDefault(r=>r.id==Id);
            if (data == null) return Json(new { code = 4000, msg = "订单不存在" });
            var delal=context1.orderDeTails.Where(r=>r.orderid==Id);
            context1.orderDeTails.RemoveRange(delal);
            context1.tb_Orders.Remove(data);
            var irows = context1.SaveChanges();
            if (irows > 0) return Json(new { code = 0, msg = "delete success" });
            return Json(new { code = 1, msg = "delete fail" });
        }
        /// <summary>
        /// 保存从电脑端上传的数据
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/AddorEditOrderForPc")]
        public IActionResult AddorEditOrderForPc([FromBody]orderAddParam order)
        {
            try
            {
                var data = context1.tb_Orders.FirstOrDefault(x => x.ordername == order.ordername);
                if (data == null)
                {
                    var tborder = new tb_orders
                    {
                     blfalge=0,
                     createdate=DateTime.Now,
                     meney=order.meney,
                     ordername=order.ordername,
                     ordertype=order.ordertype,
                     paymethod=1,
                     paystatus=100,
                     productcount=order.productcount,
                     userid=order.userid,
                     shopid=order.shopid,
                     Remark=String.Empty,
                     shipsid=0,
                     actuallyreceived=order.actuallyreceived
                    };
                    context1.tb_Orders.Add(tborder);                   
                    context1.SaveChanges();
                    var idd = tborder.id;
                    var orderdeltal = context1.orderDeTails.Where(x => x.orderid == tborder.id).ToList();
                    if (orderdeltal != null) context1.orderDeTails.RemoveRange(orderdeltal);
                    foreach (var item in order.orderDeTails)
                    {
                        var de = context1.tb_Products.FirstOrDefault(r => r.id == item.productID);
                        de.sellcount += item.quantity;
                        de.reserve-=item.quantity;
                        item.orderid = idd;
                    }
                    context1.orderDeTails.AddRange(order.orderDeTails);
                    var datt = context1.tb_shoppingcarts.Where(x => x.shopid == order.shopid && x.userid==order.userid).ToList();
                    if (datt != null) context1.tb_shoppingcarts.RemoveRange(datt);
                    var irows = context1.SaveChanges();


                }
                else
                {
                    data.userid = order.userid;
                    data.shopid = order.shopid;
                    data.meney=order.meney;
                    data.ordername = order.ordername;
                    data.ordertype = order.ordertype;
                    data.productcount = order.productcount;
                    data.actuallyreceived = order.actuallyreceived;
                    context1.tb_Orders.Update(data);
                    var orderdeltal = context1.orderDeTails.Where(x => x.orderid == data.id).ToList();
                    if (orderdeltal != null) context1.orderDeTails.RemoveRange(orderdeltal);
                    foreach (var item in order.orderDeTails)
                    {
                        var de = context1.tb_Products.FirstOrDefault(r => r.id == item.productID);
                        de.sellcount += item.quantity;
                        de.reserve -= item.quantity;
                        item.orderid = data.id;
                    }
                    context1.orderDeTails.AddRange(order.orderDeTails);
                    var datt = context1.tb_shoppingcarts.Where(x => x.shopid == order.shopid && x.userid == order.userid).ToList();
                    if (datt != null) context1.tb_shoppingcarts.RemoveRange(datt);
                    var irows = context1.SaveChanges();
                }
                return Json(new { code = 0, msg = "保存成功" });
                
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Order/AddorEditOrderForPc", ex.Message);
                return Json(new {code=500,msg=ex.Message});
            }
        }
        /// <summary>
        /// order invalid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost,Route("Order/OrderEdit")]
        public IActionResult OrderEdit([FromBody]updateRowParam param)
        {
            try
            {
                var data = context1.tb_Orders.FirstOrDefault(x => x.id == param.orderid);
                if (data == null) return Json(new { code = 400, msg = "订单不存在" });
                data.blfalge = param.blfalge;
                data.paystatus= param.paystatus;
                context1.tb_Orders.Update(data);
               var irows= context1.SaveChanges();
              if(irows>0)  return Json(new { code = 0, msg = "更新成功" });
                return Json(new { code = 1, msg = "更新失败" });
            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("Order/OrderEdit", ex.Message);
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        /// <summary>
        /// print invoice
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ordertypeid"></param>
        /// <returns></returns>
        [HttpGet,Route("Order/Printinvoice")]
        public string Printinvoice(int Id)
        {
            try
            {
                var data = (from p in context1.tb_Orders
                            join s in context1.tb_shop on p.shopid equals s.id
                            join u in context1.tb_users on s.userid equals u.id

                            where p.id == Id
                            select new
                            {
                                p.ordername,
                                s.shopname,
                                s.bankaccount,
                                s.tel,
                                s.shopaddress,
                                fromperson = s.person,
                                fromppib = u.pib,
                                frompmb = u.mb,
                                fromcompany = u.company,
                                u.useremail,
                                scity = u.city
                            }).FirstOrDefault();


                var cdata = (from p in context1.tb_Orders
                             join u in context1.tb_users on p.userid equals u.id
                             where p.id == Id
                             select new
                             {
                                 cpib = u.pib,
                                 cname = u.name,
                                 ccompany = u.company,
                                 caddress = u.address,
                                 cmb = u.mb,
                                 ctel = u.tel,
                                 ccity = u.city
                             }).FirstOrDefault();


                var ddata = (from od in context1.orderDeTails
                             join p in context1.tb_Products
                             on od.productID equals p.id
                             where od.orderid == Id
                             select new
                             {
                                 p.alias,
                                 p.unit,
                                 price = p.price,
                                 od.discount,
                                 od.quantity,
                                 amount = p.price * od.quantity,

                             }).ToList();

                decimal? amount1 = 0;
                decimal? amount2 = 0;
                foreach (var d in ddata)
                {
                    if (d.discount == "0.1") amount1 += d.amount * decimal.Parse(d.discount);
                    if (d.discount == "0.2") amount2 = d.amount * decimal.Parse(d.discount);
                }
                var pdata = new
                {
                    data.fromppib,
                    data.frompmb,
                    data.bankaccount,
                    data.tel,
                    data.fromcompany,
                    data.fromperson,
                    data.ordername,
                    data.shopname,
                    data.shopaddress,
                    data.useremail,
                    data.scity,
                    cdata.cname,
                    cdata.caddress,
                    cdata.ccompany,
                    cdata.cmb,
                    cdata.ctel,
                    cdata.ccity,
                    cdata.cpib,
                    sumtol = ddata.Sum(x => x.amount),
                    amount1,
                    amount2,
                };
                var ls = new List<dynamic>();
                ls.Add(pdata);
                var dd = new { Detail = ddata, Master = ls };

                return Newtonsoft.Json.JsonConvert.SerializeObject(dd);
            }catch (Exception ex)
            {
                return null;
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet,Route("Order/QueryPosOrderByOrdername")]
        public IActionResult QueryPosOrderByOrdername(string order)
        {
            try
            {
                var data = from p in context1.orderDeTails
                           join o in context1.tb_Orders on p.orderid equals o.id
                           join pp in context1.tb_Products on p.productID equals pp.id
                           where o.ordername == order
                           select new
                           {
                               productID = pp.id,
                               productName = pp.productName,
                               price = pp.price,
                               barcode = pp.barcode,
                               bulkprice = pp.bulkprice,
                               unit = pp.unit,
                               quantity = 1,
                               discount = 1,
                               discountname = "无折扣",
                               amount = pp.price,

                           };
                var pdata =( from p in context1.tb_Orders
                            where p.ordername == order
                            join s in context1.tb_shop on p.shopid equals s.id
                            select new
                            {
                                ordername = p.ordername,
                                shopname = s.shopname,
                                actuallyreceived = p.actuallyreceived,
                                meney = p.meney
                            }).FirstOrDefault();
                return Json(new {data,pdata});
            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("", ex.Message);
                return Json(new { code = 500, msg = ex.Message });
            }
        }
    }
}
