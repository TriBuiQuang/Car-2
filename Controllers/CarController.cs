using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car.Models;

using PagedList;
using PagedList.Mvc;

namespace Car.Controllers
{
    public class CarController : Controller
    {
        // GET: Car
       
        dbCAR2DataContext data = new dbCAR2DataContext();
        private List<NhaSanXuat> NSX(int count)
        {
            return data.NhaSanXuats.OrderByDescending(b => b.TenNSX).Take(count).ToList();
        }
        private List<Xe> Layxemoi(int count)
        {
            return data.Xes.OrderByDescending(a => a.NamSanXuat).Take(count).ToList();
        }
        private List<SanPham> Layspmoi(int count)
        {
            return data.SanPhams.OrderByDescending(b => b.TenSP).Where(b => b.Xoa == false).Take(count).ToList();
        }

        public ActionResult Index(int ? page,VIEW v)
        {
            if (Session["VisitorsCount"] != null)
            {
                
                DateTime today = DateTime.Now.Date;
                var v1 = data.VIEWs.Where(a => a.Ngay == today).FirstOrDefault();
                if (v1 == null)
                {
                    v.Ngay = today;
                    v.View1 += 1;
                    data.VIEWs.InsertOnSubmit(v);
                    data.SubmitChanges();
                }
                else
                {
                    VIEW sp = data.VIEWs.SingleOrDefault(n => n.Ngay == today);
                    sp.View1 += 1;
                    UpdateModel(sp);
                    data.SubmitChanges();
                }
            }
            
            var list = data.NhaSanXuats.OrderBy(n=>n.TenNSX).ToList();
            ViewBag.nhasan = list;
            int pageSize = 8;
            int pageNum = (page ?? 1);

            var xemoi = Layxemoi(10);
            return View(xemoi.ToPagedList(pageNum,pageSize));
        }
        
        public ActionResult PhuTung(int ? page, string sortOrder)
        {

            ViewBag.NameParm = String.IsNullOrEmpty(sortOrder) ? "NameParm_desc" : "";
            ViewBag.GiaParm = sortOrder == "GiaParm" ? "GiaParm_desc" : "GiaParm";
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var list = data.LoaiSPs.OrderBy(n => n.TenLoai).ToList();
            ViewBag.loaisp = list;
            var ptmoi = Layspmoi(10);
            var sanpham = ptmoi.ToPagedList(pageNum, pageSize);

            switch (sortOrder)
            {
                case "NameParm_desc":
                    sanpham = ptmoi.OrderByDescending(n => n.TenSP).ToPagedList(pageNum, pageSize);
                    break;
                case "GiaParm":
                    sanpham = ptmoi.OrderBy(n => n.GiaBan).ToPagedList(pageNum, pageSize);
                    break;
                case "GiaParm_desc":
                    sanpham = ptmoi.OrderByDescending(n => n.GiaBan).ToPagedList(pageNum, pageSize);
                    break;
                default:
                    sanpham = ptmoi.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize);
                    break;
            }

            return View(sanpham);
        }
        
        public ActionResult PhuTungDetails(int id)
        {
            var sp = from s in data.SanPhams
                     where s.MaSP == id
                     select s;
            var list = data.SanPhams.OrderBy(n => n.TenSP).Where(n=>n.MaSP != id ).ToList();
            ViewBag.splq = list;
            return View(sp.Single());
        }
        public ActionResult CarDetails(int id)
        {
            var xe = from x in data.Xes
                     where x.Maxe == id
                     select x;
            var list = data.Xes.OrderBy(n => n.Tenxe).Where(n=>n.Maxe != id).ToList();
            ViewBag.nhasx = list;
            return View(xe.Single());
        }
        public ActionResult Intro()
        {
            return View();
        }
        public ActionResult Search(string searchTerm)
        {
            var xes = from b in data.Xes select b;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                xes = data.Xes.Where(b => b.Tenxe.Contains(searchTerm));
            }
            ViewBag.SearchTerm = searchTerm;
            return View(xes);
        }
       
    }
}