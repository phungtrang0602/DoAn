using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Text;
using Do_An.Models;

namespace Do_An.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();

        #region Giỏ hàng

        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì tiến hành khởi tạo list giỏ hàng(sessionGioHang)
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }


        public ActionResult ThemGioHang(int iMaSach,string strURL)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<GioHang> listGioHang = LayGioHang();
            //Kiểm tra sách nếu đã tồn tại hay không trong session chưa
            GioHang sp = listGioHang.Find(n=>n.iMaSach == iMaSach);
            if(sp == null)
            {
                sp = new GioHang(iMaSach);
                //Add sản phẩm mới thêm vào list
                listGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(strURL);
            }
        }
   
        public ActionResult CapNhapGioHang(int iMaSP,FormCollection f)
        {
            
            Sach sach = db.Saches.SingleOrDefault(n=>n.MaSach == iMaSP);
          
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<GioHang> listGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong  trong session Giỏ hàng
            GioHang sp = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSP);
            //Nếu mà tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

       
        public ActionResult XoaGioHang(int iMaSP)
        {
            //Kiểm tra ma sp
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSP);
            //Nếu get sai máp thì trả về trang 404
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSP);
            //Nếu mà tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                listGioHang.RemoveAll(n => n.iMaSach == iMaSP);
            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang", "GioHang"); // Chuyển hướng về trang Giỏ hàng
        }


        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> listGioHang = LayGioHang();
            return View(listGioHang);
        }
        
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang != null)
            {
                iTongSoLuong = listGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }


        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang != null)
            {
                dTongTien = listGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }

      
        public ActionResult GioHangPartial()
        {
            if(TongSoLuong() == 0)
            {
                return PartialView(); 
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

 
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> listGioHang = LayGioHang();
            return View(listGioHang);
        }
        #endregion

       
        #region Đặt hàng
   

        [HttpPost]
        public ActionResult DatHang()
        {
            //Kiểm tra đang đăng nhập
            if (Session["TaiKhoan"] == null|| Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            //Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }

            //Thêm đơn hàng vào giỏ hàng
            DonHang ddh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            db.DonHangs.Add(ddh);

            //Thêm chi tiết đơn đặt hàng
            foreach(var item in gh)
            {
                ChiTietDonHang CTDH = new ChiTietDonHang();
                CTDH.MaDonHang = ddh.MaDonHang;
                CTDH.MaSach = item.iMaSach;
                CTDH.SoLuong = item.iSoLuong;
                CTDH.DonGia = item.dDonGia.ToString();

                db.ChiTietDonHangs.Add(CTDH);
                db.SaveChanges();
                Sach sach1 = db.Saches.Find(item.iMaSach);
                sach1.SoLuongTon = sach1.SoLuongTon - CTDH.SoLuong;
                db.Saches.AddOrUpdate(sach1);
            }

            db.SaveChanges();

            //return RedirectToAction("Index","Home");

            return View();
        }

        #endregion

       
   

    }
}