using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(int? id)
        {
            var items = db.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategorys.Find(id);
            if(cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            return View(items);
        }

        public ActionResult Detail(int id)
        {
            var items = db.Products.Find(id);
            if(items != null)
            {
                db.Products.Attach(items);
                items.ViewCount = items.ViewCount + 1;
                db.Entry(items).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var item = db.Products.Where(x=>x.IsHome).Take(12).ToList(); 
            return PartialView(item);
        }

        public ActionResult Partial_ProductSale()
        {
            var items = db.Products.Where(x => x.IsSale).Take(12).ToList();
            return PartialView(items);
        }
    }
}