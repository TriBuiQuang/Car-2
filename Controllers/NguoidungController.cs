using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car.Models;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Car.Controllers
{

    public class NguoidungController : Controller
    {
        dbCAR2DataContext db = new dbCAR2DataContext();
        // GET: Nguoidung

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplai = collection["Nhaplaimk"];
            var dienthoai = collection["Dienthoai"];
            var email = collection["Diachimail"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = "Nhập họ tên!";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["loi2"] = "Nhập tên đăng nhập!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi3"] = "Nhập mật khẩu!";
            }
            else if (String.IsNullOrEmpty(nhaplai) && nhaplai != matkhau)
            {
                ViewData["loi4"] = "Phải khớp với mật khẩu!";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["loi5"] = "Nhập điên thoại";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["loi6"] = "Nhập Email!";
            }
            else
            {
                var t = from a in db.KhachHangs where a.Taikhoan == collection["TenDN"].ToString() select a;
                if (t.ToList().Count!=0 )
                {
                    ViewData["loi7"] = "dang ky loi!";
                }
                else
                {
                    try
                    {
                        //Configuring webMail class to send emails  
                        //gmail smtp server  
                        WebMail.SmtpServer = "smtp.gmail.com";
                        //gmail port to send emails  
                        WebMail.SmtpPort = 587;
                        WebMail.SmtpUseDefaultCredentials = true;
                        //sending emails with secure protocol  
                        WebMail.EnableSsl = true;
                        //EmailId used to send emails from application  
                        WebMail.UserName = "buiquangductri@gmail.com"; // email de gui
                        WebMail.Password = "";//nhap mat khau vao day ^^ 

                        //Sender email address.  
                        WebMail.From = "buiquangductri@gmail.com"; // nguoi gui email

                        //Send email  
                        WebMail.Send(to: email, subject: "cảm ơn bạn đã đăng kí tới DCG-Cars", body: "Tai khoan : " + tendn + " Pass : " + matkhau, cc: null, bcc: null, isBodyHtml: true);

                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";

                    }
                    
                    kh.HoTen = hoten;
                    kh.Taikhoan = tendn;
                    kh.Pass = matkhau;
                    kh.Email = email;
                    kh.DienThoai = dienthoai;
                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    return RedirectToAction("Dangnhap");
                }
            }
            
            return this.Dangky();
        }
        public ActionResult TaiKhoan()
        {
            if (Session["TaiKhoan"] == null )
            {
                return PartialView();
            }
            else 
            {
                    var query = db.KhachHangs.SingleOrDefault(k => k.Taikhoan == Session["Taikhoan"].ToString());
                    return PartialView(query);
            }
        }
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            KhachHang tk = db.KhachHangs.SingleOrDefault(n => n.Taikhoan == tendn && n.Pass == matkhau);
             if (tk != null)
            {
                ViewBag.ThongBao = "Chúc mừng bạn đã đăng nhập thành công!";
                Session["TaiKhoan"] = tk.Taikhoan;
                Session["KH"] = tk;
                Session["MaKH"] = tk.MaKH;
                Session["TenKH"] = tk.HoTen;
                Session["EmailKH"] = tk.Email;
                return RedirectToAction("Index", "Car");
            }
            else ViewBag.Thongbao = "Tên đăng nhập hoặc tài khoản không đúng";
            return View();
           
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            Session["Giohang"] = null;
            return RedirectToAction("Index", "Car");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.Taikhoan == tendn && n.Pass == matkhau);
                if (ad != null)
                {
                    Session["TaikhoanAdmin"] = ad.HoTen;
                    Session["AdminRole"] = ad.Role;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["TaikhoanAdmin"] = null;
            return RedirectToAction("Index", "WatchStore");
        }
        public ActionResult TTNguoiDung(int id)
        {
            if (Session["TaiKhoan"] != null)
            {
                var ttkh = from s in db.KhachHangs
                         where s.MaKH == id
                         select s;
                return View(ttkh.Single());
            }
            return RedirectToAction("Index", "Car");
        }
        [HttpGet]
        public ActionResult SuaTT(int id)
        {
            if (Session["TaiKhoan"] != null)
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);

                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("index", "Car");
        }
        [HttpPost, ActionName("SuaTT")]
        [ValidateInput(false)]
        public ActionResult xacnhansuaTT(int id)
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);

            UpdateModel(kh);
            db.SubmitChanges();
            return RedirectToAction("TTNguoiDung","NguoiDung",new { id=kh.MaKH});
        }

        [AllowAnonymous]
        public ActionResult QuenMK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuenMK(FormCollection f)
        {
            var email = f["Email"];
            
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.Email == email);
            if (kh == null)
            {
                ViewData["loi1"] = "khong co email nay";
            }
            else
            {
                try
                {
                    //Configuring webMail class to send emails  
                    //gmail smtp server  
                    WebMail.SmtpServer = "smtp.gmail.com";
                    //gmail port to send emails  
                    WebMail.SmtpPort = 587;
                    WebMail.SmtpUseDefaultCredentials = true;
                    //sending emails with secure protocol  
                    WebMail.EnableSsl = true;
                    //EmailId used to send emails from application  
                    WebMail.UserName = "buiquangductri@gmail.com";
                    WebMail.Password = ""; //nhap mat khau vao day

                    //Sender email address.  
                    WebMail.From = "buiquangductri@gmail.com";

                    //Send email  
                    WebMail.Send(to: email, subject: "góp ý", body: "Tai khoan : " + kh.Taikhoan  + " Pass : " + kh.Pass, cc: null, bcc: null, isBodyHtml: true);

                }
                catch (Exception)
                {
                    ViewBag.Status = "Problem while sending email, Please check details.";

                }
                return RedirectToAction("Xacnhanquenemail");
            }
            return View();
        }
        public ActionResult Xacnhanquenemail()
        {
            return View();
        }

    }
}