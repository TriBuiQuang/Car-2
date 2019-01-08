using Car.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Car.Controllers
{
    public class VisiterController : Controller
    {
        // GET: Visiter
        dbCAR2DataContext db = new dbCAR2DataContext();
        private int TongView()
        {
            int iTongView = 0;
            iTongView = db.VIEWs.Sum(n=>n.View1);
            
            return iTongView;
        }
        
        public ActionResult ViewCount()
        {         
            ViewBag.TongView = TongView();
            return PartialView(db.VIEWs.SingleOrDefault(n => n.Ngay == DateTime.Now));
        }
        private int TongViewAdmin()
        {
            int iTongViewAd = 0;
            iTongViewAd = db.ADMINs.Sum(n=>n.MaAD);
            return iTongViewAd;
        }
        public ActionResult AdminCount()
        {
            ViewBag.Admin = db.ADMINs.Count();
       
            ViewBag.User = db.KhachHangs.Count(); 
            return PartialView();
        }
        public ActionResult MyChart()
        {
            ;
            decimal Jan = db.DonHangs.Count(n => n.NgayDat.Month == 1);
            decimal Feb = db.DonHangs.Count(n => n.NgayDat.Month == 2);
            decimal Mar = db.DonHangs.Count(n => n.NgayDat.Month == 3);
            decimal Apr = db.DonHangs.Count(n => n.NgayDat.Month == 4);
            decimal May = db.DonHangs.Count(n => n.NgayDat.Month == 5);
            decimal Jun = db.DonHangs.Count(n => n.NgayDat.Month == 6);
            decimal Jul = db.DonHangs.Count(n => n.NgayDat.Month == 7);
            decimal Aug = db.DonHangs.Count(n => n.NgayDat.Month == 8);
            decimal Sept = db.DonHangs.Count(n => n.NgayDat.Month == 9);
            decimal Oct = db.DonHangs.Count(n => n.NgayDat.Month == 10);
            decimal Nov = db.DonHangs.Count(n => n.NgayDat.Month == 11);
            decimal Dec = db.DonHangs.Count(n => n.NgayDat.Month == 12);
            new Chart(width: 750, height: 200)
                .AddSeries(
                    chartType: "column",
                    xValue : new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec", },
                    yValues : new[] { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sept, Oct, Nov, Dec }
                ).Write("png");

            return null;
        }
    }
}