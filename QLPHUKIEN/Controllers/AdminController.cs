using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHUKIEN.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
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

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin"); // chưa có thì nhảy vào loginz
            else
                // có r thì show trang chủ admin

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
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
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
        //[HttpPost]
        //public ActionResult ThemPhuKien(PHUKIEN phukien)
        //{
        //    if (Session["Taikhoanadmin"] == null)
        //        return RedirectToAction("Login", "Admin");
        //    else
        //    {


        //        db.PHUKIENs.InsertOnSubmit(phukien);
        //        db.SubmitChanges();
        //        return RedirectToAction("PhuKien", "Admin");
        //    }
        //}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemPhuKien(PHUKIEN phukien, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //kiem tra đường dẫn file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //thêm vào csdl
                else
                {
                    if (ModelState.IsValid)
                    {
                        // lưu tên file, lưu ý bổ sung thư viện System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        // lưu đường dẫn ccủa file
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        // kiểm tra hình ảnh tồn tại chưaaaaaaa?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            // lưu hình ảnh vào đường dẫn
                            fileUpload.SaveAs(path);
                        }
                        phukien.Anhbia = fileName;
                        // lưu vào csdl
                        db.PHUKIENs.InsertOnSubmit(phukien);
                        db.SubmitChanges();
                    }
                    return RedirectToAction("PhuKien", "Admin");
                }


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
        [ValidateInput(false)]
        public ActionResult XacNhanSua(PHUKIEN phukien, HttpPostedFileBase fileUpload)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                PHUKIEN phukienUpdate = db.PHUKIENs.SingleOrDefault(n => n.MaPK == phukien.MaPK);

                //kiem tra đường dẫn file
                if (fileUpload != null)
                {
                    // lưu tên file, lưu ý bổ sung thư viện System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // lưu đường dẫn ccủa file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    // kiểm tra hình ảnh tồn tại chưaaaaaaa?
                    if (!System.IO.File.Exists(path))
                    {
                        // lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }

                    phukienUpdate.Anhbia = fileName;
                }

                UpdateModel(phukienUpdate);
                db.SubmitChanges();
                return RedirectToAction("PhuKien", "Admin");
            }
        }
        // chi tiết sản phẩm
        public ActionResult ChiTietPK(int id)
        {
            //lấy ra đối tượng phụ kiện theo mã
            PHUKIEN phukien = db.PHUKIENs.SingleOrDefault(n => n.MaPK == id);
            ViewBag.MaPK = phukien.MaPK; ;
            if (phukien == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phukien);
        }
        
        // xoa san pham
        [HttpGet]
        public ActionResult XoaPhuKien(int id)
        {
            PHUKIEN phukiendelete = db.PHUKIENs.SingleOrDefault(n => n.MaPK == id);
            ViewBag.MaPK = phukiendelete.MaPK;
            if(phukiendelete == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phukiendelete);
        }

        [HttpPost, ActionName("XoaPhuKien")]
        public ActionResult XacNhanXoa(int id)
        {
            // lấyy ra đối tượng cần xoá theo mã
            PHUKIEN phukiendelete = db.PHUKIENs.SingleOrDefault(n => n.MaPK == id);
            ViewBag.MaPK = phukiendelete.MaPK;
            if (phukiendelete == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.PHUKIENs.DeleteOnSubmit(phukiendelete);
            db.SubmitChanges();
            return RedirectToAction("PhuKien");
        }
            
            




    }
}