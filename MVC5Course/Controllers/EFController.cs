using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class EFController : BaseController
    {
        FabricsEntities db = new FabricsEntities();

        public ActionResult Index()
        {
            var all = db.Product.AsQueryable();

            var data = all
                .Where(p => p.Active == true && p.ProductName.Contains("Black") && p.IsDel == false)
                .OrderByDescending(p => p.ProductId);

            //var data1 = all.Where(p => p.ProductId == 1);
            //var data2 = all.FirstOrDefault(p => p.ProductId == 1);
            //var data3 = db.Product.Find(1);

            return View(data);
        }

        public ActionResult Details(int id)
        {
            var data = db.Database.SqlQuery<Product>("SELECT * FROM dbo.Product WHERE ProductId=@p0", id).FirstOrDefault();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var item = db.Product.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                var item = db.Product.Find(id);
                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Stock = product.Stock;
                item.Active = product.Active;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Delete(int id)
        {
            var product = db.Product.Find(id);
            /*
            //先刪除關聯的訂單
            //Method 1
            foreach (var item in product.OrderLine.ToList())
            {
                db.OrderLine.Remove(item);
            }
            //Method 2
            db.OrderLine.RemoveRange(product.OrderLine);

            //再刪除產品
            db.Product.Remove(product);
            */
            product.IsDel = true;
            try {
                db.SaveChanges();//此時會送出SQL Transaction，集中處理上面的所有SQL
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }
    }
}