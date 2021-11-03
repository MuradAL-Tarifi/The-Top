using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTop.Models;

namespace TheTop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {

        private readonly TheTopContext db;

        public EmployeeController(TheTopContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var theTopContext = db.Employee.Include(e => e.User);
            
            return View(await theTopContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await db.Employee
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName");

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpId,DiscountsDescription,Discounts,Salary,LastLogout,LastLogin,CreatedAt,UserId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedAt = DateTime.Now;
                var user =await db.Users.FindAsync(employee.UserId);
                db.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName", employee.UserId);
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await db.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName", employee.UserId);
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpId,DiscountsDescription,Discounts,Salary,LastLogout,LastLogin,CreatedAt,UserId")] Employee employee)
        {
            if (id != employee.EmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName", employee.UserId);
                    db.Update(employee);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmpId))
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
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName", employee.UserId);
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await db.Employee
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName", employee.UserId);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await db.Employee.FindAsync(id);
            ViewData["UserId"] = new SelectList(db.Users, "UserId", "UsarName", employee.UserId);
            db.Employee.Remove(employee);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return db.Employee.Any(e => e.EmpId == id);
        }
    }
}
