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
    public class UserController : Controller
    {

        private readonly TheTopContext db;
        public UserController(TheTopContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var theTopContext = db.Users.Include(u => u.Role);
            return View(await theTopContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UsarName,Password,Address,Email,Photo,RoleId")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Add(users);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1", users.RoleId);
            return View(users);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1", users.RoleId);
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UsarName,Password,Address,Email,Photo,RoleId")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(users);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1", users.RoleId);
            return View(users);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1", users.RoleId);
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await db.Users.FindAsync(id);
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1", users.RoleId);
            db.Users.Remove(users);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            ViewData["RoleId"] = new SelectList(db.Role, "RoleId", "Role1", users.RoleId);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }
        private bool UsersExists(int id)
        {
            return db.Users.Any(e => e.UserId == id);
        }
    }
}
