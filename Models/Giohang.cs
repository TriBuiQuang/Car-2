using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Car.Models
{
    public class Giohang
    {
        dbCAR2DataContext data = new dbCAR2DataContext();
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnhBia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        public Giohang(int MaPT)
        {
            iMaSP = MaPT;
            SanPham sanpham = data.SanPhams.Single(n => n.MaSP == iMaSP);
            sTenSP = sanpham.TenSP;
            sAnhBia = sanpham.AnhBia;
            dDongia = double.Parse(sanpham.GiaBan.ToString());
            iSoluong = 1;
        }


    }
}