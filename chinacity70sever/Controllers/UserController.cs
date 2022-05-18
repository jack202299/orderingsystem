using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;

namespace chinacity70sever.Controllers
{
    public class UserController : Controller
    {
        public  readonly china70Context context1;
        public UserController(china70Context context)
        {
            context1 = context;
           
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost,Route("User/UserAddorEdit")]
        [LimitAttr]
        public IActionResult UserAddorEdit([FromBody]tb_users users)
        {
            try
            {
                var data = context1.tb_users.FirstOrDefault(x => x.useremail == users.useremail);
                if (data == null) context1.tb_users.Add(users);
                else return Json(new { code = 1, msg = "该邮箱已注册过" });
                var irow = context1.SaveChanges();
                if (irow == 0) return Json(new { code = 0, msg = "没有更新" });
                else return Json(new { code = 0, msg = "保存成功" });
            }catch (Exception ex)
            {
                new Syslog(this.context1).writelog("User / UserAddorEdit", ex.Message);
               
                return Json(new { code = 508, msg = ex.Message });
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost,Route("User/Login")]
        [LimitAttr]
        public IActionResult Login([FromBody]UserLoginParam userLogin)
        {
            var data=context1.tb_users.FirstOrDefault(x=>x.useremail == userLogin.useremail.Trim());
            if (data == null) return Json(new {code=-1,msg= "用户不存在" });
            if (data.pwd != userLogin.Password.Trim()) return Json(new { code = -2, msg = "密码错误" });
            var strtoken = Guid.NewGuid().ToString().ToString().Replace("-", "");
            var tempstr = dbManager.Encrypt(string.Concat(strtoken,"|", data.id,"|", data.pwd));
            var tblog = new tb_loginlog
            {
                id = 0,
                guid = tempstr,
                createdate = DateTime.Now,
                userid = data.id
            };
            context1.tb_loginlog.Add(tblog);
            context1.SaveChanges();
            return Json(new {code=0,msg= "登录成功", data,strtoken= tempstr });
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="editPwd"></param>
        /// <returns></returns>
        [HttpPost,Route("User/EditUpdatePwd")]
        [LimitAttr]
        [Auther]
        public IActionResult EditUpdatePwd([FromBody]EditPwdParam editPwd)
        {
            var data = context1.tb_users.FirstOrDefault(x => x.id == editPwd.UserId);
            if (data == null) return Json(new { code = -1, msg = "用户不存在" });
            if (data.pwd != editPwd.oldpwd.Trim()) return Json(new { code = -2, msg = "旧密码不正确" });
            data.pwd = editPwd.newpwd;
            var irow = context1.SaveChanges();
            if (irow >= 0) return Json(new { code = 1, msg = "更新成功" });
            else return Json(new { code = 0, msg = "没有更新成功" });
        }
         /// <summary>
         /// select userlist by username or email
         /// </summary>
         /// <param name="userParam"></param>
         /// <returns></returns>
        [HttpPost,Route("User/QueryUsers")]
        [LimitAttr]
        public IActionResult QueryUsers([FromBody]QueryUserParam userParam)
        {
            var data=(from p in context1.tb_users where p.name.Contains(userParam.username )|| p.useremail.Contains(userParam.username) && p.Identitytype=="User"
                     select new { p.id,p.name,p.useremail }).ToList();

            return Json(data);
        }
        /// <summary>
        /// get user info by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet,Route("User/QueryUserInfo")]
        [LimitAttr]
        public IActionResult QueryUserInfo(int Id)
        {
            var data=context1.tb_users.FirstOrDefault(r=>r.id==Id);
            if (data == null) return Json(new { code = -1, msg = "用户不存在" });
            var result = new {data.id,data.Identitytype };
            return Json(new { code = 0, result });
        }
        /// <summary>
        /// save shop user
        /// </summary>
        /// <param name="shopuser"></param>
        /// <returns></returns>
        [HttpPost,Route("User/AddorEditshopuser"),LimitAttr,Auther]
        public IActionResult AddorEditshopuser([FromBody]tb_shopuser shopuser)
        {
            try
            {
                if(shopuser.id==0) context1.tb_Shopusers.Add(shopuser);
                else context1.tb_Shopusers.Update(shopuser);
                var irows=context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "保存成功" });
                else return Json(new { code = 1, msg = "保存失败" });

            }catch (Exception ex)
            {
                return Json(new {code=404,msg=ex.Message});
            }
        }
        /// <summary>
        /// select shop user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost,Route("User/QueryShopUser"),LimitAttr]
        public IActionResult QueryShopUser([FromBody]QueryShopUserParam param)
        {
            try
            {
                if (param == null) return Json(new { code = 403, msg = "parameter can not empty" });
                var data = (from p in context1.tb_Shopusers
                           join r in context1.tb_shop on p.shopid equals r.id
                           join x in context1.tb_users on p.userid equals x.id
                           where p.shopid == param.shopid
                           select new
                           {
                              p.id,
                              r.shopname,
                              x.name,
                              x.pwd,
                              x.tel,
                              x.useremail ,
                              userid=x.id
                           });
                var temp=data.Skip((param.page-1)*param.pagesize).Take(param.pagesize).ToList();
                return Json(new {icount=data.Count(),result=temp });

            }catch (Exception ex)
            {
                return Json(new { code = 404, msg =ex.Message });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("User/QueryUserList")]
        public IActionResult QueryUserList()
        {
            try
            {
               var data=(from p in   context1.tb_users select new {p.id,p.name}).ToList();

                return Json(new { code = 0, msg = "", data });

            }catch (Exception ex)
            {
                return Json(new { code = 404, msg = ex.Message });
            }
        }
        /// <summary>
        /// get the shop users by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("User/QueryShopUserInfo")]
       public IActionResult QueryShopUserInfo(int id)
        {
            try
            {
                var data = context1.tb_Shopusers.FirstOrDefault(x => x.id==id);
                return Json(new { code = 0, msg = "ok", data });
            }catch(Exception ex)
            {
                return Json(new {code=404,msg= ex.Message});
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("User/deleterow")]
        public IActionResult deleterow(int id)
        {
            try
            {
                var data = context1.tb_Shopusers.FirstOrDefault(x => x.id == id);
              
                var rows = context1.tb_Permissions.Where(r => r.userid == data.userid).ToList();
                context1.tb_Shopusers.Remove(data);
                context1.tb_Permissions.RemoveRange(rows);
                var irows = context1.SaveChanges();
                return Json(new { code = 0, msg = "delete success" });

            }catch (Exception ex)
            {
                return Json(new {code=404,msg=ex.Message});
            }
        }
        /// <summary>
        /// query the users by code name
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpGet,Route("User/QueryUserByName")]
        public IActionResult QueryUserByName(string par)
        {
            try
            {
                var data =( from p in context1.tb_users where p.name.Contains(par) select new { value = p.id, text = p.name }).ToList();
                return Json(new { code = 0, msg = "ok", data });
            }catch (Exception ex)
            {
                return Json(new {code=500,msg=ex.Message});
            }
        }
        /// <summary>
        /// query users by name with page
        /// </summary>
        /// <param name="queryuser"></param>
        /// <returns></returns>
        [HttpPost,Route("User/QueryUserLs")]
        public IActionResult QueryUserLs([FromBody]queryuserParam queryuser)
        {
            try
            {
                if (queryuser == null) return Json(new { code = 400, msg = "参数不能为空" });
                var icount = context1.tb_users.Count(x => x.name.Contains(queryuser.custtxt) && x.Identitytype=="User");
                var data = context1.tb_users.Where(x => x.name.Contains(queryuser.custtxt) && x.Identitytype == "User").Skip((queryuser.page - 1) * queryuser.pagesize).Take(queryuser.pagesize).ToList();
                return Json(new {code=0,msg="ok",icount,data});

            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("User/QueryUserLs", ex.Message);
                return Json(new {code=500,msg=ex.Message}); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost,Route("User/AddorEditUsers")]
        public IActionResult AddorEditUsers([FromBody]tb_users users)
        {
            try
            {
                if (users == null) return Json(new { code = 400, msg = "参数不能留空" });
                if(users.id==0) context1.tb_users.Add(users);
                else context1.tb_users.Update(users);
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "保存成功" });
                else return Json(new { code = 1, msg = "保存失败" });

            }catch(Exception ex)
            {
                new Syslog(this.context1).writelog("User/AddorEditUsers", ex.Message);
                return Json(new {code=500,msg=ex.Message});
            }
        }
        /// <summary>
        /// delete user by id
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost,Route("User/deleteUsersById")]
        public IActionResult deleteUsersById([FromBody]tb_users users)
        {
            try
            {
                if (users == null) return Json(new { code = 400, msg = "参数不能留空" });
                context1.tb_users.Remove(users);
                var irows = context1.SaveChanges();
                if (irows > 0) return Json(new { code = 0, msg = "保存成功" });
                else return Json(new { code = 1, msg = "保存失败" });
            }
            catch(Exception ex)
            {
                new Syslog(this.context1).writelog("User/deleteUsersById", ex.Message);
                return Json(new { code = 500, msg = ex.Message });
            }
        }
    }
}
