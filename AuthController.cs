using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.Models;
using FoodOrderingAPI.Services;
using FoodOrderingAPI.Data;
using System.Linq;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // SEND OTP
        [HttpPost("send-otp")]
        public IActionResult SendOtp(User user)
        {
            if (string.IsNullOrEmpty(user.Mobile))
                return BadRequest(new { message = "Mobile required" });

            var otp = new Random().Next(1000, 9999).ToString();

            // Store OTP
            OtpService.otpStore[user.Mobile] = otp;

            // Save user if not exists
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Mobile == user.Mobile);

            if (existingUser == null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            // RETURN OTP (for demo)
            return Ok(new
            {
                message = "OTP generated",
                otp = otp
            });
        }

        // VERIFY OTP
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpRequest request)
        {
            if (string.IsNullOrEmpty(request.Mobile) || string.IsNullOrEmpty(request.Otp))
                return BadRequest(new { message = "Invalid request" });

            if (OtpService.otpStore.ContainsKey(request.Mobile) &&
                OtpService.otpStore[request.Mobile].ToString() == request.Otp.ToString())
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Mobile == request.Mobile);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Remove OTP after success
                OtpService.otpStore.Remove(request.Mobile);

                return Ok(new
                {
                    message = "Login success",
                    name = user.Name
                });
            }

            return Unauthorized(new { message = "Invalid OTP" });
        }
    }

    public class OtpRequest
    {
        public string Mobile { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}