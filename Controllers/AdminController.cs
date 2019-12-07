using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car.Models;
using PagedList;
using System.IO;

using PagedList.Mvc;

namespace Car.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbCAR2DataContext db = new dbCAR2DataContext();
        public ActionResult Index()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login","Nguoidung");
        }
        private List<Xe> Layxemoi(int count)
        {
            return db.Xes.OrderByDescending(a => a.NamSanXuat).Take(count).ToList();
        }
        private List<SanPham> Layspmoi(int count)
        {
            return db.SanPhams.OrderByDescending(b => b.GiaBan).Take(count).ToList();
        }

        


        public ActionResult Sanpham(int ?page, String search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.SanPhams.Where(n=>n.TenSP.StartsWith(search)||search==null).ToList().OrderBy(n => n.MaSP).Where(n => n.Xoa == false).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login","Nguoidung");
        }

        [HttpGet]
        public ActionResult Themmoisp()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
                //ViewBag.MaSP = new SelectList(db.SanPhams.ToList().OrderBy(n => n.TenSP), "MaSP", "TenSP");
                return View();
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        public ActionResult ThemmoiSp(SanPham sanpham, HttpPostedFileBase fileUpload)
        {
            
            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if(fileUpload==null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hành ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sanpham.Xoa = false;
                    sanpham.AnhBia = fileName;
                    db.SanPhams.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("SanPham");
        }

        //SuaSP
        [HttpGet]
        public ActionResult Suasp(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);

                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
                return View(sp);
            }
            return RedirectToAction("Login","Nguoidung");
        }
        [HttpPost, ActionName("Suasp")]
        [ValidateInput(false)]
        public ActionResult xacnhansuasp(int id)
        {

            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            sp.Xoa = false;
            UpdateModel(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }

        [HttpPost]
        public ActionResult Delete1(int[] dsxoa)
        {
            foreach(int Id in dsxoa)
            {
                SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == Id);
                sanpham.Xoa = true;
                UpdateModel(sanpham);
            }
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        [HttpPost]
        public ActionResult Delete2(int[] dsxoa)
        {
            foreach (int Id in dsxoa)
            {
                Xe xe = db.Xes.SingleOrDefault(n => n.Maxe == Id);
                xe.Xoa = true;
                UpdateModel(xe);
            }
            db.SubmitChanges();
            return RedirectToAction("Xe");
        }

        // Xoa sp
        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);

                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
                return View(sp);
            }
            return RedirectToAction("Login","Nguoidung");
        }

        [HttpPost, ActionName("Xoasp")]
        [ValidateInput(false)]
        public ActionResult xacnhanxoasp(int id)
        {

            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            sp.Xoa = true;
            UpdateModel(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        ////////////////////////////////////XE
        [HttpGet]
        public ActionResult Themmoixe()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
                //ViewBag.Maxe = new SelectList(db.Xes.ToList().OrderBy(n => n.Tenxe), "Maxe", "Tenxe");
                return View();
            }
            return RedirectToAction("Login","Nguoidung");
        }
        [HttpPost]
        public ActionResult Themmoixe(Xe xe, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    xe.Xoa = false;
                    xe.Anh = fileName;
                    db.Xes.InsertOnSubmit(xe);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Xe","Admin");
        }

        [HttpGet]
        public ActionResult Suaxe(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                Xe x = db.Xes.SingleOrDefault(n => n.Maxe == id);

                if (x == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
                return View(x);
            }
            return RedirectToAction("Login","Nguoidung");
        }
        [HttpPost, ActionName("Suaxe")]
        [ValidateInput(false)]
        public ActionResult xacnhansuaxe(int id)
        {
            
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            Xe x = db.Xes.SingleOrDefault(n => n.Maxe == id);
           
            UpdateModel(x);
            db.SubmitChanges();
            return RedirectToAction("Xe");
        }

        // Xoa sp
        [HttpGet]
        public ActionResult Xoaxe(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                Xe x = db.Xes.SingleOrDefault(n => n.Maxe == id);
                x.Xoa = false;
                if (x == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
                return View(x);
            }
            return RedirectToAction("Login","Nguoidung");
        }

        [HttpPost, ActionName("Xoaxe")]
        [ValidateInput(false)]
        public ActionResult xacnhanxoaxe(int id)
        {
            
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            Xe x = db.Xes.SingleOrDefault(n => n.Maxe == id);
            x.Xoa = true;
            UpdateModel(x);
            db.SubmitChanges();
            return RedirectToAction("Xe");
        }


        public ActionResult Xe(int ?page)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                return View(db.Xes.ToList().OrderBy(n => n.Maxe).Where(n => n.Xoa == false).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }

        public ActionResult DonHang(int ?page, String search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 3;

                return View(db.DonHangs.Where(n=>n.KhachHang.HoTen.StartsWith(search) || search == null ).ToList().OrderBy(n => n.MaDH).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        public ActionResult ChitietDH(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                CTDH ctdh = db.CTDHs.SingleOrDefault(n => n.MaDH == id);
                ViewBag.MaDH = ctdh.MaDH;
                ViewBag.TinhTrangGiaoHang = ctdh.TinhTrangGiaoHang;
                var tkh= db.DonHangs.Where(a => a.MaDH == ctdh.MaDH).FirstOrDefault();

                ViewBag.TenKH = db.KhachHangs.Where(a => a.MaKH == tkh.MaKH).FirstOrDefault();
                ViewBag.Sp = db.SanPhams.Where(a => a.MaSP == ctdh.MaSP).ToList();

                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ctdh);
            }
            return RedirectToAction("Login","Nguoidung");
        }


        
        public ActionResult DSAdmin(int ?page ,string search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                return View(db.ADMINs.Where(n => n.HoTen.StartsWith(search) || search == null && n.Role!=0 || search == null).ToList().OrderBy(n => n.MaAD).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login","Nguoidung");
        }
        [HttpGet]
        public ActionResult Themadmin()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themadmin(ADMIN a)
        {
            if (ModelState.IsValid)
            {
                db.ADMINs.InsertOnSubmit(a);
                db.SubmitChanges();
            }
            return RedirectToAction("DSAdmin");
        }
        [HttpGet]
        public ActionResult Suaadmin(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                ADMIN kh = db.ADMINs.SingleOrDefault(n => n.MaAD == id);
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaadmin(FormCollection f)
        {
            ADMIN l = db.ADMINs.First(d => d.MaAD == int.Parse(f["MaAD"]));
            if (ModelState.IsValid)
            {
                l.Taikhoan = f["Taikhoan"];
                l.Pass = f["Pass"];
                l.HoTen = f["HoTen"];
                l.Email = f["Email"];
                l.DienThoai = f["DienThoai"];
                l.Role = int.Parse(f["Role"]);

                UpdateModel(l);
                db.SubmitChanges();
            }
            return RedirectToAction("DSAdmin");
        }
        [HttpGet]
        public ActionResult Xoaadmin(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                ADMIN th = db.ADMINs.SingleOrDefault(n => n.MaAD == id);

                if (th == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(th);
            }
            return RedirectToAction("Login", "Nguoidung");
        }

        [HttpPost, ActionName("Xoaadmin")]

        public ActionResult XacnhanXoaadmin(int id)
        {
            ADMIN l = db.ADMINs.SingleOrDefault(n => n.MaAD == id);
            if (l == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ADMINs.DeleteOnSubmit(l);
            db.SubmitChanges();
            return RedirectToAction("DSAdmin");
        }
        public ActionResult DangXuat()
        {
            Session["TaikhoanAdmin"] = null;
            return RedirectToAction("Login", "Nguoidung");
        }

        //loai sp
        public ActionResult QLLoai(int? page, string search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.LoaiSPs.Where(n => n.TenLoai.StartsWith(search) || search == null).ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpGet]
        public ActionResult Themloai()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themloai(LoaiSP l)
        { 
            if (ModelState.IsValid)
            {
                db.LoaiSPs.InsertOnSubmit(l);
                db.SubmitChanges();
            }
            return RedirectToAction("QLLoai");           
        }
        [HttpGet]
        public ActionResult Sualoai(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                LoaiSP kh = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == id);
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sualoai(FormCollection f)
        {
            LoaiSP l = db.LoaiSPs.First(d => d.MaLoai == int.Parse(f["IdBrand"]));
            if (ModelState.IsValid)
            {
                l.TenLoai = f["TenLoai"];
                UpdateModel(l);
                db.SubmitChanges();
            }
            return RedirectToAction("QLLoai");
        }
        [HttpGet]
        public ActionResult Xoaloai(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                LoaiSP th = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == id);

                if (th == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(th);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
 
        [HttpPost, ActionName("Xoaloai")]

        public ActionResult XacnhanXoaloai(int id)
        {
            LoaiSP l = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == id);
            if (l == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LoaiSPs.DeleteOnSubmit(l);
            db.SubmitChanges();
            return RedirectToAction("QLLoai");
        }
        //nhasx
        public ActionResult QLNsx(int? page, string search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.NhaSanXuats.Where(n => n.TenNSX.StartsWith(search) || search == null).ToList().OrderBy(n => n.MaNSX).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpGet]
        public ActionResult ThemNsx()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNsx(NhaSanXuat nsx)
        {
            if (ModelState.IsValid)
            {
                db.NhaSanXuats.InsertOnSubmit(nsx);
                db.SubmitChanges();
            }
            return RedirectToAction("QLNsx");
        }
        [HttpGet]
        public ActionResult SuaNsx(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
                if (nsx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(nsx);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNsx(FormCollection f)
        {
            NhaSanXuat nsx = db.NhaSanXuats.First(d => d.MaNSX == int.Parse(f["MaNSX"]));
            if (ModelState.IsValid)
            {
                nsx.TenNSX = f["TenNSX"];
                UpdateModel(nsx);
                db.SubmitChanges();
            }
            return RedirectToAction("QLNsx");
        }

        [HttpGet]
        public ActionResult XoaNsx(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                NhaSanXuat th = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);

                if (th == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(th);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost, ActionName("XoaNsx")]

        public ActionResult XacnhanXoaNsx(int id)
        {
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NhaSanXuats.DeleteOnSubmit(nsx);
            db.SubmitChanges();
            return RedirectToAction("QLNsx");
        }
        public ActionResult QLUser(int? page, string search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.KhachHangs.Where(n => n.Taikhoan.StartsWith(search) || search == null).ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpGet]
        public ActionResult ThemUser()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemUser(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
            }
            return RedirectToAction("QLUser");
        }
        [HttpGet]
        public ActionResult SuaUser(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaUser(FormCollection f)
        {
            KhachHang kh = db.KhachHangs.First(d => d.MaKH == int.Parse(f["MaKH"]));
            if (ModelState.IsValid)
            {
                kh.HoTen = f["HoTen"];
                kh.Taikhoan= f["Taikhoan"];
                kh.Pass= f["Pass"];
                kh.DienThoai = f["DienThoai"];
                kh.Email = f["Email"];
                UpdateModel(kh);
                db.SubmitChanges();
            }
            return RedirectToAction("QLUser");
        }

        [HttpGet]
        public ActionResult XoaUser(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);

                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost, ActionName("XoaUser")]

        public ActionResult XacnhanxoaUser(int id)
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KhachHangs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("QLUser");
        }
        public ActionResult QLGiaodich(int? page, String search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.GiaoDiches.Where(n=>n.KhachHang.HoTen.StartsWith(search)|| search == null).ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpGet]
        public ActionResult SuaGiaodich(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                GiaoDich gd = db.GiaoDiches.SingleOrDefault(n => n.MaGD == id);
                
                if (gd == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MaKh = new SelectList(db.KhachHangs.ToList().OrderBy(n => n.HoTen), "MaKh", "HoTen", gd.MaKH);
                return View(gd);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaGiaodich(FormCollection f)
        {
            GiaoDich gd = db.GiaoDiches.First(d => d.MaGD == int.Parse(f["MaGD"]));
            if (ModelState.IsValid)
            {
                gd.MaKH = int.Parse(f["MaKH"]);
                gd.NgayMua = DateTime.Now;
                UpdateModel(gd);
                db.SubmitChanges();
            }
            return RedirectToAction("QLGiaodich");
        }

        [HttpGet]
        public ActionResult XoaGiaodich(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                GiaoDich gd = db.GiaoDiches.SingleOrDefault(n => n.MaGD == id);

                if (gd == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(gd);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost, ActionName("XoaGiaodich")]

        public ActionResult XacnhanxoaGiaodich(int id)
        {
            GiaoDich gd = db.GiaoDiches.SingleOrDefault(n => n.MaGD == id);
            if (gd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.GiaoDiches.DeleteOnSubmit(gd);
            db.SubmitChanges();
            return RedirectToAction("QLGiaodich");
        }


        [HttpPost]
        public ActionResult Xoadh(int[] dsxoa)
        {
            foreach (int Id in dsxoa)
            {
                DonHang x = db.DonHangs.SingleOrDefault(n => n.MaDH == Id);
                CTDH c= db.CTDHs.SingleOrDefault(n => n.MaDH == Id);
                db.DonHangs.DeleteOnSubmit(x);
                db.CTDHs.DeleteOnSubmit(c);

            }
            db.SubmitChanges();
            return RedirectToAction("DonHang");
        }


        public ActionResult Suadonhang(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                DonHang gd = db.DonHangs.SingleOrDefault(n => n.MaDH == id);

                if (gd == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(gd);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suadonhang(FormCollection f)
        {
            DonHang gd = db.DonHangs.First(d => d.MaDH == int.Parse(f["MaDH"]));
            if (ModelState.IsValid)
            {
              
                UpdateModel(gd);
                db.SubmitChanges();
            }
            return RedirectToAction("DonHang");
        }


        //ncc
        public ActionResult NhaCungCap(int? page, String search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.NhaCungCaps.Where(n => n.TenNCC.StartsWith(search) || search == null).ToList().OrderBy(n => n.TenNCC).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }

        [HttpGet]
        public ActionResult ThemNhaCC()
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                
                return View();
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost]
        public ActionResult ThemNhaCC(NhaCungCap nhaCungCap)
        {
            
                if (ModelState.IsValid)
                {
                    db.NhaCungCaps.InsertOnSubmit(nhaCungCap);
                    db.SubmitChanges();
                }
            
            return RedirectToAction("NhaCungCap");
        }

        //SuaSP
        [HttpGet]
        public ActionResult SuaNhaCC(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.ID == id);

                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ncc);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost, ActionName("SuaNhaCC")]
        [ValidateInput(false)]
        public ActionResult xacnhansuaNCC(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.ID == id);
            
            UpdateModel(ncc);
            db.SubmitChanges();
            return RedirectToAction("NhaCungCap");
        }

        [HttpPost]
        public ActionResult XoaNCC(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.ID == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NhaCungCaps.DeleteOnSubmit(ncc);
            db.SubmitChanges();
            return RedirectToAction("NhaCungCap");
        }

        //Quang cao
        public ActionResult QuangCao(int? page, String search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.QuangCaos.Where(n => n.TenQC.StartsWith(search) || search == null).ToList().OrderBy(n => n.TenQC).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }

        [HttpGet]
        public ActionResult ThemQC(QuangCao quangCao, HttpPostedFileBase fileUpload)
        {

            ViewBag.IDNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "ID", "TenNCC");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hành ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    quangCao.img = fileName;
                    db.QuangCaos.InsertOnSubmit(quangCao);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("QuangCao");
        }
        [HttpPost]
        public ActionResult ThemQC(QuangCao quangcao)
        {

            if (ModelState.IsValid)
            {
                db.QuangCaos.InsertOnSubmit(quangcao);
                db.SubmitChanges();
            }
            ViewBag.IDNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "ID", "TenNCC");
            return RedirectToAction("QuangCao");
        }

        [HttpGet]
        public ActionResult SuaQC(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                QuangCao qc = db.QuangCaos.SingleOrDefault(n => n.ID == id);

                if (qc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.IDNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "ID", "TenNCC");
                return View(qc);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost, ActionName("SuaQC")]
        [ValidateInput(false)]
        public ActionResult xacnhansuaQC(int id)
        {

            ViewBag.IDNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "ID", "TenNCC");
            QuangCao qc = db.QuangCaos.SingleOrDefault(n => n.ID == id);
            
            UpdateModel(qc);
            db.SubmitChanges();
            return RedirectToAction("QuangCao");
        }

        [HttpPost]
        public ActionResult XoaQC(int id)
        {
            QuangCao qc = db.QuangCaos.SingleOrDefault(n => n.ID == id);
            if (qc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.QuangCaos.DeleteOnSubmit(qc);
            db.SubmitChanges();
            return RedirectToAction("QuangCao");
        }

        //Banner
        public ActionResult Banner(int? page, String search)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.Banners.Where(n => n.TenBanner.StartsWith(search) || search == null).ToList().OrderBy(n => n.TenBanner).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpGet]
        public ActionResult ThemBanner(Banner banner, HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hành ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    banner.img = fileName;
                    db.Banners.InsertOnSubmit(banner);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Banner");
        }
        [HttpPost]
        public ActionResult ThemBanner(Banner banner)
        {

            if (ModelState.IsValid)
            {
                db.Banners.InsertOnSubmit(banner);
                db.SubmitChanges();
            }

            return RedirectToAction("NhaCungCap");
        }

        [HttpGet]
        public ActionResult SuaBanner(int id)
        {
            if (Session["TaikhoanAdmin"] != null)
            {
                Banner banner = db.Banners.SingleOrDefault(n => n.ID == id);

                if (banner == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                
                return View(banner);
            }
            return RedirectToAction("Login", "Nguoidung");
        }
        [HttpPost, ActionName("SuaBanner")]
        [ValidateInput(false)]
        public ActionResult xacnhansuaBanner(int id)
        {
            Banner banner = db.Banners.SingleOrDefault(n => n.ID == id);
            UpdateModel(banner);
            db.SubmitChanges();
            return RedirectToAction("Banner");
        }

        [HttpPost]
        public ActionResult XoaBanner(int id)
        {
            Banner banner = db.Banners.SingleOrDefault(n => n.ID == id);
            if (banner == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Banners.DeleteOnSubmit(banner);
            db.SubmitChanges();
            return RedirectToAction("Banner");
        }
    }
}