using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shops.Models;

namespace Shops.Controllers
{
    public class ProductController : Controller
    {
        private ShopsEntities db = new ShopsEntities();
        private const string ALL = "All";

        //
        // GET: /Product/
        public ActionResult Index(string shopFilter, int? maxPrice)
        {
            List<String> list = (from s in db.shops select s.name).Distinct().ToList();
            list.Insert(0, ALL);
            
            ViewBag.ShopList = list;
            ViewBag.ShopFilter = shopFilter;
            var products = db.products.Include(p => p.shop);
            ProductViewModel VM = new ProductViewModel();

            products = products.OrderByDescending(p => p.price);
            if ((shopFilter != null) && (shopFilter != ALL))
            {
                products = products.Where(p => p.shop.name == shopFilter);
            }
            if ((maxPrice != null))
            {
                products = products.Where(p => p.price <= maxPrice);
            }

            VM.products = products.ToList();
            return View(VM);
        }



        //TODO
        public ActionResult GetProducts(string shopFilter, int? maxPrice)
        {
            JsonResult result;
            try
            {
                var products = db.products.Include(p => p.shop);

                if ((shopFilter != null) && (shopFilter != ALL))
                {
                    products = products.Where(p => p.shop.name == shopFilter);
                }
                if ((maxPrice != null))
                {
                    products = products.Where(p => p.price <= maxPrice);
                }

                result = Json(products.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                String errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage = ex.InnerException.Message.Substring(0, Math.Max(ex.InnerException.Message.Length, 20));
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return result;
        }


        //
        // GET: /Product/Details/5
        public ActionResult Details(int id = 0)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        //
        // GET: /Product/Create
        [Authorize(Users = "administrator")]
        public ActionResult Create()
        {
            ViewBag.shop_id = new SelectList(db.shops, "id", "name");
            return View();
        }


        //
        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "administrator")]
        public ActionResult Create(product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.shop_id = new SelectList(db.shops, "id", "name", product.shop_id);
            return View(product);
        }


        //
        // GET: /Product/Edit/5

        [Authorize(Users = "administrator")]
        public ActionResult Edit(int id = 0)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.shop_id = new SelectList(db.shops, "id", "name", product.shop_id);
            return View(product);
        }


        //
        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "administrator")]
        public ActionResult Edit(product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.shop_id = new SelectList(db.shops, "id", "name", product.shop_id);
            return View(product);
        }


        //
        // GET: /Product/Delete/5
        public ActionResult Delete(int id = 0)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        //
        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}