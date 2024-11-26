using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using VermittlungsPlattform.Models.Db;
using VermittlungsPlattform.ViewModels;
using System.Net.Mail;

namespace VermittlungsPlattform.Controllers
{
    public class AccountController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;
        public AccountController( VermittlungsplattformDbContext context)
        {
            _context = context;
            
        }
        public IActionResult RegisterUnternehmen()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterUnternehmen(UserUnternehmen user)
        {
            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Email = user.Email?.Trim();
            user.Password = user.Password?.Trim();
            user.Name = user.Name?.Trim();
            user.Vorname = user.Vorname?.Trim();
            user.RecoveryCode = 0;
            //------------------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------Valid Email Checking------------
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(user);
            }
            //-----------Duplictae Email Checking-------------
            var prevUser = _context.UserUnternehmen.Any(x => x.Email == user.Email);
            if (prevUser)
            {
                ModelState.AddModelError("Email", "Email is used");
                return View(user);
            }
            //------------------------------------------------
            _context.UserUnternehmen.Add(user);
            _context.SaveChanges();
            //------------------------
            return RedirectToAction("LoginUnternehmen");
        }
        public IActionResult LoginUnternehmen()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginUnternehmen(LoginViewModel user)
        {
            //------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------
            var foundUser = _context.UserUnternehmen.FirstOrDefault(x => x.Email == user.Email.Trim() && x.Password == user.Password.Trim());
            //-----
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email or Password  not exist");
                return View(user);
            }
            //------------
            // Create claims for the authenticated user
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, foundUser.Name));
            claims.Add(new Claim(ClaimTypes.Email, foundUser.Email));
            //------------
            if (foundUser.IsAdmin == true)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "unternehmen"));
            }
            //------------
            // Create an identity based on the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //------------
            // Create a principal based on the identity
            var principal = new ClaimsPrincipal(identity);
            //------------
            // Sign in the user with the created principal
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //------------
            return Redirect("/Unternehmen/");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginUnternehmen", "Account");
        }


        //------------Student---------
        public IActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterStudent(UserStudent user)
        {
            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Email = user.Email?.Trim();
            user.Password = user.Password?.Trim();
            user.Name = user.Name?.Trim();
            user.Vorname = user.Vorname?.Trim();
            user.RecoveryCode = 0;
            //------------------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------Valid Email Checking------------
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(user);
            }
            //-----------Duplictae Email Checking-------------
            var prevUser = _context.UserStudents.Any(x => x.Email == user.Email);
            if (prevUser)
            {
                ModelState.AddModelError("Email", "Email is used");
                return View(user);
            }
            //------------------------------------------------
            _context.UserStudents.Add(user);
            _context.SaveChanges();
            //------------------------
            return RedirectToAction("LoginStudent");
        }

        public IActionResult LoginStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginStudent(LoginViewModel user)
        {
            //------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------
            var foundUser = _context.UserStudents.FirstOrDefault(x => x.Email == user.Email.Trim() && x.Password == user.Password.Trim());
            //-----
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email or Password  not exist");
                return View(user);
            }
            //------------
            // Create claims for the authenticated user
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, foundUser.Name));
            claims.Add(new Claim(ClaimTypes.Email, foundUser.Email));
            //------------
            if (foundUser.IsAdmin == true)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "student"));
            }
            //------------
            // Create an identity based on the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //------------
            // Create a principal based on the identity
            var principal = new ClaimsPrincipal(identity);
            //------------
            // Sign in the user with the created principal
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //------------
            if (foundUser.IsAdmin == true)
            {
                return Redirect("/Admin/");
            }
            else
            {
                return Redirect("/Student/");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult LogoutStudent()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginStudent", "Account");
        }

        [HttpGet]
        public IActionResult RecoveryPasswordStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecoveryPasswordStudent(RecoveryPasswordViewModel recoveryPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ////-------------------------------------------

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(recoveryPassword.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.UserStudents.FirstOrDefault(x => x.Email == recoveryPassword.Email.Trim());
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            foundUser.RecoveryCode = new Random().Next(10000, 100000);
            _context.UserStudents.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("nheltonn@gmail.com");
            mail.To.Add(foundUser.Email);
            mail.Subject = "Recovery code";
            mail.Body = $"Hello {foundUser.Name}\nYour  recovery code:" + foundUser.RecoveryCode;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("nheltonn@gmail.com", "ugju meqj olao siko");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            ////-------------------------------------------
            return Redirect("/Account/ResetPasswordViewStudent?email=" + foundUser.Email);
        }


        public IActionResult ResetPasswordViewStudent(string email)
        {
            var resetPasswordModel = new ResetPasswordViewModel();
            resetPasswordModel.Email = email;

            return View(resetPasswordModel);
        }

        [HttpPost]
        public IActionResult ResetPasswordViewStudent(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.UserStudents.FirstOrDefault(x => x.Email == resetPassword.Email && x.RecoveryCode == resetPassword.RecoveryCode);
            if (foundUser == null)
            {
                ModelState.AddModelError("RecoveryCode", " recovery code is not valid");
                return View(resetPassword);
            }

            ////-------------------------------------------

            foundUser.Password = resetPassword.NewPassword;

            _context.UserStudents.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            return RedirectToAction("LoginStudent");
        }
        [HttpGet]
        public IActionResult RecoveryPasswordUnternehmen()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecoveryPasswordUnternehmen(RecoveryPasswordViewModel recoveryPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ////-------------------------------------------

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(recoveryPassword.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.UserUnternehmen.FirstOrDefault(x => x.Email == recoveryPassword.Email.Trim());
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            foundUser.RecoveryCode = new Random().Next(10000, 100000);
            _context.UserUnternehmen.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("nheltonn@gmail.com");
            mail.To.Add(foundUser.Email);
            mail.Subject = "Recovery code";
            mail.Body = $"Hello {foundUser.Name}\nYour  recovery code:" + foundUser.RecoveryCode;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("nheltonn@gmail.com", "ugju meqj olao siko");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            ////-------------------------------------------
            return Redirect("/Account/ResetPasswordViewUnternehmen?email=" + foundUser.Email);
        }


        public IActionResult ResetPasswordViewUnternehmen(string email)
        {
            var resetPasswordModel = new ResetPasswordViewModel();
            resetPasswordModel.Email = email;

            return View(resetPasswordModel);
        }

        [HttpPost]
        public IActionResult ResetPasswordViewUnternehmen(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.UserUnternehmen.FirstOrDefault(x => x.Email == resetPassword.Email && x.RecoveryCode == resetPassword.RecoveryCode);
            if (foundUser == null)
            {
                ModelState.AddModelError("RecoveryCode", " recovery code is not valid");
                return View(resetPassword);
            }

            ////-------------------------------------------

            foundUser.Password = resetPassword.NewPassword;

            _context.UserUnternehmen.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            return RedirectToAction("LoginUnternehmen");
        }
    }

    
}
