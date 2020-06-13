


using denemee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace denemee.Controllers
{
    public class HomeController : Controller
    {
        private StajProjeEntities5 db = new StajProjeEntities5();
        public ActionResult Index()
        {
          //  List<Urunler> urunler = db.Urunler.ToList();
            List<Urunler> model = db.Urunler.ToList();

            return View(model);
        }

        public ActionResult UyeGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeGiris(Kullanici model)
        {
            var kullanici = db.Kullanici.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (kullanici != null)
            {
                Session["YoneticiName"] = null;
                Session["UserName"] = kullanici.KullaniciID;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.HATA = "Kullanici adi veya şifre yanlış";
                return View();
            }
        }

        public ActionResult UyeKayit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeKayit(Kullanici model)
        {
            db.Kullanici.Add(model);
            db.SaveChanges();
            if (Session["UserName"] == null)
            {
                var kullanici = db.Kullanici.FirstOrDefault(x => x.UserName == model.UserName);
                Session["UserName"] = kullanici.KullaniciID;
                Session["YoneticiName"] = null;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cikis()
        {
            Session["UserName"] = null;
            Session["YoneticiName"] = null;
            RedirectToAction("Index", "Home");
            return View();
        }
    }
}