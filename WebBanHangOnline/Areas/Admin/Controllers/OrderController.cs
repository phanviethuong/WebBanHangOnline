using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var item = db.Orders.OrderByDescending(x=>x.CreatedDate).ToList();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item.ToPagedList(pageIndex,pageSize));
        }

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x=>x.OrderId == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if(item != null)
            {
                db.Orders.Attach(item);
                item.TypePayment = trangthai;
                db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
    }
}