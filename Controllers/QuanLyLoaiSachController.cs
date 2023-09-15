using Do_An.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Do_An.Controllers
{
    public class QuanLyLoaiSachController : Controller
    {
        QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();
        public ActionResult Index()
        {
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
                return View(db.ChuDes.OrderBy(x => x.MaChuDe).ToList());
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
           
        }

      
        public ActionResult Details(int? page,int maChuDe)
        {   
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(db.Saches.Where(x =>x.MaChuDe == maChuDe).OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Create()
        {
            
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(ChuDe chude)
        {
            if (ModelState.IsValid)
            {
                if (chude.TenChuDe == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    db.ChuDes.Add(chude);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");

        }

    
        public ActionResult Edit(int id)
        {
            // Lấy từ cơ sở dữ liệu dựa trên id và truyền vào view
            ChuDe chude = db.ChuDes.Find(id);
            if (chude == null)
            {
                return HttpNotFound();
            }

            return View(chude);
        }

        [HttpPost]
        public ActionResult Edit(ChuDe chude)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chude).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chude);
        }

        
        public ActionResult Delete(int id)
        {
            ChuDe chude = db.ChuDes.Find(id);
            if (chude == null)
            {
                return HttpNotFound();
            }
            else
            {
                while (db.Saches.Any(x => x.MaChuDe == id))
                {
                    Sach sach = db.Saches.FirstOrDefault(x => x.MaChuDe == id);
                    sach.MaChuDe = 24;
                    db.Saches.AddOrUpdate(sach);
                    db.SaveChanges();
                }
                db.ChuDes.Remove(chude);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

       
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
