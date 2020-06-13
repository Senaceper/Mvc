using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using denemee.Models;
using System.Web.Security;

namespace denemee.Controllers
{
    public class YoneticiController : Controller
    {
        private StajProjeEntities5 db = new StajProjeEntities5();


        
        
       public ActionResult YoneticiGiris()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult YoneticiGiris(Yonetici model)
       {
           var yonetici = db.Yonetici.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if(yonetici!=null)
            {
                Session["YoneticiName"] = yonetici.YoneticiID;
                Session["UserName"] = null;
                return RedirectToAction("Listele", "Urunler");
            }
            else
            {
                ViewBag.HATA = "Kullanici adi veya şifre yanlış";
                return View();
            }

       }

        public ActionResult YoneticiKayit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YoneticiKayit(Yonetici model)
        {
            db.Yonetici.Add(model);
            db.SaveChanges();
            if (Session["YoneticiName"] == null)
            {
                var yonetici = db.Yonetici.FirstOrDefault(x => x.UserName == model.UserName);
                Session["YoneticiName"] = yonetici.YoneticiID;
                Session["UserName"] = null;
            }

            return RedirectToAction("Listele", "Urunler");
        }
      

      


        // GET: Yonetici
        public ActionResult Index()
        {
            return View(db.Yonetici.ToList());
        }

        // GET: Yonetici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yonetici yonetici = db.Yonetici.Find(id);
            if (yonetici == null)
            {
                return HttpNotFound();
            }
            return View(yonetici);
        }

       

        // GET: Yonetici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yonetici yonetici = db.Yonetici.Find(id);
            if (yonetici == null)
            {
                return HttpNotFound();
            }
            return View(yonetici);
        }

        // POST: Yonetici/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YoneticiID,UserName,Mail,Password")] Yonetici yonetici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yonetici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yonetici);
        }

        // GET: Yonetici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yonetici yonetici = db.Yonetici.Find(id);
            if (yonetici == null)
            {
                return HttpNotFound();
            }
            return View(yonetici);
        }

        // POST: Yonetici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yonetici yonetici = db.Yonetici.Find(id);
            db.Yonetici.Remove(yonetici);
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
    }
}
