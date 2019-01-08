using Car.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Car.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        dbCAR2DataContext data = new dbCAR2DataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMaSP, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMaSP == iMaSP);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
                return RedirectToAction("PhuTung", "Car");
            }
            else
            {
                sanpham.iSoluong++;
                return RedirectToAction("PhuTung", "Car");
            }
        }
        private int TongSoLuong()
       {
                int iTongSoLuong = 0;
                List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang> ;
                if(lstGiohang!=null)
                {
                    iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
                }
            return iTongSoLuong;
       }
        private double TongTien()
        {
            double iTongtien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "Car");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if(sanpham!=null)
            {
                lstGiohang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("Giohang");
            }
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index3", "Car");
            }return RedirectToAction("Giohang");
        }
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if(sanpham!=null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }return RedirectToAction("Giohang");
        }
        public ActionResult XoaAllGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Car");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["KH"] == null || Session["KH"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Car");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["KH"];
            List<Giohang> gh = Laygiohang();
            CTDH ctdh = new CTDH();
            //dh.MaKH = kh.MaKH;
            dh.MaKH = kh.MaKH;
            dh.NgayDat = DateTime.Now;
            dh.TinhTrangGiaoHang = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                
                ctdh.MaDH = dh.MaDH;
                ctdh.MaSP = item.iMaSP;
                ctdh.SoLuong= item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CTDHs.InsertOnSubmit(ctdh);
            }
            

            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
    
}