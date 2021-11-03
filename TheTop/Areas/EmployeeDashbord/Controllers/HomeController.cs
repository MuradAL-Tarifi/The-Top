using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTop.Models;
using TheTop.Models.ViewModel;

namespace TheTop.Areas.EmployeeDashbord.Controllers
{
    [Area("EmployeeDashbord")]
    public class HomeController : Controller
    {
        const string key = "UserId";
        private readonly TheTopContext db;
        [BindProperty]
        public EmployeeViewModer EmployeeVM { get; set; }
        public HomeController(TheTopContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32(key);
            var empid = db.Employee.Where(m => m.UserId == id).FirstOrDefault().EmpId;
            EmployeeVM = new EmployeeViewModer()
            {
                Users = db.Users.Find(HttpContext.Session.GetInt32("UserId")),
                Employee = db.Employee.SingleOrDefault(m => m.UserId == HttpContext.Session.GetInt32("UserId")),
                Attendances = db.Attendance.Where(m => m.EmpId == empid).ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Where(m => m.UserId == HttpContext.Session.GetInt32("UserId") && m.IsActive == true).Include(m => m.Role).Include(m => m.User).ToList(),

            };

            DateTime dt3 = new DateTime(2015, 5, 5, 9, 07, 08);

            Attendance att = new Attendance();
            att.EmpId = EmployeeVM.Employee.EmpId;
            att.LastLogin = DateTime.Now.ToString("HH:mm:ss");
            att.Day = DateTime.Today;
            if (DateTime.Now.Hour > dt3.Hour)
            {
                var emp = db.Employee.Find(EmployeeVM.Employee.EmpId);
                emp.Discounts = emp.Discounts + 5;
                db.Employee.Update(emp);

            }

            db.Attendance.Add(att);
            db.SaveChanges();
            HttpContext.Session.SetInt32("ATTID", att.Id);

            return View(EmployeeVM);
        }
        public IActionResult Done(int id)
        {
            var uid = HttpContext.Session.GetInt32(key);
            var empid = db.Employee.Where(m => m.UserId == uid).FirstOrDefault().EmpId;
            EmployeeVM = new EmployeeViewModer()
            {
                Users = db.Users.Find(HttpContext.Session.GetInt32("UserId")),
                Employee = db.Employee.SingleOrDefault(m => m.UserId == HttpContext.Session.GetInt32("UserId")),
                Attendances = db.Attendance.Where(m => m.EmpId == empid).ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Where(m => m.UserId == HttpContext.Session.GetInt32("UserId") && m.IsActive == true).Include(m => m.Role).Include(m => m.User).ToList(),

            };
            var task = EmployeeVM.Challenge.Where(m => m.TaskId == id).FirstOrDefault();
            task.IsActive = false;
            task.EmpId = EmployeeVM.Employee.EmpId;
            db.Challenge.Update(task);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DoneTask(int id)
        {
            var uid = HttpContext.Session.GetInt32(key);
            var empid = db.Employee.Where(m => m.UserId == uid).FirstOrDefault().EmpId;
            EmployeeVM = new EmployeeViewModer()
            {
                Users = db.Users.Find(HttpContext.Session.GetInt32("UserId")),
                Employee = db.Employee.SingleOrDefault(m => m.UserId == HttpContext.Session.GetInt32("UserId")),
                Attendances = db.Attendance.Where(m => m.EmpId == empid).ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Where(m => m.UserId == HttpContext.Session.GetInt32("UserId") && m.IsActive == true).Include(m => m.Role).Include(m => m.User).ToList(),

            };
            var task = EmployeeVM.EmployeeTasks.Where(m => m.Id == id).FirstOrDefault();
            task.IsActive = false;
            db.EmployeeTasks.Update(task);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            var uid = HttpContext.Session.GetInt32(key);
            var empid = db.Employee.Where(m => m.UserId == uid).FirstOrDefault().EmpId;
            EmployeeVM = new EmployeeViewModer()
            {
                Users = db.Users.Find(HttpContext.Session.GetInt32("UserId")),
                Employee = db.Employee.SingleOrDefault(m => m.UserId == HttpContext.Session.GetInt32("UserId")),
                Attendances = db.Attendance.Where(m => m.EmpId == empid).ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Where(m => m.UserId == HttpContext.Session.GetInt32("UserId") && m.IsActive == true).Include(m => m.Role).Include(m => m.User).ToList(),

            };

            string dateStringBm = "7/10/1974 5:00:00 PM";
            DateTime dateFromStringbm =
                DateTime.Parse(dateStringBm, System.Globalization.CultureInfo.InvariantCulture);

            DateTime dt3 = new DateTime(2015, 5, 5, 5, 07, 08);
            var id = HttpContext.Session.GetInt32("ATTID");
            var att = db.Attendance.Find(id);
            att.LastLogout= DateTime.Now.ToString("HH:mm:ss");
            var X = dateFromStringbm.Hour;

                if (DateTime.Now.Hour< dateFromStringbm.Hour)
                {
                    var emp = db.Employee.Find(EmployeeVM.Employee.EmpId);
                    emp.Discounts = emp.Discounts + 5;
                    db.Employee.Update(emp);

                }
            if (DateTime.Now.Hour > dateFromStringbm.Hour)
            {
                var emp = db.Employee.Find(EmployeeVM.Employee.EmpId);
                var increas = DateTime.Now.Hour - dateFromStringbm.Hour;
                emp.Increase = emp.Increase + (5* increas);
                db.Employee.Update(emp);

            }

            db.Attendance.Update(att);
            db.SaveChanges();
            return RedirectToAction("Home", "Home", new { area = "Customer" });
        }
        public IActionResult SlipSalary()
        {

            FileStream docStream = new FileStream("wwwroot/Slip.pdf", FileMode.Open, FileAccess.Read);

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);

            PdfLoadedForm form = loadedDocument.Form;
            var emp = db.Employee.Where(m => m.UserId == 3).Include(m=>m.User).FirstOrDefault();

            (form.Fields[0] as PdfLoadedTextBoxField).Text = emp.User.UsarName;
            (form.Fields[1] as PdfLoadedTextBoxField).Text = DateTime.Now.Month.ToString();
            (form.Fields[2] as PdfLoadedTextBoxField).Text = DateTime.Now.Year.ToString();

            (form.Fields[3] as PdfLoadedTextBoxField).Text = emp.Salary.ToString();
            (form.Fields[4] as PdfLoadedTextBoxField).Text = emp.Discounts.ToString();
            (form.Fields[5] as PdfLoadedTextBoxField).Text = emp.Increase.ToString();
            var total = emp.Salary + emp.Increase - emp.Discounts;
            (form.Fields[6] as PdfLoadedTextBoxField).Text = total.ToString();
            (form.Fields[7] as PdfLoadedTextBoxField).Text = "Islamic Bank";
            (form.Fields[8] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("dd MMMM yyyy");

            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            string contentType = "application/pdf";
            string fileName = "Slip.pdf";
            return File(stream, contentType, fileName);
        }
    }
}
