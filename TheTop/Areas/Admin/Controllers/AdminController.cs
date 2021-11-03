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

namespace TheTop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        const string key = "UserId";

        private readonly TheTopContext db;
        [BindProperty]
        public AdminViewModel AdminVM { get; set; }
        public AdminController(TheTopContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(uid),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
            };
            return View(AdminVM);
        }
        public IActionResult Create()
        {
            ViewData["EmpId"] = new SelectList(db.Employee, "EmpId", "EmpId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Category,Description,EmpId")] Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                challenge.IsActive = true;
                db.Add(challenge);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpId"] = new SelectList(db.Employee, "EmpId", "EmpId", challenge.EmpId);
            return View(challenge);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await db.Challenge.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["EmpId"] = new SelectList(db.Employee, "EmpId", "EmpId", task.EmpId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Category,Description,EmpId")] Challenge challenge)
        {
            if (id != challenge.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    db.Update(challenge);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!challengeExists(challenge.TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpId"] = new SelectList(db.Employee, "EmpId", "EmpId", challenge.EmpId);
            return View(challenge);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await db.Challenge
                .Include(t => t.Emp)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challenge = await db.Challenge.FindAsync(id);
            db.Challenge.Remove(challenge);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool challengeExists(int id)
        {
            return db.Challenge.Any(e => e.TaskId == id);
        }
        public async Task<IActionResult> Sales()
        {
            var theTopContext = db.Payment.Include(u => u.User).ToListAsync();
            return View(await theTopContext);
        }
        [HttpPost]
        public async Task<IActionResult> Sales(DateTime fromDate , DateTime toDate)
        {
            var theTopContext = db.Payment.Include(u => u.User).Where(m=>m.PaymentDate>= fromDate && m.PaymentDate <=toDate).ToListAsync();
            return View(await theTopContext);
        }

        public IActionResult monthExp(string month)
        {
             
            var MonthlySales = db.Payment.Include(u => u.User).ToList();
            List<Users> user = db.Users.ToList();


            var builder = new StringBuilder();
            builder.AppendLine("PaymentId,Amount,Profits,PaymentDate,CustomerName,PaymentType");
            foreach (var item in MonthlySales.Where(m=>m.PaymentMonth==month))
            {
                    var profit = item.Amount * (decimal)0.10;

                        builder.AppendLine($"{item.PaymentId},{item.Amount + "$"},{profit + "$"},{item.PaymentDate},{item.User.UsarName},{item.PaymentType}");

               
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "MonthlySales "+month+".csv");
        }
        public IActionResult YearExp(string Year)
        {

            var MonthlySales = db.Payment.Include(u => u.User).ToList();
            List<Users> user = db.Users.ToList();


            var builder = new StringBuilder();
            builder.AppendLine("PaymentId,Amount,Profits,PaymentDate,CustomerName,PaymentType");
            foreach (var item in MonthlySales.Where(m => m.PaymentYear == Year))
            {
                        var profit = item.Amount * (decimal)0.10;
                        builder.AppendLine($"{item.PaymentId},{item.Amount + "$"},{profit + "$"},{item.PaymentDate},{item.User.UsarName},{item.PaymentType}");

            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "AnnualSales " + Year + ".csv");
        }
        public IActionResult monthEmp(string month)
        {


            var theTopContext = db.Employee.Include(e => e.User);

            List<Users> user = db.Users.ToList();


            var builder = new StringBuilder();
            builder.AppendLine("EmpId,Employee Name,Salary,discount");
            foreach (var item in theTopContext.Where(m => m.Month == month))
            {
                foreach (var s in user)
                {
                    if (s.UserId == item.UserId)
                    {
                        if (item.Discounts !=null)
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts }");
                        }
                        else
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts }");

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
            builder.AppendLine("EmpId,Employee Name,Salary,discount");
            foreach (var item in theTopContext.Where(m => m.Year == Year))
            {
                foreach (var s in user)
                {
                    if (s.UserId == item.UserId)
                    {
                        if (item.Discounts != null)
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts}");
                        }
                        else
                        {
                            builder.AppendLine($"{item.EmpId},{s.UsarName},{item.Salary + "$"},{item.Discounts }");

                        }
                    }
                }
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "annualsalaries " + Year + ".csv");
        }
       public IActionResult CreateEmployeeTasks()
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(uid),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
            };
            return View(AdminVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployeeTask()
        {
            if (ModelState.IsValid)
            {
                AdminVM.EmployeeTask.IsActive = true;
                db.EmployeeTasks.Add(AdminVM.EmployeeTask);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(AdminVM);
        }

        public async Task<IActionResult> EditEmployeeTasks(int? id)
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(uid),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
            };
            if (id == null)
            {
                return NotFound();
            }

            var employeeTasks = await db.EmployeeTasks.Include(m=>m.User).Include(m=>m.Role).SingleOrDefaultAsync(m=>m.Id==id);
            if (employeeTasks == null)
            {
                return NotFound();
            }
            AdminVM.EmployeeTask = employeeTasks;
            AdminVM.Users = await db.Users.Where(m => m.RoleId == AdminVM.EmployeeTask.RoleId).ToListAsync();
            return View(AdminVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployeeTasks()
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(uid),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
            };
            if (ModelState.IsValid)
            {
                try
                {

                    db.Update(AdminVM.EmployeeTask);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeTasksExists(AdminVM.EmployeeTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(AdminVM);
        }

        public async Task<IActionResult> DeleteEmployeeTasks(int? id)
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(uid),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
            };
            if (id == null)
            {
                return NotFound();
            }

            var employeeTasks = await db.EmployeeTasks
                .Include(e => e.Role)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeTasks == null)
            {
                return NotFound();
            }
            AdminVM.EmployeeTask = employeeTasks;
            AdminVM.Users = await db.Users.Where(m => m.RoleId == AdminVM.EmployeeTask.RoleId).ToListAsync();
            return View(AdminVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeTasks()
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(uid),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
            };
            db.EmployeeTasks.Remove(AdminVM.EmployeeTask);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeTasksExists(int id)
        {
            return db.EmployeeTasks.Any(e => e.Id == id);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers(int id)
        {
            List<Users> users = new List<Users>();
            users = await db.Users.Where(m => m.RoleId == id).ToListAsync();

            return Json(new SelectList(users, "UserId", "UsarName"));
        }
        public IActionResult Testimonials()
        {
            var uid = HttpContext.Session.GetInt32(key);

            AdminVM = new AdminViewModel()
            {
                User = db.Users.Find(20),
                Payments = db.Payment.ToList(),
                Employee = db.Employee.ToList(),
                Product = db.Product.ToList(),
                Challenge = db.Challenge.ToList(),
                EmployeeTasks = db.EmployeeTasks.Include(m => m.Role).Include(m => m.User).ToList(),
                Users = db.Users.ToList(),
                Roles = db.Role.Where(m => m.RoleId != 4 && m.RoleId != 2).ToList(),
                EmployeeTask = new EmployeeTasks(),
                Testimonials = db.Testimonial.ToList()
            };
            return View(AdminVM);
        }
        public IActionResult Prove(int id)
        {
            var Testimonial = db.Testimonial.Find(id);
            Testimonial.Approved = true;
            db.Testimonial.Update(Testimonial);
            db.SaveChanges();
            return RedirectToAction("Testimonials");
        }
    }
}
