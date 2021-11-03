using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTop.Models;
using TheTop.Models.ViewModel;

namespace TheTop.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class AccountantController : Controller
    {
        const string key = "UserId";

        private readonly TheTopContext db;
        [BindProperty]
        public AccountantViewModel AccountantVM { get; set; }
        public AccountantController(TheTopContext db)
        {

            this.db = db;
        }
        public IActionResult Index()
        {
            var uid = HttpContext.Session.GetInt32(key);

            AccountantVM = new AccountantViewModel()
            {
                User = db.Users.Find(uid),
                Users = db.Users.ToList(),
                Employees = db.Employee.ToList(),
            };
            return View(AccountantVM);
        }
        public IActionResult monthEmp(string month)
        {


            var theTopContext = db.Employee.Include(e => e.User);

            List<Users> user = db.Users.ToList();


            var builder = new StringBuilder();
            builder.AppendLine("EmpId,Employee Name,Salary,discount,Discounts Description");
            foreach (var item in theTopContext.Where(m => m.Month == month))
            {
                foreach (var s in user)
                {
                    if (s.UserId == item.UserId)
                    {
                        if (item.Discounts != null)
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts},{item.DiscountsDescription}");
                        }
                        else
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts },{item.DiscountsDescription}");

                        }
                    }
                }
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Monthlysalaries " + month + ".csv");
        }
        public IActionResult YearEmp(string Year)
        {

            var theTopContext = db.Employee.Include(e => e.User);

            List<Users> user = db.Users.ToList();


            var builder = new StringBuilder();
            builder.AppendLine("EmpId,Employee Name,Salary,discount,Discounts Description");
            foreach (var item in theTopContext.Where(m => m.Year == Year))
            {
                foreach (var s in user)
                {
                    if (s.UserId == item.UserId)
                    {
                        if (item.Discounts != null)
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts},{item.DiscountsDescription}");
                        }
                        else
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts },{item.DiscountsDescription}");

                        }
                    }
                }
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "annualsalaries " + Year + ".csv");
        }
    }
}
