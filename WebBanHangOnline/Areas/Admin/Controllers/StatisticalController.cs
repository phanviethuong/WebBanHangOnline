using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Migrations;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OrderId
                        join p in db.Products on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            // Kiểm tra và lọc theo fromDate
            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    query = query.Where(x => x.CreatedDate >= startDate);
                }
                else
                {
                    return Json(new { Error = "Invalid fromDate format." }, JsonRequestBehavior.AllowGet);
                }
            }

            // Kiểm tra và lọc theo toDate
            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    query = query.Where(x => x.CreatedDate < endDate);
                }
                else
                {
                    return Json(new { Error = "Invalid toDate format." }, JsonRequestBehavior.AllowGet);
                }
            }

            var result = query
                .AsEnumerable() // Chuyển về local để tránh lỗi `TruncateTime` trong EF Core
                .GroupBy(x => x.CreatedDate.Date)  // Nhóm theo ngày
                .Select(x => new
                {
                    Date = x.Key,
                    TotalNhap = x.Sum(y => y.Quantity * y.OriginalPrice),
                    TotalBan = x.Sum(y => y.Quantity * y.Price),
                })
                .Select(x => new
                {
                    Date = x.Date,
                    DoanhThu = x.TotalBan,
                    LoiNhuan = x.TotalBan - x.TotalNhap
                });

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }


    }
}