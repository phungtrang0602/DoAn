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


                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }



            return View();
        }
    }
}