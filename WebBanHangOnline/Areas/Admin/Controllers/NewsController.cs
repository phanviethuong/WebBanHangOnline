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
    [Authorize(Roles = "Admin,Employee")]
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index(string SearchText, string SearchCriteria, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = db.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(SearchText) && !string.IsNullOrEmpty(SearchCriteria))
            {
                switch (SearchCriteria)
                {
                    case "Alias":
                        items = items.Where(x => x.Alias.Contains(SearchText));
                        break;
                    case "Title":
                        items = items.Where(x => x.Title.Contains(SearchText));
                        break;
                    case "Detail":
                        items = items.Where(x => x.Detail.Contains(SearchText));
                        break;
                    default:
                        break;
                }
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.SearchText = SearchText;
            ViewBag.SearchCriteria = SearchCriteria;

            return View(items);
        }


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 1;
                model.ModifiedrDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.News.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.News.Find(id);
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                var existingNews = db.News.Find(model.Id); // Truy xuất thực thể gốc từ cơ sở dữ liệu
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
            var item = db.News.Find(id);
            if (item != null)
            {
                var DeleteItem = db.News.Attach(item);
                db.News.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsActive = item.IsActive});
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
           
            if (!string.IsNullOrEmpty(ids))
            {
               var item = ids.Split(',');
                if(item != null && item.Any())
                {
                    foreach (var id in item)
                    {
                        var obj = db.News.Find(Convert.ToInt32(id));
                        if (obj != null)
                        {
                            db.News.Remove(obj);
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