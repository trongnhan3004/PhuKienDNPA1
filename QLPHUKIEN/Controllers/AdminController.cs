using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHUKIEN.Models;
using PagedList;
using PagedList.Mvc;
namespace QLPHUKIEN.Controllers
{
    public class AdminController : Controller
    {
        dbQLPhukienDataContext db = new dbQLPhukienDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin"); // chưa có thì nhảy vào loginz
            else
                // có r thì show trang chủ admin
                return View();
        }
        // tao view cho acction phu kien
        public ActionResult PhuKien(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;

            return View(db.PHUKIENs.ToList().OrderBy(n => n.MaPK).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["txtuser"];
            var matkhau = collection["txtpass"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẫu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẫu không đúng ";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemPhuKien()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                ViewBag.MaCD = new SelectList(db.CHUDEs.OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            return View();

        }
        [HttpPost]
        public ActionResult ThemPhuKien(PHUKIEN phukien)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                

                db.PHUKIENs.InsertOnSubmit(phukien);
                db.SubmitChanges();
                return RedirectToAction("PhuKien", "Admin");
            }
        }
        // sửa sản phẩm
        public ActionResult SuaPhuKien(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {

               // var phukien = from s in db.PHUKIENs where s.MaPK == id select s;
                PHUKIEN phukien = db.PHUKIENs.SingleOrDefault(n => n.MaPK == id);
                // lấy DL từ table chude để đổ vào dropdownlist kèm theo chọn MaCD tương ứng
                ViewBag.MaCD = new SelectList(db.CHUDEs.OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
                return View(phukien);
            }
        }
        [HttpPost, ActionName("SuaPhuKien")]
        public ActionResult XacNhanSua(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                
                PHUKIEN phukien = db.PHUKIENs.SingleOrDefault(n => n.MaPK == id);
                UpdateModel(phukien);
                db.SubmitChanges();
                return RedirectToAction("PhuKien", "Admin");
            }
        }
    }
}