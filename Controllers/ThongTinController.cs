using Do_An.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Controllers
{
    public class ThongTinController : Controller
    {

        QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();

        // GET: ThongTin
        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }

        public ActionResult DonHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<DonHang> donhang = db.DonHangs.Where(x => x.MaKH == kh.MaKH).ToList();


            return View(donhang);
        }

        public ActionResult Delete(int id)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            KhachHang kh = (KhachHang)Session["TaiKhoan"];

            DonHang donhang = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);

            donhang.TinhTrangGiaoHang = -1;
            db.DonHangs.AddOrUpdate(donhang);

            
           
            List<ChiTietDonHang> listChitiet;

            do
            {
                ChiTietDonHang chitiet = db.ChiTietDonHangs.FirstOrDefault(x => x.MaDonHang == id);
                Sach sach = db.Saches.FirstOrDefault(x => x.MaSach == chitiet.MaSach);
                sach.SoLuongTon = sach.SoLuongTon + chitiet.SoLuong;
                db.Saches.AddOrUpdate(sach);
                db.SaveChanges();

            } while (db.ChiTietDonHangs.FirstOrDefault(x => x.MaDonHang == id) == null);

            db.SaveChanges();

            return RedirectToAction("DonHang");
        }
    }
}