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
    public class PhuKienController : Controller
    {
        dbQLPhukienDataContext data = new dbQLPhukienDataContext();
        private List<PHUKIEN> Layphukien(int count)
            {
            return data.PHUKIENs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();


            }

        // GET: PhuKien
        public ActionResult Index(int ? page)
        {
            // tao bien quy dinh so san pham tren moi trang
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var phukienmoi = Layphukien(5);
            return View(phukienmoi);
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhasanxuat()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult SPTheochude(int id)
        {
            var phukien = from s in data.PHUKIENs where s.MaCD == id select s;
            return View(phukien);
        }
        public ActionResult Detail(int id)
        {
            var linhkien = from s in data.PHUKIENs
                           where s.MaPK == id
                           select s;
            return View(linhkien.Single());
        }
    }
}