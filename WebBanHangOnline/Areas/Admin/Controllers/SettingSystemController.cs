using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Setting()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            var checkTitle = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            if (checkTitle ==null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingTitle";
                set.SettingValue = req.SettingTitle;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkTitle.SettingValue = req.SettingTitle;
                db.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;
            }

            //Logo
            var checkLogo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingLogo";
                set.SettingValue = req.SettingLogo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
                db.Entry(checkLogo).State = System.Data.Entity.EntityState.Modified;
            }

            //Email
            var checkEmail = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingEmail"));
            if (checkEmail == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingEmail";
                set.SettingValue = req.SettingEmail;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkEmail.SettingValue = req.SettingEmail;
                db.Entry(checkEmail).State = System.Data.Entity.EntityState.Modified;
            }

            //Hotline
            var checkHotline = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingHotline"));
            if (checkHotline == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingHotline";
                set.SettingValue = req.SettingHotline;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkHotline.SettingValue = req.SettingHotline;
                db.Entry(checkHotline).State = System.Data.Entity.EntityState.Modified;
            }

            //TitleSeo
            var checkTitleSeo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitleSeo"));
            if (checkTitleSeo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = req.SettingTitleSeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkTitleSeo.SettingValue = req.SettingTitleSeo;
                db.Entry(checkTitleSeo).State = System.Data.Entity.EntityState.Modified;
            }

            //DesSeo
            var checkDesSeo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingDesSeo"));
            if (checkDesSeo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = req.SettingDesSeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkDesSeo.SettingValue = req.SettingDesSeo;
                db.Entry(checkDesSeo).State = System.Data.Entity.EntityState.Modified;
            }

            //KeySeo
            var checkKeySeo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingKeySeo"));
            if (checkKeySeo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = req.SettingKeySeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkKeySeo.SettingValue = req.SettingKeySeo;
                db.Entry(checkKeySeo).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return View("Partial_Setting");
        }
    }
}