using chinacity70sever.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace chinacity70sever.BLL
{
    public class Auther: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tok=context.HttpContext.Request.Headers["token"].ToString();
            
            if (string.IsNullOrEmpty(tok))
            {
                context.Result = new JsonResult(new { code = 505, msg = "请登录" });
            }
            else
            {
                var str=dbManager.Decrypt(tok).Split("|");
                var id = str[1];
                var datacontex = ContextService.GetContext();
                var data = datacontex.tb_users.FirstOrDefault(x => x.id == Convert.ToInt32(id));
                if (data.pwd != str[2]) context.Result = new JsonResult(new { code = 505, msg = "请登录" });

            }
        }
    }
}
