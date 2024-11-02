
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
        private readonly IConfiguration _configuration;
        public AuthController(CoffeeShopDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            // Retrieve the RoleId for "User" from UserRoles table
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (userRole == null)
            {
                ModelState.AddModelError(string.Empty, "Vai trò người dùng không tồn tại.");
                return View(user);
            }

            // Hash the password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.RoleId = userRole.RoleId; // Assign the RoleId for "User"

            // Save the user
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

        //POST: Auth/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(User loginUser)
        {
            // Kiểm tra xem người dùng có tồn tại không
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            // Kiểm tra xem người dùng có tồn tại và xác thực mật khẩu
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.PasswordHash, user.PasswordHash))
            {
                ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không hợp lệ.");
                return View(loginUser); 
            }

            // Lấy vai trò người dùng dựa trên RoleId
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleId == user.RoleId);
            if (userRole == null)
            {
                ModelState.AddModelError("", "Vai trò người dùng không hợp lệ.");
                return View(loginUser); 
            }

            // Tạo JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Lấy khóa từ cấu hình

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userRole.RoleName) // Sử dụng RoleName từ UserRoles
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["Jwt:Audience"], 
                Issuer = _configuration["Jwt:Issuer"]   
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Thiết lập JWT token trong cookie
            Response.Cookies.Append("jwt", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Chỉ hoạt động qua HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            ViewBag.Username = user.Username;

            if (userRole.RoleName == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xóa cookie JWT
            Response.Cookies.Delete("jwt");

            // Chuyển hướng đến trang chủ hoặc trang đăng nhập
            return RedirectToAction("Index", "Home"); // Hoặc RedirectToAction("Login", "Auth");
        }

    }
}
