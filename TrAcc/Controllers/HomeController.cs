using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrAcc.Models;
using TrAcc.Utils;

namespace TrAcc.Controllers
{
    public class HomeController : Controller
    {
        ProductContext db = new ProductContext();

        public ActionResult Index()
        {
            var products = db.Products.OrderBy(product => product.Id).Take(20).ToList();

            ViewData["encodings"] = new SelectList(Encoding.GetEncodings().Select(e => e.GetEncoding()), "CodePage", "EncodingName", 1251);

            ViewBag.Start = 0;
            ViewBag.Count = 20;
            ViewBag.Total = db.Products.Count();
            ViewBag.Products = products;
            return View();
        }

        [HttpGet]
        public ActionResult Products(int offset)
        {
            var products = db.Products.OrderBy(product => product.Id).Skip(offset).Take(20).ToList();

            ViewData["encodings"] = new SelectList(Encoding.GetEncodings().Select(e => e.GetEncoding()), "CodePage", "EncodingName", 1251);

            ViewBag.Start = offset;
            ViewBag.Count = 20;
            ViewBag.Total = db.Products.Count();
            ViewBag.Products = products;
            return View("Index");
        }

        [HttpGet]
        public ActionResult AddToStock(string id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        [HttpPost]
        public string AddToStock(ProductInStock product)
        {
            product.AddDate = DateTime.Now;
            db.Stock.Add(product);
            db.SaveChanges();
            return "Готово!";
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase upload, int codepage)
        {
            string error = String.Empty;
            string errLine = string.Empty;
            int errLineNum = 1;
            if (upload != null)
            {
                try
                {
                    var path = Path.GetTempFileName();
                    upload.SaveAs(path);
                    var lines = System.IO.File.ReadLines(path, Encoding.GetEncoding(codepage));
                    db.Products.AddRange(lines.Select(line =>
                    {
                        errLineNum++;
                        return (errLine = line).Split(';');
                    }).Select(data => new Product() { Id = data[0].Trim(), Name = data[1].Trim() }));

                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    error = e.Message;
                    db.UndoChanges();
                }
            }

            ViewBag.ErrorLineNum = errLineNum;
            var encoding = Encoding.GetEncoding(codepage);
            ViewBag.ErrorLineNum = errLineNum;
            ViewBag.ErrorLine = Encoding.UTF8.GetString(Encoding.Convert(encoding, Encoding.UTF8, encoding.GetBytes(errLine)));
            ViewBag.Error = error;
            return View("ProductImportReport");
        }
    }
}