using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shops.Controllers
{
    [Authorize(Users = "administrator")]
    public class ShopController : Controller
    {
        private ShopsEntities db = new ShopsEntities();

        //
        // GET: /Shop/
        public ActionResult Index()
        {
            return View(db.shops.ToList());
        }

        //
        // GET: /Shop/Details/5
        public ActionResult Details(int id = 0)
        {
            shop shop = db.shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // GET: /Shop/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Shop/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "administrator")]
        public ActionResult Create(shop shop)
        {
            if (ModelState.IsValid)
            {
                db.shops.Add(shop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shop);
        }

        //
        // GET: /Shop/Edit/5

        public ActionResult Edit(int id = 0)
        {
            shop shop = db.shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // POST: /Shop/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(shop shop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shop);
        }

        //
        // GET: /Shop/Delete/5

        public ActionResult Delete(int id = 0)
        {
            shop shop = db.shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // POST: /Shop/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            shop shop = db.shops.Find(id);
            db.shops.Remove(shop);
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