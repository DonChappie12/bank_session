using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bank_session.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace bank_session.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
        public HomeController(UserContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(ValidateUser user)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<ValidateUser> Hasher = new PasswordHasher<ValidateUser>();
                user.Password = Hasher.HashPassword(user, user.Password);
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("user_id",newUser.UserId);
                return RedirectToAction("Money", newUser.UserId);
            }
            else
            {
                return View("Index");
            }
        }

        [Route("money")]
        public IActionResult Money(int Id)
        {
            User currUser = _context.user.Include(u => u.Transactions).SingleOrDefault(c=>c.UserId == HttpContext.Session.GetInt32("user_id"));
            return View("Money", currUser);
        }

        [HttpPost("login")]
        public IActionResult LoginIn(string Email, string Password)
        {
            var user = _context.user.Where(u=> u.Email == Email).FirstOrDefault();
            if(user != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, Password))
                {
                    HttpContext.Session.SetInt32("user_id", user.UserId);
                    return RedirectToAction("Money", user.UserId);
                }
            }
            ViewBag.error="Email and/or Password dont match";
            return View("Login");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost("transaction")]
        public IActionResult Transaction(int Amount)
        {
            int Id = (int)HttpContext.Session.GetInt32("user_id");
            User user = _context.user.Include(u => u.Transactions).SingleOrDefault(x=>x.UserId == Id);
            if(user.Balance + Amount<0)
            {
                ViewBag.broke="You Broke Homie";
            }
            else{
                Transaction newTrans = new Transaction();
                newTrans.Amount=Amount;
                newTrans.CreatedAt=DateTime.Now;
                newTrans.User_Id= (int)HttpContext.Session.GetInt32("user_id");
                _context.transactions.Add(newTrans);
                user.Balance += Amount;
                _context.SaveChanges();
            }
            return View("Money", user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
