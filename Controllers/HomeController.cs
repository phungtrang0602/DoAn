using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An.Models;
using PagedList;
using PagedList.Mvc;

namespace Do_An.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();
        public ActionResult Index(int? page)
        {
            //Tạo biến số sản phẩm trên trang
            int pageSize = 12;
            //Tạo biến số trang
            int pageNumber = (page ?? 1);
            return View(db.Saches.Where(n=>n.Moi==1).OrderBy(n=>n.GiaBan).ToPagedList(pageNumber,pageSize));
        }

        public ActionResult ThoiGian(int? page)
        {
            //Tạo biến số sản phẩm trên trang
            int pageSize = 12;
            //Tạo biến số trang
            int pageNumber = (page ?? 1);
            return View(db.Saches.Where(n => n.Moi == 1).OrderBy(n => n.NgayCapNhat).ToPagedList(pageNumber, pageSize));
        }
    }
}