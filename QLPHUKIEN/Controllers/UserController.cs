using QLPHUKIEN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using System.Configuration;
using NPOI.SS.Formula.Functions;

namespace QLPHUKIEN.Controllers
{
    public class UserController : Controller
    {
        dbQLPhukienDataContext db = new dbQLPhukienDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ Tên Khách hàng không được để trống ";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập điện thoại";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);

                // vai dog nay sao lai la khach hang co s?
                db.KHACHHANGs.InsertOnSubmit(kh);
                //db.KHACHHANG.InsertOnSoubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap", "User");
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "PhuKien");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng ";
            }
            return View();
        }

    //    private Uri RedirectUri
    //    {
    //        get
    //        {
    //            var uriBuilder = new UriBuilder(Request.Url);
    //            uriBuilder.Query = null;
    //            uriBuilder.Fragment = null;
    //            uriBuilder.Path = Url.Action("FacebookCallback");
    //            return uriBuilder.Uri;
    //        }
    //    }
    //    public ActionResult LoginFaceBook()
    //    {
    //        var fb = new FacebookClient();
    //        var loginUrl = fb.GetLoginUrl(new
    //        {
    //            client_id = ConfigurationManager.AppSettings["fbAppId"],
    //            client_secret = ConfigurationManager.AppSettings["fbAppSecret"],
    //            redirect_uri = RedirectUri.AbsoluteUri,
    //            response_type = "code",
    //            scope = "email",
    //        });
    //        return Redirect(loginUrl.AbsoluteUri);
    //    }

    //    public ActionResult FacebookCallback(string code)

    //    {

    //        var fb = new FacebookClient();
    //        dynamic result = fb.Post("oauth/access_token", new
    //        {
    //            client_id = ConfigurationManager.AppSettings["fbAppId"],
    //            client_secret = ConfigurationManager.AppSettings["fbAppSecret"],
    //            redirect_uri = RedirectUri.AbsoluteUri,
    //            code = code
    //        });
    //        var accessToken = result.access_token;
    //        fb.AccessToken = accessToken();
    //        if (!string.IsNullOrEmpty(accessToken))
    //        {
    //            dynamic kh = fb.Get("me?fields=name,ngaysinh,dienthoai,id,email");
    //            string email = kh.email;
    //            string username = kh.email;
    //            string name = kh.name;
    //            string dienthoai = kh.dienthoai;
    //            string ngaysinh = kh.ngaysinh;

    //            var user = new KHACHHANG();
    //            user.Email = email;
    //            user.Taikhoan = email;
    //            user.DienthoaiKH = dienthoai;
    //            user.HoTen = name;
    //        }
    //        else
    //        {

    //        }
    //        return RedirectToAction("Index", "Home");
    //    }

    }

   
}


