using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
namespace chinacity70sever.BLL
{
    public class LimitAttr: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!IPCacheManager.CheckIsAble(ip)) context.Result = new JsonResult(new { code = 503, msg = "访问过多" });
        }
    }
}
