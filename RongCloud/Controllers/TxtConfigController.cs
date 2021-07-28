using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;

namespace RongCloud.Controllers
{
    public class TxtConfigController : Controller
    {
        // GET: TxtConfig
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ReadFile(string fileName)
        {
            var filePath = Server.MapPath("~/" + fileName);
            var fileContent = await FileHelper.Instance.ReadFileAsync(filePath);
            return Json(new { code = 200, result = fileContent });
        }

        public async Task<ActionResult> SaveFile(string fileName, string fileContent)
        {
            var filePath = Server.MapPath("~/" + fileName);
            await FileHelper.Instance.WriteFileAsync(filePath, fileContent);
            return Json(new { code = 200, result = fileContent });
        }

        public async Task<ActionResult> ReadFileHttp(string fileName)
        {
            var filePath = fileName;//http://localhost:8080/
            var fileContent = await FileHelper.Instance.ReadFileHttpAsync(filePath);
            return Json(new { code = 200, result = fileContent });
        }
    }
}