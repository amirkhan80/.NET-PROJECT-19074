using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartServiceHub.Models;
using SmartServiceHub.Services;
using SmartServiceHub.Utils;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartServiceHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ Register (GET)
        [HttpGet]
        public IActionResult Register() => View();

        // ✅ Register (POST)
        [HttpPost]
        public async Task<IActionResult> Register(string fullname, string email, string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
            {
                ModelState.AddModelError("", "Please fill all fields.");
                return View();
            }

            if (!Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w{2,4}$"))
            {
                ModelState.AddModelError("", "Invalid email format.");
                return View();
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                ModelState.AddModelError("", "Phone number must be 10 digits.");
                return View();
            }

            if (password.Length < 6)
            {
                ModelState.AddModelError("", "Password must be at least 6 characters.");
                return View();
            }

            var existing = await _userService.GetByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email already registered.");
                return View();
            }

            // ✅ Default Role = Customer
            var user = new User
            {
                FullName = fullname,
                Email = email.ToLower(),
                Phone = phone,
                PasswordHash = PasswordHasher.Hash(password),
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };

            await _userService.CreateAsync(user);
            await SignInUser(user);

            return RedirectToAction("Index", "Home");
        }

        // ✅ Login (GET)
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // ✅ Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Please enter both email and password.");
                return View();
            }

            var user = await _userService.GetByEmailAsync(email.ToLower());
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View();
            }

            bool validPassword = PasswordHasher.Verify(password, user.PasswordHash);
            if (!validPassword)
            {
                ModelState.AddModelError("", "Invalid password.");
                return View();
            }

            await SignInUser(user);

            // ✅ Admin → Admin Dashboard
            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            // ✅ Customer → Home OR returnUrl
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ✅ Correct Role-Based Sign-In Helper
        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),

                // ✅ Role Claim FIX
                new Claim(ClaimTypes.Role, user.Role ?? "Customer")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                }
            );
        }

        // ✅ Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ✅ Profile
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return RedirectToAction("Login");

            var user = await _userService.GetByIdAsync(id);
            return View(user);
        }

        // ✅ Access Denied page
        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
