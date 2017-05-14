using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    //[Authorize]  //可以在Action或是Controller整體加上登入驗證
    public class ProductsController : BaseController
    {
        //private FabricsEntities db = new FabricsEntities();
        //private ProductRepository repo = RepositoryHelper.GetProductRepository();
        //如果屬性不加，預設就是private. 不能被繼承

        //[RequireHttps] //會自動將URL轉址到https的路徑，如果沒有web server https就會連到空網頁
        // GET: Products
        [OutputCache(Duration=5, Location=System.Web.UI.OutputCacheLocation.Server)] //加入輸出快取,Duration單位為秒
        public ActionResult Index(bool Active = true)
        {
            //Sol1: using entity framework
            /* 
             var data = db.Product
                .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
             */
            //Sol2: using repository
            /*
            ProductRepository repo2 = new ProductRepository();
            repo2.UnitOfWork = new EFUnitOfWork();
            var data = repo2.All().Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
            */
            /*
            var data = repo.All()
                .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
             */
            
            //Sol3: using repositoty with biz logic moved to model
            var data = repo.GetAllRecords(Active);


            ViewData.Model = data;//return View(data)
            ViewData["foo"] = data;
            ViewBag.foo = data;
            TempData["foo"] = data;

            return View();
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //using entity framework
            /*
            Product product = db.Product.Find(id);
            */

            //using repository
            Product product = repo.GetRecordByProductId(id.Value);
            
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            //if (true) //故意產生DbEntityValidationException
            {
                //sol1
                /*
                db.Product.Add(product);
                db.SaveChanges();
                 */
                //sol2: using repository
                repo.Add(product);
                repo.UnitOfWork.Commit();

                TempData["CreateSuccess"] = "商品新增成功";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //sol1
            /*
             Product product = db.Product.Find(id);
             */
            //sol2
            Product product = repo.GetRecordByProductId(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Ver1: model binding
        //public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)

        //Ver2: 使用延遲模行驗證，先不傳入強型別model
        public ActionResult Edit(int id, FormCollection foo)
        {
            //Ver1
            /*
            if (ModelState.IsValid)            
            {
                repo.Update(product);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
             */

            //Ver2: 在Action裡面才去做model binding，而非進入Action之前做model binding
            var product = repo.GetRecordByProductId(id);
            //var product = new Product();//如果這樣寫的話，就等於全部都是拿預設值，然後等下再用ModelState來更新
            if(TryUpdateModel<Product>(product)) // TryUpdateModel 是自動從<Product>的ModelState裡面找有上傳的欄位來更新上面這個var product的內容值
            {
                //sol1
                /*
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                 */
                //sol2
                repo.Update(product);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //sol1
            /*
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
             */ 
            //sol2
            Product product = repo.GetRecordByProductId(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //sol1
            /*
            Product product = db.Product.Find(id);             
            product.IsDel = true;
            //db.Product.Remove(product);
            db.SaveChanges();
             */ 
            //sol2
            //暫時關閉驗證
            repo.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            Product product = repo.GetRecordByProductId(id);
            product.IsDel = true;
            repo.UnitOfWork.Commit();
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            /*if (disposing)
            {
                db.Dispose();
            }*/
            base.Dispose(disposing);
        }

        //public ActionResult ListProducts(string q)
        public ActionResult ListProducts_bak(FormCollection form) //使用弱型別的時候沒有model binding所以也沒有model state，所以回傳到view後欄位為空值(沒有model state值)
        {
            //sol1
            /*
            var data = db.Product
                .Where(p => p.Active == true)
                .Select(p => new ProductLiteVM()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .Take(10);
             */
            
            var data = repo.GetAllRecords(Active: true);
            if (!String.IsNullOrEmpty(form["q"]))
            {
                var keyword = form["q"];
                data = data.Where(p => p.ProductName.Contains(keyword));
            }

            if (!String.IsNullOrEmpty(form["s1"]) && !String.IsNullOrEmpty(form["s2"]))
            {
                var stock1 = Convert.ToInt32(form["s1"]);
                var stock2 = Convert.ToInt32(form["s2"]);                
                data = data.Where(p => p.Stock > stock1 && p.Stock < stock2);
            }

            //sol2
             ViewData.Model = data.Select(p => new ProductLiteVM()  //選訂你要哪些欄位作輸出
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .Take(10);

           
            return View();
        }

        public ActionResult ListProducts(ProductListSearchVM model) //使用弱型別的時候沒有model binding所以也沒有model state，所以回傳到view後欄位為空值(沒有model state值)
        {
            //sol1
            /*
            var data = db.Product
                .Where(p => p.Active == true)
                .Select(p => new ProductLiteVM()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .Take(10);
             */

            var data = repo.GetAllRecords(Active: true);
            if (!String.IsNullOrEmpty(model.q))
            {
                var keyword = model.q;
                data = data.Where(p => p.ProductName.Contains(keyword));
            }

            if ((model.s1 != null) && (model.s2 != null)) //沒有帶入時，會變成0
            {
                var stock1 = model.s1;
                var stock2 = model.s2;
                data = data.Where(p => p.Stock >= stock1 && p.Stock <= stock2);
            }

            //sol2
            ViewData.Model = data.Select(p => new ProductLiteVM()  //選訂你要哪些欄位作輸出
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Stock = p.Stock
            }).Take(10);


            return View();
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductLiteVM data)
        {
            if (ModelState.IsValid)
            {
                // TODO: 儲存資料進資料庫

                return RedirectToAction("ListProducts");
            }
            // 驗證失敗，繼續顯示原本的表單
            return View();
        }
    }
}
