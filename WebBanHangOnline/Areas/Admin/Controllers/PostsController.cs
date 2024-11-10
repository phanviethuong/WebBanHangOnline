using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Posts
        public ActionResult Index()
        {
            var items = db.Posts.OrderByDescending(x => x.Id).ToList();
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 1;
                model.ModifiedrDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Posts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Posts.Find(id);
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts model)
        {
            if (ModelState.IsValid)
            {
                var existingNews = db.Posts.Find(model.Id); // Truy xuất thực thể gốc từ cơ sở dữ liệu
                if (existingNews != null)
                {
                    // Cập nhật các thuộc tính cần thay đổi
                    existingNews.Title = model.Title;
                    existingNews.Image = model.Image;
                    existingNews.Description = model.Description;
                    existingNews.Detail = model.Detail;
                    existingNews.IsActive = model.IsActive;
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
            var item = db.Posts.Find(id);
            if (item != null)
            {
                var DeleteItem = db.Posts.Attach(item);
                db.Posts.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsActive = item.IsActive });
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
                        var obj = db.Posts.Find(Convert.ToInt32(id));
                        if (obj != null)
                        {
                            db.Posts.Remove(obj);
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