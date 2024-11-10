using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ProductCategory> items = db.ProductCategorys.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedrDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.ProductCategorys.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.ProductCategorys.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                var existingNews = db.ProductCategorys.Find(model.Id); // Truy xuất thực thể gốc từ cơ sở dữ liệu
                if (existingNews != null)
                {
                    // Cập nhật các thuộc tính cần thay đổi
                    existingNews.Title = model.Title;
                    existingNews.Description = model.Description;
                    existingNews.Icon = model.Icon;
                    existingNews.SeoTitle = model.SeoTitle;
                    existingNews.SeoDescription = model.SeoDescription;
                    existingNews.SeoKeywords = model.SeoKeywords;
                    existingNews.ModifiedrDate = DateTime.Now;
                    existingNews.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);

                    db.Entry(existingNews).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductCategorys.Find(id);
            if (item != null)
            {
                var DeleteItem = db.ProductCategorys.Attach(item);
                db.ProductCategorys.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {

            if (!string.IsNullOrEmpty(ids))
            {
                var item = ids.Split(',');
                if (item != null && item.Any())
                {
                    foreach (var id in item)
                    {
                        var obj = db.ProductCategorys.Find(Convert.ToInt32(id));
                        if (obj != null)
                        {
                            db.ProductCategorys.Remove(obj);
                        }
                    }
                    db.SaveChanges();  // Thực hiện save sau khi đã xóa hết các bản ghi
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}