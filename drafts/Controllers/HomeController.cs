using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace drafts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/files/"));
            List<ListItem> files = new List<ListItem>();
            foreach (string filePath in filePaths)
            {
                files.Add(new ListItem(Path.GetFileName(filePath), filePath));
            }
            ViewBag.files = files;
            return View();
        }

        [HttpPost]
        public void DownloadFiles()
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                string[] s = Request.Form["chkselect"].Split(',');
                foreach (string value in s)
                {
                    string filePath = value.ToString();
                    zip.AddFile(filePath, "");
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }
}
