using Microsoft.AspNetCore.Mvc;
using btl_web_coffeeshop.Models;
using System.Threading.Tasks;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public AuthController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        // GET: Auth/Register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Tên người dùng đã được sử dụng.");
                return View(user);
            }

            // Hash mật khẩu
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            // Lưu người dùng
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // GET: Auth/Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(User loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.PasswordHash, user.PasswordHash))
            {
                ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không hợp lệ.");
                return View(loginUser);
            }

            // Tạo JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("@huy27112004"); // Đưa cái này vào appsettings.json
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Đặt token JWT trong cookie
            Response.Cookies.Append("jwt", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ hoặc một trang khác sau khi đăng nhập thành công
        }
    }
}
