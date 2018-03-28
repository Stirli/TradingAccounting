using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;

namespace TrAcc.Models
{
    public class ProductDbInitializer : DropCreateDatabaseAlways<ProductContext>
    {
        public ProductDbInitializer()
        {
            
        }
        protected override void Seed(ProductContext db)
        {
            var lines = System.IO.File.ReadLines(@"D:\GIT\TrAccRepos\TrAcc\App_Data\Книга1.csv", Encoding.GetEncoding(1251));
            db.Products.AddRange(lines.Take(123).Select(line => line.Split(';')).Select(data => new Product() { Id = data[0].Trim(), Name = data[1].Trim() }));

            base.Seed(db);
        }
    }
}