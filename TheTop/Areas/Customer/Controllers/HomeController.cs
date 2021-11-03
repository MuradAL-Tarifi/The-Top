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
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace TheTop.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly TheTopContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public HomeViewModel HomeVM { get; set; }
        public HomeController(TheTopContext db, IWebHostEnvironment webHostEnvironment)
        {
            
            HomeVM = new HomeViewModel()
            {
                Products = db.Product.Include(p => p.Category).Include(p => p.User).ToList(),
                Categorys = db.Category.ToList(),
                Users = db.Users.ToList(),
                Testimonials = db.Testimonial.Where(m=>m.Approved==true).ToList()
            };
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Home()
        {
            return View(HomeVM);
        }
        [HttpPost]
        public IActionResult Home(int Category, DateTime Expire)
        {
            DateTime dt = new DateTime();

            if (Category == 0 && Expire == dt)
            {
                HomeVM = new HomeViewModel()
                {
                    Products = db.Product.Include(p => p.Category).Include(p => p.User).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.ToList(),
                    Testimonials = db.Testimonial.ToList()
                };
            }
            else if (Category != 0 && Expire == dt)
            {
                HomeVM = new HomeViewModel()
                {
                    Products = db.Product.Include(p => p.Category).Include(p => p.User).Where(m => m.CategoryId == Category).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.ToList(),
                    Testimonials = db.Testimonial.ToList()
                };
            }
            else if (Category == 0 && Expire != dt)
            {
                HomeVM = new HomeViewModel()
                {
                    Products = db.Product.Include(p => p.Category).Include(p => p.User).Where(m => m.Expire < Expire).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.ToList(),
                    Testimonials = db.Testimonial.ToList()
                };
            }
            else
            {
                HomeVM = new HomeViewModel()
                {
                    Products = db.Product.Where(m => m.Expire < Expire && m.CategoryId == Category).Include(p => p.Category).Include(p => p.User).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.ToList(),
                    Testimonials = db.Testimonial.ToList()
                };
            }

            return View(HomeVM);
        }
        [HttpPost]
        public IActionResult Contact(string Name, string email, string phone, string message)
        {

            Testimonial testimonial = new Testimonial();
            testimonial.Name = Name;
            testimonial.Email = email;
            testimonial.Phone = phone;
            testimonial.Message = message;

            db.Add(testimonial);
            db.SaveChanges();

            return RedirectToAction("Home");
        }
    }
    }
