using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Car.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index(string name, string email, string message)
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
                WebMail.Password = "";

                //Sender email address.  
                WebMail.From = "buiquangductri@gmail.com";

                //Send email  
                WebMail.Send(to: email, subject: "góp ý", body: email + "  đã gửi "+ name +" là người  " + " nội dung " + message, cc: null, bcc: null, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }
    }
}