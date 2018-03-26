using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            IEnumerable<Product> products = db.Products;
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            long count = 0;
            List<string> errors = new List<string>();
            if (upload != null)
            {
                using (var fr = new FileReader(upload.InputStream))
                {
                    var lines = fr.ReadLines().Skip(1);
                    foreach (var line in lines)
                    {
                        try
                        {
                            var data = line.Split(';');
                            var product = new Product() { Id = long.Parse(data[0]), Name = data[1] };
                            if (db.Products.AsEnumerable().Contains(product))
                            {
                                throw new ArgumentException(product.Id + " уже существует");
                            }
                            else
                            {
                                db.Products.Add(product);
                            }

                            count++;
                        }
                        catch (Exception e)
                        {
                            errors.Add("Ошибка в строке " + (count) + "\n" + e.Message);
                        }
                    }
                }
                db.SaveChanges();
            }

            ViewBag.Imported = count;
            ViewBag.Errors = errors;
            return View("ProductImportReport");
        }
    }
}