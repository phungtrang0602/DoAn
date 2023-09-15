using Do_An.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Do_An.Controllers
{
    public class QuanLyDonHangController : Controller
    {
  
        QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();
        
        public ActionResult Index(int? page)
        {
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
                // Tạo biến số don hang trên trang
                int pageSize = 100;
                // Tạo biến số trang
                int pageNumber = (page ?? 1);
                return View(db.DonHangs.OrderBy(n => n.NgayDat).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Details(int maDonHang = 0)
        {
            List<ChiTietDonHang> chitiet = db.ChiTietDonHangs.Where(x => x.MaDonHang == maDonHang).ToList();   
            return View(chitiet);
        }
        public ActionResult Edit(int id)
        {
            DonHang donhang = db.DonHangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }
        [HttpPost]
        public ActionResult Edit(DonHang donhang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donhang);
        }
        public ActionResult Delete(int id)
        {
            DonHang donhang = db.DonHangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            else
            {   
                while (db.ChiTietDonHangs.Any(x => x.MaDonHang == id))
                {
                    ChiTietDonHang chitiet = db.ChiTietDonHangs.FirstOrDefault(x => x.MaDonHang == id);
                    db.ChiTietDonHangs.Remove(chitiet);
                    db.SaveChanges();
                }
                
                db.DonHangs.Remove(donhang);
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

        public ActionResult ChuaXuLy(int? page)
        {
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
                // Tạo biến số don hang trên trang
                int pageSize = 50;
                // Tạo biến số trang
                int pageNumber = (page ?? 1);

                return View(db.DonHangs.OrderBy(n => n.MaKH).Where(x =>x.TinhTrangGiaoHang == null || x.TinhTrangGiaoHang == 0 ).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DaXuLy(int? page)
        {
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
                // Tạo biến số don hang trên trang
                int pageSize = 50;
                // Tạo biến số trang
                int pageNumber = (page ?? 1);

                return View(db.DonHangs.OrderBy(n => n.MaKH).Where(x => x.TinhTrangGiaoHang == 1 || x.DaThanhToan == 1 || x.TinhTrangGiaoHang == -1 ).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult VanChuyen(int maDonHang = 0)
        {
            DonHang donHang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);
            KhachHang khach = db.KhachHangs.FirstOrDefault(x => x.MaKH == donHang.MaKH);

            return View(khach);
        }

        public ActionResult sachchitiethoadon(int maSach = 0)
        {

            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
                return View(db.Saches.Where(n => n.MaSach == maSach).OrderBy(n => n.MaSach).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public PartialViewResult XuLyPartial(int maDonHang)
        {
            DonHang donhang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);
          
            return PartialView(donhang);
        }

        public ActionResult GiaoHang(int maDonHang)
        {
            DonHang donhang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);

            donhang.TinhTrangGiaoHang = 0;
            db.DonHangs.AddOrUpdate(donhang);
            db.SaveChanges();

            return RedirectToAction("ChuaXuLy");
        }

        public ActionResult HuyDon(int maDonHang)
        {
            DonHang donhang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);

            donhang.TinhTrangGiaoHang = -1;
            db.DonHangs.AddOrUpdate(donhang);
            List<ChiTietDonHang> chitiet = db.ChiTietDonHangs.Where(x => x.MaDonHang == maDonHang).ToList();
            foreach(var item in chitiet)
            {
                Sach sach = db.Saches.FirstOrDefault(x => x.MaSach == item.MaSach);
                sach.SoLuongTon = sach.SoLuongTon + item.SoLuong;
                db.Saches.AddOrUpdate(sach);
                db.SaveChanges();
            }

            db.SaveChanges();

            return RedirectToAction("ChuaXuLy");
        }

        public ActionResult ThanhCong(int maDonHang)
        {
            DonHang donhang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);

            donhang.TinhTrangGiaoHang = 1;
            donhang.DaThanhToan = 1;
            donhang.NgayGiao = DateTime.Now;
            db.DonHangs.AddOrUpdate(donhang);
            db.SaveChanges();

            return RedirectToAction("ChuaXuLy");
        }

        public ActionResult DoHangDemo(int maDonHang)
        {
            ViewBag.MaDonHang = maDonHang;

            DonHang donhang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);
            List<ChiTietDonHang> chitiet = db.ChiTietDonHangs.Where(x => x.MaDonHang == maDonHang).ToList();
            List<GioHang> listGioHang = new List<GioHang>();
            foreach(var item in chitiet)
            {
                GioHang gioHang = new GioHang(item.MaSach);
                gioHang.iSoLuong = (int)item.SoLuong;
                listGioHang.Add(gioHang);
            }
            return View(listGioHang); 
            
        }
    }
}
