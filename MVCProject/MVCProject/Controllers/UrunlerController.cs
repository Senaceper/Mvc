using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using denemee.Models;
using System.IO;
using System.Data.SqlClient;

namespace denemee.Controllers
{
    public class UrunlerController : Controller
    {
        private StajProjeEntities5 db = new StajProjeEntities5();
        
        // GET: Urunler
        public ActionResult Index()
        {
           
            return View(db.Urunler.ToList());
        }
        public ActionResult IDListele(int id)
        {
            
               
                var model = db.Urunler.Where(x => x.UrunKategoriID == id).ToList();
            
                return View(model);
            
                
           
            
            
        }


        public ActionResult SepetEkle(int id)
        {
            if (Session["UserName"] != null)
            {
                Sepet sepet = new Sepet();
                var model = db.Urunler.Find(id);
                sepet.UrunID = model.UrunID;
                sepet.KullaniciID = Convert.ToInt32(Session["UserName"].ToString());
                sepet.Fiyat = model.UrunFiyat;
                sepet.Durum = "Beklemede";
                sepet.Adet = 1;
                db.Sepet.Add(sepet);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UyeGiris", "Home");
            }
        }

        public ActionResult Sepetim()
        {
            if (Session["UserName"] != null)
            {
                ViewModel vm = new ViewModel();
                vm.SepetModel = db.Sepet.ToList();
                vm.UrunModel = db.Urunler.ToList();
                return View(vm);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
            

        }

        public ActionResult SepetiOnayla()
        {
            int uyeID = Convert.ToInt32(Session["UserName"].ToString());
            var model = db.Sepet.Where(x=> x.KullaniciID==uyeID && x.Durum=="Beklemede").ToList();
            foreach(var item in model)
            {
                    item.Durum = "Onaylandı";
            }
            db.SaveChanges();

            return RedirectToAction("Sepetim","Urunler");
        }

        public ActionResult Detay(int id)
        {
            var model = db.Urunler.Find(id);
            return View(model);

        }

        public ActionResult Listele()
        {
            
            return View(db.Urunler.ToList());
        }

        public ActionResult Sil(int id)
        { 
            db.Urunler.Remove(db.Urunler.Find(id));
            db.SaveChanges();
            return RedirectToAction("Listele");
        
        }


        public ActionResult SepettenCikar(int id)
        {
            db.Sepet.Remove(db.Sepet.Find(id));
            db.SaveChanges();
            return RedirectToAction("Sepetim");
        }
        public ActionResult Guncelle(int id)
        {
            return View("Guncelle", db.Urunler.Find(id));
        }
        
        [HttpPost]
        public ActionResult Guncelle(Urunler urun, System.Web.HttpPostedFileBase urunresim)
        {
            string dosyaYolu = "";
            var mevcut = db.Urunler.Find(urun.UrunID);
            mevcut.UrunName = urun.UrunName;
            mevcut.UrunFiyat = Convert.ToInt32(urun.UrunFiyat);
            mevcut.UrunAciklama = urun.UrunAciklama;
            mevcut.UrunKategoriID = Convert.ToInt32(urun.UrunKategoriID.ToString());
            mevcut.UrunYoneticiID = Convert.ToInt32(Session["YoneticiName"].ToString());

            if(urunresim==null)
            {
                mevcut.UrunResmi = mevcut.UrunResmi;
            }
            else
            {
                Guid benzersiz = Guid.NewGuid(); ;
                dosyaYolu = benzersiz.ToString();
                dosyaYolu += Path.GetFileName(urunresim.FileName);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/resimler"), dosyaYolu);
                urunresim.SaveAs(yuklemeYeri);
                urun.UrunResmi = dosyaYolu;
                mevcut.UrunResmi = urun.UrunResmi;
            }
            db.SaveChanges();
            return RedirectToAction("Listele") ;
        }


        public ActionResult Add()
        {
             
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form,System.Web.HttpPostedFileBase urunresim)
        {
           
            string dosyaYolu="";
            Urunler model = new Urunler();
            model.UrunName = form["UrunName"];
            model.UrunFiyat = Convert.ToInt32(form["UrunFiyat"]);
            //  model.UrunResmi = form["urunresim"];
            model.UrunAciklama = form["UrunAciklama"];
    
            if(Session["YoneticiName"]==null)
            {
                model.UrunYoneticiID = -1;
            }
            else
            {
                model.UrunYoneticiID = Convert.ToInt32(Session["YoneticiName"].ToString());
            }
           
         
            model.UrunKategoriID = Convert.ToInt32(form["UrunKategoriID"]);

            if (urunresim != null)
            {
                Guid benzersiz = Guid.NewGuid(); ;
                dosyaYolu = benzersiz.ToString();
                dosyaYolu += Path.GetFileName(urunresim.FileName);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/resimler"), dosyaYolu);
                urunresim.SaveAs(yuklemeYeri);
                model.UrunResmi = dosyaYolu;
            }
            else
            {
                model.UrunResmi= "ResimYok.png";
            }



            db.Urunler.Add(model);
            db.SaveChanges();
          
            return RedirectToAction("Listele");
        }


        // GET: Urunler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // GET: Urunler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Urunler/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UrunID,UrunName,UrunFiyat,UrunResmi,UrunAciklama,UrunYoneticiID,UrunKategoriID")] Urunler urunler,System.Web.HttpPostedFileBase yuklenecekDosya)
        {
            if (ModelState.IsValid)
            {
                db.Urunler.Add(urunler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(urunler);
        }

        // GET: Urunler/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // POST: Urunler/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UrunID,UrunName,UrunFiyat,UrunResmi,UrunAciklama,UrunYoneticiID,UrunKategoriID")] Urunler urunler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(urunler);
        }

        // GET: Urunler/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // POST: Urunler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urunler urunler = db.Urunler.Find(id);
            db.Urunler.Remove(urunler);
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
