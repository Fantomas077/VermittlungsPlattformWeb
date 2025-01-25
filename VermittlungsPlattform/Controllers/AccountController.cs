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
        /// <summary>
        /// Register unternehmen
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterUnternehmen()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        
        public IActionResult RegisterUnternehmen(User user)
        {
            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Email = user.Email?.Trim();
            user.Password = user.Password?.Trim();
            user.Name = user.Name?.Trim();
            user.Vorname = user.Vorname?.Trim();
            user.RecoveryCode = 0;
            user.IsStudent = false;
            user.IsCompany = true;
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
                ModelState.AddModelError("Email", "Email ist nicht valid");
                return View(user);
            }
            //-----------Duplictae Email Checking-------------
            var prevUser = _context.Users.Any(x => x.Email == user.Email);
            if (prevUser)
            {
                ModelState.AddModelError("Email", "Email wurde schon registriert");
                return View(user);
            }
            //------------------------------------------------
            _context.Users.Add(user);
            _context.SaveChanges();
            //------------------------
            return RedirectToAction("Login");
        }
        

        //------------Student---------
        public IActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterStudent(User user)
        {
            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Email = user.Email?.Trim();
            user.Password = user.Password?.Trim();
            user.Name = user.Name?.Trim();
            user.Vorname = user.Vorname?.Trim();
            user.RecoveryCode = 0;
            user.IsCompany = false;
            user.IsStudent=true;
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
                ModelState.AddModelError("Email", "Email ist nicht valid");
                return View(user);
            }
            //-----------Duplictae Email Checking-------------
            var prevUser = _context.Users.Any(x => x.Email == user.Email);
            if (prevUser)
            {
                ModelState.AddModelError("Email", "Email wurde schon registriert");
                return View(user);
            }
            //------------------------------------------------
            _context.Users.Add(user);
            _context.SaveChanges();
            //------------------------
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            //------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------
            var foundUser = _context.Users.FirstOrDefault(x => x.Email == user.Email.Trim() && x.Password == user.Password.Trim());
            //-----
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email oder Password  existiert nicht");
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
            else if(foundUser.IsStudent==true)
            {
                claims.Add(new Claim(ClaimTypes.Role, "student"));
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
            if (foundUser.IsAdmin)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if(foundUser.IsStudent)
            {
                var studentProfile = _context.StudentProfiles.FirstOrDefault(x => x.UserId == foundUser.Id);
                if (studentProfile != null)
                {
                    return Redirect("/Home/");
                }
                else
                {
                   
                    return RedirectToAction("Create", "StudentProfiles", new { area = "Student" });
                }
                
            }
            else
            {
               
                var unternehmenProfile = _context.UnternehmenProfiles.FirstOrDefault(x => x.UserId == foundUser.Id);

                if (unternehmenProfile != null)
                {
                    
                    return RedirectToAction("Index", "Home", new { area = "Unternehmen" });
                }
                else
                {
                  
                    return RedirectToAction("Create", "UnternehmenProfiles", new { area = "Unternehmen" });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult RecoveryPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecoveryPassword(RecoveryPasswordViewModel recoveryPassword)
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

            var foundUser = _context.Users.FirstOrDefault(x => x.Email == recoveryPassword.Email.Trim());
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            foundUser.RecoveryCode = new Random().Next(10000, 100000);
            _context.Users.Update(foundUser);
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
            return Redirect("/Account/ResetPasswordView?email=" + foundUser.Email);
        }


        public IActionResult ResetPasswordView(string email)
        {
            var resetPasswordModel = new ResetPasswordViewModel();
            resetPasswordModel.Email = email;

            return View(resetPasswordModel);
        }

        [HttpPost]
        public IActionResult ResetPasswordView(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.Users.FirstOrDefault(x => x.Email == resetPassword.Email && x.RecoveryCode == resetPassword.RecoveryCode);
            if (foundUser == null)
            {
                ModelState.AddModelError("RecoveryCode", " recovery code is not valid");
                return View(resetPassword);
            }

            ////-------------------------------------------

            foundUser.Password = resetPassword.NewPassword;

            _context.Users.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            return RedirectToAction("Login");
        }
    }
    

    
}
