using Do_An.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Do_An.Controllers
{
    public class ThongKeController : Controller
    {
       
        QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();

   
        public ActionResult Index()
        {
            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
               List<DonHang> donhang = db.DonHangs.Where(x => x.DaThanhToan == 1).ToList();

                double tongtien = 0;
                foreach(var item in donhang)
                {
                    double tien = 0;
                    List<ChiTietDonHang> chitiet = db.ChiTietDonHangs.Where(x => x.MaDonHang == item.MaDonHang).ToList();
                    foreach(var item2 in chitiet)
                    {
                        tien = tien + (double)(item2.SoLuong * Convert.ToDouble(item2.DonGia));
                    }

                    tongtien = tongtien + tien;
                }

                ViewBag.tien = tongtien;
                

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }



        public ActionResult Loc(string Batdau, string KetThuc)
        {
            DateTime batdau = Convert.ToDateTime(Batdau);
            DateTime ketthuc = Convert.ToDateTime(KetThuc);

            KhachHang kh = Session["TaiKhoan"] as KhachHang;
            if (kh != null && kh.quyen == 1)
            {
                List<DonHang> donhang = db.DonHangs.Where(x => x.DaThanhToan == 1 && x.NgayDat >= batdau && x.NgayDat <= ketthuc).ToList();

                double tongtien = 0;
                foreach (var item in donhang)
                {
                    double tien = 0;
                    List<ChiTietDonHang> chitiet = db.ChiTietDonHangs.Where(x => x.MaDonHang == item.MaDonHang).ToList();
                    foreach (var item2 in chitiet)
                    {
                        tien = tien + (double)(item2.SoLuong * Convert.ToDouble(item2.DonGia));
                    }

                    tongtien = tongtien + tien;
                }

                ViewBag.tien = tongtien;
                ViewBag.batdau = Batdau;
                ViewBag.ketthuc = KetThuc;


                return View(donhang);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}