using BelanjaYukAPI.Data;
using BelanjaYukAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BelanjaYukAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest(new { Message = "Request tidak valid" });
            }

            var user = await _context.MsUsers
                .FirstOrDefaultAsync(u => u.Email == loginRequest.EmailOrPhone || u.PhoneNumber == loginRequest.EmailOrPhone);

            if (user == null)
            {
                return Unauthorized(new { Message = "Email atau nomor HP tidak terdaftar." });
            }

            var userPassword = await _context.MsUserPasswords
                .FirstOrDefaultAsync(p => p.IdUser == user.IdUser);

            if (userPassword == null)
            {
                return Unauthorized(new { Message = "Data password tidak ditemukan untuk pengguna ini." });
            }

            if (loginRequest.Password != userPassword.PasswordHashed)
            {
                return Unauthorized(new { Message = "Kata sandi salah." });
            }

            // 4. Buat token dummy
            string token = "dummy-jwt-token-for-" + user.UserName;

            var response = new LoginResponse
            {
                UserId = user.IdUser,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new { Message = "Data yang dikirim tidak lengkap atau tidak valid." });
            }

            if (await _context.MsUsers.AnyAsync(u => u.UserName == request.UserName))
            {
                return BadRequest(new { Message = "Username sudah terpakai." });
            }

            if (await _context.MsUsers.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { Message = "Email sudah terdaftar." });
            }

            if (await _context.MsUsers.AnyAsync(u => u.PhoneNumber == request.PhoneNumber))
            {
                return BadRequest(new { Message = "Nomor HP sudah terdaftar." });
            }

            var newUser = new MsUser
            {
                IdUser = "USR" + (await _context.MsUsers.CountAsync() + 1).ToString("D3"),
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob,
                IdGender = request.IdGender,
                IsActive = true,
                DateIn = DateTime.Now,
                UserIn = "SYSTEM"
            };

            var newPassword = new MsUserPassword
            {
                IdUserPassword = "PASS" + (await _context.MsUserPasswords.CountAsync() + 1).ToString("D3"),
                IdUser = newUser.IdUser,
                PasswordHashed = request.Password,
                IsActive = true,
                DateIn = DateTime.Now,
                UserIn = "SYSTEM"
            };

            try
            {
                _context.MsUsers.Add(newUser);
                _context.MsUserPasswords.Add(newPassword);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Terjadi error internal saat menyimpan data: " + ex.Message });
            }

            return Ok(new { Message = "Registrasi berhasil." });
        }
    }
}

