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
using System.Net.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

using Facebook;
using Microsoft.AspNetCore.Authorization;

namespace TheTop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class UserController : Controller
    {
        const string key = "UserId";

        string appid = string.Empty;
        string appsecret = string.Empty;

        private readonly TheTopContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

      
        [BindProperty]
        public IndexViewModel IndexVM { get; set; }
        [BindProperty]
        public MyAdsViewModel MyAdsVM { get; set; }
        [BindProperty]
        public CheckoutViewModel CheckoutVM { get; set; }
        [BindProperty]
        public FavoriteViewModel FavoriteVM { get; set; }
        public UserController(TheTopContext db , IWebHostEnvironment webHostEnvironment )
        {

            var configuration = GetConfiguration();
            appid = configuration.GetSection("AppID").Value;
            appsecret = configuration.GetSection("AppSecret").Value;
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Users users)
        {

            var user = await db.Users.Where(m => m.Email == users.Email && m.Password == users.Password).FirstOrDefaultAsync();
            if (user == null)
            {
                ModelState.AddModelError("Wrong", "Wrong username or password");
                return View();
            }
            else if (user.RoleId == 1)
            {
                HttpContext.Session.SetInt32(key, user.UserId);
                return RedirectToAction("Index", "Home", new { area = "EmployeeDashbord" });
            }
            else if (user.RoleId == 2)
            {
                HttpContext.Session.SetInt32(key, user.UserId);
                return RedirectToAction("Index", "Admin",
                        new { area = "Admin" });
            }
            else if (user.RoleId == 3)
            {
                HttpContext.Session.SetInt32(key, user.UserId);
                return RedirectToAction("Index", "Accountant",
                                        new { area = "Accountant" });
            }
            else if (user.RoleId == 4)
            {
                HttpContext.Session.SetInt32(key, user.UserId);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

            //HttpContext.Session.GetString("Test");


            
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users users)
        {
            var user = await db.Users.Where(m => m.Email == users.Email).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                if (user==null)
                {

                
                var files = HttpContext.Request.Form.Files;
                string ImagePath = null;

                if (files.Count > 0)
                {
                    string webRootPath = webHostEnvironment.WebRootPath;

                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);

                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImageName), FileMode.Create);

                    files[0].CopyTo(fileStream);

                    ImagePath = @"\Images\" + ImageName;
                }

                users.UserPhoto = ImagePath;


                users.RoleId = 4;
                users.CreatedAt =Convert.ToDateTime(DateTime.Now.ToString("MM-dd-yyyy"));

                db.Add(users);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("Wrong", "This email has already been used");
                    return View(users);

                }
            }
            return View(users);
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (IsEmailExist(email))
            {
                var userInfo = db.Users.AsNoTracking().Where(x => x.Email == email).ToList().FirstOrDefault();
                SendPasswrodLinkEmail(userInfo.Email);
                var message = "Send new paswword successfully done. has been sent to your email " + userInfo.Email;
                ViewBag.Message = message;
                return View("Login");
            }
            else
            {
                var message = "Your email not exist";
                ViewBag.Message = message;
                return View();
            }

        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            var v = db.Users.Where(a => a.Email == email).FirstOrDefault();
            return v != null;
        }

        [NonAction]
        public void SendPasswrodLinkEmail(string email)
        {
            var userPass = db.Users.Where(x => x.Email == email).ToList().FirstOrDefault();
            userPass.Password = CreateRandomPassword(9);
            db.Entry(userPass).State = EntityState.Modified;


            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject= "password chanded successfully.";
            mm.Body = "We are excited to tell you that your password chanded successfully.\n" + "Your New Password : " + userPass.Password;
            mm.From = new MailAddress("thetopwebsite12@gmail.com");

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("thetopwebsite12@gmail.com", "8974563120");
            smtp.Send(mm);






            db.SaveChanges();

        }
        [NonAction]
        public string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
   
        public IActionResult Index()
        {
            var uid = HttpContext.Session.GetInt32(key);
            if (uid!=null)
            {
                IndexVM = new IndexViewModel()
                {
                    Products = db.Product.Include(p => p.Category).Include(m => m.Rate).Include(p => p.User).Include(p => p.FavoriteProduct).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.Find(uid),
                    CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                    FavoriteProducts = db.FavoriteProduct.ToList(),
                    Rates = db.Rate.ToList(),
                    Coupons = db.Coupon.ToList(),
                    Notifications = db.Notification.Include(m=>m.Product).Where(m => m.UserId == uid).ToList(),

                };
                
                return View(IndexVM);
            }
            else
            {
                return RedirectToAction("Home","Home");
            }
           
        }
        [HttpPost]
        public IActionResult Index(int Category, DateTime Expire)
        {
            DateTime dt = new DateTime();
            var uid = HttpContext.Session.GetInt32(key);

            if (Category == 0 && Expire == dt)
            {
                IndexVM = new IndexViewModel()
                {
                    Products = db.Product.Where(m => m.UserId != uid).Include(p => p.Category).Include(m => m.Rate).Include(p => p.User).Include(p => p.FavoriteProduct).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.Find(uid),
                    CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                    FavoriteProducts = db.FavoriteProduct.ToList(),
                    Rates = db.Rate.ToList(),
                    Notifications = db.Notification.Include(m => m.Product).Where(m => m.UserId == uid).ToList(),
                    Coupons = db.Coupon.ToList(),
                    

                };
            }
            else if (Category != 0 && Expire == dt)
            {
                IndexVM = new IndexViewModel()
                {
                    Products = db.Product.Where(m => m.UserId != uid).Where(m => m.CategoryId == Category).Include(m => m.Rate).Include(p => p.Category).Include(p => p.User).Include(p => p.FavoriteProduct).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.Find(uid),
                    CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                    FavoriteProducts = db.FavoriteProduct.ToList(),
                    Rates = db.Rate.ToList(),
                    Notifications = db.Notification.Include(m => m.Product).Where(m => m.UserId == uid).ToList(),
                    Coupons = db.Coupon.ToList(),

                };
            }
            else if (Category == 0 && Expire != dt)
            {
                IndexVM = new IndexViewModel()
                {
                    Products = db.Product.Where(m => m.UserId != uid).Where(m => m.Expire < Expire).Include(m => m.Rate).Include(p => p.Category).Include(p => p.User).Include(p => p.FavoriteProduct).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.Find(uid),
                    CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                    Rates = db.Rate.ToList(),
                    Notifications = db.Notification.Include(m => m.Product).Where(m => m.UserId == uid).ToList(),
                    FavoriteProducts = db.FavoriteProduct.ToList(),
                    Coupons = db.Coupon.ToList(),

                };
            }
            else
            {
                IndexVM = new IndexViewModel()
                {
                    Products = db.Product.Where(m => m.UserId != uid).Where(m => m.Expire < Expire && m.CategoryId == Category).Include(p => p.Category).Include(p => p.User).Include(m=>m.Rate).Include(p => p.FavoriteProduct).ToList(),
                    Categorys = db.Category.ToList(),
                    Users = db.Users.Find(uid),
                    CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                    Rates = db.Rate.ToList(),
                    Notifications = db.Notification.Include(m => m.Product).Where(m => m.UserId == uid).ToList(),
                    FavoriteProducts = db.FavoriteProduct.ToList(),
                    Coupons = db.Coupon.ToList(),

                };
            }

            return View(IndexVM);
        }
        public async Task<IActionResult> AddToFavorite(int productId)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                var product = db.Product.Find(productId);
                FavoriteProduct fp = new FavoriteProduct();
                var fpList = await db.FavoriteProduct.Where(m => m.UserId == uid).ToListAsync();
                foreach (var item in fpList)
                {
                    if (item.ProductId == productId && item.UserId == uid)
                    {
                        db.FavoriteProduct.Remove(item);
                        await db.SaveChangesAsync();
                    }
                }
                fp.ProductId = product.ProductId;
                fp.IsFavorite = true;
                fp.UserId = uid;
                db.FavoriteProduct.Add(fp);
                await db.SaveChangesAsync();
                return Redirect("Index");
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
        }
        public async Task<IActionResult> RemoveFromFavorite(int productId)
        {
            var uid = HttpContext.Session.GetInt32(key);

            var product = db.Product.Find(productId);
            var fpList = await db.FavoriteProduct.Where(m => m.UserId == uid).ToListAsync();
            foreach (var item in fpList)
            {
                if (item.ProductId == productId && item.UserId == uid)
                {
                    db.FavoriteProduct.Remove(item);
                    await db.SaveChangesAsync();
                }
            }
            return Redirect("Index");
        }
        public async Task<IActionResult> AddToCart(int productId)
        {
            var uid = HttpContext.Session.GetInt32(key);

            var cart = await db.CartItem.Where(m => m.UserId == uid).ToListAsync();
            if (cart.Count == 0)
            {
                var product = db.Product.Find(productId);
                CartItem NewCart = new CartItem();
                NewCart.CartName = product.Name;
                NewCart.CartPrice = product.Price;
                NewCart.Cartphoto = product.Photo;
                NewCart.ProductId = product.ProductId;
                NewCart.UserId = uid;
                db.CartItem.Add(NewCart);
                await db.SaveChangesAsync();
            }
            else
            {
                var product = db.Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        var CartItem = await db.CartItem.FindAsync(item.Id);

                        db.CartItem.Remove(CartItem);
                        await db.SaveChangesAsync();

                        CartItem NewCart1 = new CartItem();
                        NewCart1.CartName = product.Name;
                        NewCart1.CartPrice = product.Price;
                        NewCart1.Cartphoto = product.Photo;
                        NewCart1.ProductId = product.ProductId;
                        NewCart1.UserId = uid;
                        db.CartItem.Add(NewCart1);
                        await db.SaveChangesAsync();
                        return Redirect("Index");
                    }

                }
                CartItem NewCart = new CartItem();
                NewCart.CartName = product.Name;
                NewCart.CartPrice = product.Price;
                NewCart.Cartphoto = product.Photo;
                NewCart.ProductId = product.ProductId;
                NewCart.UserId = uid;
                db.CartItem.Add(NewCart);
                await db.SaveChangesAsync();


            }
            return Redirect("Index");
        }
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var Cart = await db.CartItem.FindAsync(id);
            db.CartItem.Remove(Cart);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditProfile()
        {
            var uid = HttpContext.Session.GetInt32(key);
            if (uid != null)
            {
                ViewBag.error = HttpContext.Session.GetString("error");
                ViewBag.Personal = HttpContext.Session.GetString("Personal");
                ViewBag.Email = HttpContext.Session.GetString("Email");

                if (uid != null)
                {
                    ViewBag.MyAds = db.Product.Where(m => m.UserId == uid).Count();
                    ViewBag.Facourite = db.FavoriteProduct.Where(m => m.UserId == uid).Count();
                    return View(await db.Users.FindAsync(uid));
                }
                else
                {
                    return RedirectToAction("Home", "Home");
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
           
        }
        [HttpPost]
        public async Task<IActionResult> EditPersonal(Users users)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                var user = await db.Users.FindAsync(users.UserId);
                user.UsarName = users.UsarName;
                user.Address = users.Address;

                var files = HttpContext.Request.Form.Files;
                string ImagePath = users.UserPhoto;

                if (files.Count > 0)
                {
                    string webRootPath = webHostEnvironment.WebRootPath;

                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);

                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImageName), FileMode.Create);

                    files[0].CopyTo(fileStream);

                    ImagePath = @"\Images\" + ImageName;
                }

                user.UserPhoto = ImagePath;

                db.Users.Update(user);
                await db.SaveChangesAsync();
                HttpContext.Session.SetString("Personal", "Data Changed");

                return RedirectToAction(nameof(EditProfile));
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(Users users, string newpassword, string confirmpassword)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                var user = await db.Users.FindAsync(users.UserId);

                if (users.Password == user.Password)
                {
                    if (newpassword == confirmpassword)
                    {
                        user.Password = newpassword;
                        db.Users.Update(user);
                        await db.SaveChangesAsync();
                        HttpContext.Session.SetString("error", "Data Changed");
                        return RedirectToAction(nameof(EditProfile));
                    }
                    else
                    {
                        HttpContext.Session.SetString("error", "Passwords Not Match");

                        return RedirectToAction(nameof(EditProfile));
                    }

                }
                else
                {
                    HttpContext.Session.SetString("error", "Wrong Current password");

                    return RedirectToAction(nameof(EditProfile));
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            

        }
        [HttpPost]
        public async Task<IActionResult> EditEmail(Users users, string newemail)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                var user = await db.Users.FindAsync(users.UserId);

                user.Email = newemail;
                db.Users.Update(user);
                await db.SaveChangesAsync();
                HttpContext.Session.SetString("Email", "Data Changed");

                return RedirectToAction(nameof(EditProfile));
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }


        }
        public IActionResult MyAds()
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                ViewBag.MyAds = db.Product.Where(m => m.UserId == uid).Count();
                ViewBag.Facourite = db.FavoriteProduct.Where(m => m.UserId == uid).Count();
                MyAdsVM = new MyAdsViewModel()
                {
                    Products = db.Product.Where(m => m.UserId == uid).Include(p => p.Category).Include(p => p.User).ToList(),
                    Users = db.Users.Find(uid),
                    Categories = db.Category.ToList(),
                    Product = new Product(),
                };
                return View(MyAdsVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }

        }
        public IActionResult CreateAds()
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                ViewBag.MyAds = db.Product.Where(m => m.UserId == uid).Count();
                ViewBag.Facourite = db.FavoriteProduct.Where(m => m.UserId == uid).Count();
                MyAdsVM = new MyAdsViewModel()
                {
                    Products = db.Product.Where(m => m.UserId == uid).Include(p => p.Category).Include(p => p.User).ToList(),
                    Users = db.Users.Find(uid),
                    Categories = db.Category.ToList(),
                    Product = new Product(),
                };
                ViewData["CategoryId"] = new SelectList(db.Category, "CategoryId", "Category1");
                return View(MyAdsVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAds(Product product)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string ImagePath = null;

                if (files.Count > 0)
                {
                    string webRootPath = webHostEnvironment.WebRootPath;

                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);

                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImageName), FileMode.Create);

                    files[0].CopyTo(fileStream);

                    ImagePath = @"\Images\" + ImageName;
                }

                product.Photo = ImagePath;
                product.UserId = uid;
                db.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(MyAds));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "CategoryId", "Category1", product.CategoryId);
            return View(product);
        }


        public async Task<IActionResult> EditAds(int? id)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                ViewBag.MyAds = db.Product.Where(m => m.UserId == uid).Count();
                ViewBag.Facourite = db.FavoriteProduct.Where(m => m.UserId == uid).Count();
                if (id == null)
                {
                    return NotFound();
                }
                MyAdsVM = new MyAdsViewModel()
                {
                    Products = db.Product.Where(m => m.UserId == uid).Include(p => p.Category).Include(p => p.User).ToList(),
                    Users = db.Users.Find(uid),
                    Categories = db.Category.ToList(),
                    Product = await db.Product.AsNoTracking().Where(m => m.ProductId == id).FirstOrDefaultAsync(),
                };

                if (MyAdsVM.Product == null)
                {
                    return NotFound();
                }
                ViewData["CategoryId"] = new SelectList(db.Category, "CategoryId", "Category1", MyAdsVM.Product.CategoryId);
                return View(MyAdsVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAds(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    string ImagePath = product.Photo;

                    if (files.Count > 0)
                    {
                        string webRootPath = webHostEnvironment.WebRootPath;

                        string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);

                        FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImageName), FileMode.Create);

                        files[0].CopyTo(fileStream);

                        ImagePath = @"\Images\" + ImageName;
                    }

                    product.Photo = ImagePath;

                    var Newproduct = db.Product.First(g => g.ProductId == product.ProductId);
                    db.Entry(Newproduct).CurrentValues.SetValues(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyAds));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "CategoryId", "Category1", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> DeleteAds(int? id)
        {
             var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                ViewBag.MyAds = db.Product.Where(m => m.UserId == uid).Count();
                ViewBag.Facourite = db.FavoriteProduct.Where(m => m.UserId == uid).Count();
                if (id == null)
                {
                    return NotFound();
                }
                MyAdsVM = new MyAdsViewModel()
                {
                    Products = db.Product.Where(m => m.UserId == uid).Include(p => p.Category).Include(p => p.User).ToList(),
                    Users = db.Users.Find(uid),
                    Categories = db.Category.ToList(),
                    Product = await db.Product.AsNoTracking().Where(m => m.ProductId == id).FirstOrDefaultAsync(),
                };

                if (MyAdsVM.Product == null)
                {
                    return NotFound();
                }

                return View(MyAdsVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAds(int id)
        {
            var product = await db.Product.FindAsync(id);
            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(MyAds));
        }

        private bool ProductExists(int id)
        {
            return db.Product.Any(e => e.ProductId == id);
        }
        public IActionResult Checkout()
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                CheckoutVM = new CheckoutViewModel()
                {
                    CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                    Users = db.Users.Find(uid),
                    Payment = new Payment(),
                };
                var discount = HttpContext.Session.GetInt32("Discount");

                ViewBag.TotalDiscount = HttpContext.Session.GetInt32("TotalDiscount");

                if (discount == null)
                {
                    ViewBag.Total = db.CartItem.Where(m => m.UserId == uid).Sum(m => m.CartPrice);
                }
                else
                {
                    ViewBag.Total = db.CartItem.Where(m => m.UserId == uid).Sum(m => m.CartPrice);

                    ViewBag.TotalAfterDiscount = discount;
                }

                return View(CheckoutVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
        }
        [HttpPost]
        public IActionResult Checkout(PaymentDetails paymentDetails,string PaymnetType)
        {
            var uid = HttpContext.Session.GetInt32(key);

            CheckoutVM = new CheckoutViewModel()
            {
                CartItems = db.CartItem.Where(m => m.UserId == uid).ToList(),
                Users = db.Users.Find(uid),
                Payment = new Payment(),
            };
            var payment = db.PaymentDetails.Where(m => m.CardCode == paymentDetails.CardCode).FirstOrDefault();

            var totalCart = db.CartItem.Where(m => m.UserId == uid).Sum(m => m.CartPrice);

            var Cart = db.CartItem.Where(m => m.UserId == uid).ToList();

            var Product = db.CartItem.Include(m=>m.Product).Where(m => m.UserId == uid).ToList();

            var not = db.Product.Include(m => m.CartItem).Where(m=>m.UserId== uid).ToList();

            var discount = HttpContext.Session.GetInt32("Discount");


            if (ModelState.IsValid)
            {
                if (payment.Balance < totalCart)
                {
                    ModelState.AddModelError("Wrong", "There is not enough balance on your card");
                    return View(CheckoutVM);
                }
                else
                {
                    if (Cart == null)
                    {
                        ModelState.AddModelError("Wrong", "There are no items to buy");
                        return View(CheckoutVM);
                    }
                    else
                    {
                        if (discount == null)
                        {
                            Payment userPayment = new Payment();
                            userPayment.PaymentType = PaymnetType;
                            userPayment.Amount = totalCart;
                            userPayment.PaymentDate = DateTime.Now;
                            userPayment.UserId = uid;
                            userPayment.PaymentMonth = DateTime.Now.Month.ToString();
                            userPayment.PaymentYear = DateTime.Now.Year.ToString();
                            userPayment.PaymentDetailsId = payment.Id;
                            db.Add(userPayment);
                            foreach (var item in Product)
                            {
                                Notification notification = new Notification();
                                notification.ProductId = item.ProductId;
                                notification.UserId = item.Product.UserId;
                                db.Add(notification);
                                var changeId = db.Product.Find(item.ProductId);
                                changeId.UserId = uid;

                            }
                            foreach (var item in Cart)
                            {
                                var RemoveCart = db.CartItem.Find(item.Id);
                                db.Remove(RemoveCart);
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            Payment userPayment = new Payment();
                            userPayment.PaymentType = PaymnetType;
                            userPayment.Amount = discount;
                            userPayment.PaymentDate = DateTime.Now;
                            userPayment.UserId = uid;
                            userPayment.PaymentMonth = DateTime.Now.Month.ToString();
                            userPayment.PaymentYear = DateTime.Now.Year.ToString();
                            userPayment.PaymentDetailsId = payment.Id;
                            db.Add(userPayment);
                            foreach (var item in Product)
                            {
                                Notification notification = new Notification();
                                notification.ProductId = item.ProductId;
                                notification.UserId = item.Product.UserId;
                                db.Add(notification);
                                var changeId = db.Product.Find(item.ProductId);
                                changeId.UserId = uid;

                            }
                            foreach (var item in Cart)
                            {
                                var RemoveCart = db.CartItem.Find(item.Id);
                                db.Remove(RemoveCart);
                            }
                            db.SaveChanges();
                        }
                    }

                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult applyCoupon(string Name)
        {
            var uid = HttpContext.Session.GetInt32(key);

            var Coupon = db.Coupon.Where(m => m.Name == Name).FirstOrDefault();
            var total = db.CartItem.Where(m => m.UserId == uid).Sum(m => m.CartPrice);
            int d = ((total * Coupon.Discount) / 100);
            HttpContext.Session.SetInt32("TotalDiscount", d);
            int discount =(total - d);
            HttpContext.Session.SetInt32("Discount", discount);
            return RedirectToAction(nameof(Checkout));
        }
        public IActionResult Favorite()
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                FavoriteVM = new FavoriteViewModel()
                {
                    favoriteProducts = db.FavoriteProduct.Where(m => m.UserId == uid).Include(p => p.User).Include(n => n.Product).ToList(),
                    Users = db.Users.Find(uid),
                };
                ViewBag.MyAds = db.Product.Where(m => m.UserId == uid).Count();
                ViewBag.Facourite = db.FavoriteProduct.Where(m => m.UserId == uid).Count();
                return View(FavoriteVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
           
        }
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var FavoriteProduct = await db.FavoriteProduct.FindAsync(id);
            db.FavoriteProduct.Remove(FavoriteProduct);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Favorite));
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var User = await db.Users.FindAsync(id);
            db.Users.Remove(User);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public JsonResult Rate(int PrId, int UsId ,int rate)
        {
            var userrate = db.Rate.Where(m => m.ProductId == PrId && m.UserId == UsId).FirstOrDefault();
            ViewBag.x = userrate;

            if (userrate==null)
            {
                Rate newRate = new Rate();
                newRate.ProductId = PrId;
                newRate.UserId = UsId;
                newRate.Rating = rate;
                db.Rate.Add(newRate);
                db.SaveChanges();
            }
            return Json(userrate);
        }
        public async Task<IActionResult> PublicMessage()
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                IndexVM = new IndexViewModel()
                {
                    Products = await db.Product.Include(p => p.Category).Include(m => m.Rate).Include(p => p.User).Include(p => p.FavoriteProduct).ToListAsync(),
                    Categorys = await db.Category.ToListAsync(),
                    Users = await db.Users.FindAsync(uid),
                    CartItems = await db.CartItem.Where(m => m.UserId == uid).ToListAsync(),
                    FavoriteProducts = await db.FavoriteProduct.ToListAsync(),
                    Rates = await db.Rate.ToListAsync(),
                    UsersList = await db.Users.ToListAsync(),
                    Notifications = await db.Notification.Where(m => m.UserId == uid).ToListAsync(),
                };

                IndexVM.Users.Status = true;
                db.Users.Update(IndexVM.Users);
                await db.SaveChangesAsync();
                return View(IndexVM);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            
        }
        public IActionResult RemoveFromNotf(int id)
        {
            var notf = db.Notification.Find(id);
            db.Notification.Remove(notf);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Headers["Referer"].ToString());
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public IActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginurl = fb.GetLoginUrl(new
            {
                client_id = appid,
                client_secret = appsecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginurl.AbsoluteUri);
        }

        public IActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = appid,
                client_secret = appsecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accesstoken = result.access_token;
            fb.AccessToken = accesstoken;
            dynamic data = fb.Get("me?fields=link,first_name,last_name,email,picture");
            Users user = new Users();
            user.Email = data.email;
            user.UsarName = data.first_name + " " + data.last_name;
            user.UserPhoto = data.picture.data.url;
            user.RoleId = 4;
            user.CreatedAt = Convert.ToDateTime(DateTime.Now.ToString("MM-dd-yyyy"));
            db.Add(user);
            db.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.UserId);

            return RedirectToAction("CreatePassword");
        }
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return Json(claims);
        }
        public IActionResult CreatePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreatePassword(string NewPassword,string ConfirmPassword)
        {
            var uid = HttpContext.Session.GetInt32(key);

            if (uid != null)
            {
                if (NewPassword == ConfirmPassword)
                {
                    var id = uid;

                    var userInfo = db.Users.AsNoTracking().Where(x => x.UserId == id).ToList().FirstOrDefault();
                    userInfo.Password = NewPassword;
                    db.Users.Update(userInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Wrong", "Passwords Not Match");
                    return View("CreatePassword");
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
           

        }
    }
}
