using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace chinacity70sever.Controllers
{
    public class AboutController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly china70Context context1;
        public AboutController(china70Context context, IWebHostEnvironment hostingEnvironment)
        {
            context1 = context;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// get about txt
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("About/GetAboutTxt")]
        public IActionResult GetAboutTxt(string par)
        {
            var path = _hostingEnvironment.WebRootPath + "/" + dbManager.Gethosturl("about_zh");
            if(par== "en-US") path= _hostingEnvironment.WebRootPath + "/" + dbManager.Gethosturl("about_en");
            if(par== "sr-SR") path = _hostingEnvironment.WebRootPath + "/" + dbManager.Gethosturl("about_sr");
            if (System.IO.File.Exists(path))          {
                              
                var cont=System.IO.File.ReadAllLines(path);
                return Json(new { cont });
            }
            return NotFound();
        }
    }
}
