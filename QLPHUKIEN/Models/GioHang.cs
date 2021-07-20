using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLPHUKIEN.Models
{
    public class GioHang
    {
        dbQLPhukienDataContext data = new dbQLPhukienDataContext();
        public int iMaPK { get; set; }
        public string sTenPK { get; set; }
        public string sAnhbia { get; set; }
        public double dGiaban { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dGiaban; }
        }
        public GioHang (int MaPK)
        {
            iMaPK = MaPK;
            PHUKIEN phukien = data.PHUKIENs.Single(n => n.MaPK == iMaPK);
            sTenPK = phukien.TenPK;
            sAnhbia = phukien.Anhbia;
            dGiaban = double.Parse(phukien.Giaban.ToString());
            iSoLuong = 1;

        }



    }
}