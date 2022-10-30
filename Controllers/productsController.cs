using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Homework_Assignment_6.Models;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace Homework_Assignment_6.Controllers
{
    public class productsController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: products
        public ActionResult Index()
        {
          
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return View(products.ToList());
        }
        public ActionResult Store(product product)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
               db.products.Add(product);
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }
        
        public JsonResult GetProducts()
        {
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            products.ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getProduct(int Id)
        {
           
            product product = db.products.Find(Id);
            
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
   
            return Json(product,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateProduct(product products)
        {
            using (BikeStoresEntities entities = new BikeStoresEntities())
            {
                product updatedProduct = (from c in entities.products
                                            where c.product_id == products.product_id
                                            select c).FirstOrDefault();
                updatedProduct.product_name = products.product_name;
                updatedProduct.list_price = products.list_price;
                updatedProduct.brand_id = products.brand_id;
                updatedProduct.category_id = products.category_id;
                updatedProduct.product_name = products.product_name;
                updatedProduct.model_year = products.model_year;
                entities.SaveChanges();
            }

            return RedirectToAction("Index", "Products"); ;
        }
        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
      
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Chart()
        {
         
            var products = db.products.Where(a => a.category_id == 6).ToList();
            int products2016 = 0;
            int products2017 = 0;
            int products2018 = 0;
            int products2019 = 0;
            if (products.Count > 0)
            {
 
           
                foreach (var product in products)
                {
                    if (product.model_year == 2016)
                    {
                        products2016++;
                    }
                    if (product.model_year == 2017)
                    {
                        products2017++;
                    }
                    if (product.model_year == 2018)
                    {
                        products2018++;
                    }
                    if (product.model_year == 2019)
                    {
                        products2019++;
                    }
                }
              
                string Datachart = "["+products2016+ ","+products2017+ ","+products2018+ ","+products2019+"]";
                ViewData["Chartdata"] = Datachart;
                ViewData["Chartlabels"] ="[2016,2017,2018,2019]";
            }
          return View();
        }
       
       
    }
}
