using Agora.BLL.Infrastructure;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Agora.BLL.Services;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/account")]

    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISellerService _sellerService;
        private readonly IStoreService _storeService;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly ISecureService _secureService;
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IStatisticsInitializer _statisticsInitializer;

        public AccountController(IUserService userService, ISellerService sellerService, IStoreService storeService, IAddressService addressService,
            ICountryService countryService, ISecureService secureService, ICustomerService customerService, IConfiguration config, IEmailService emailService, IStatisticsInitializer statisticsInitializer)

        {
            _userService = userService;
            _sellerService = sellerService;
            _storeService = storeService;
            _addressService = addressService;
            _countryService = countryService;
            _secureService = secureService;
            _customerService = customerService;
            _config = config;
            _emailService = emailService;
            _statisticsInitializer = statisticsInitializer;            
        }

        [HttpPost("register-seller")]
        public async Task<IActionResult> RegisterSeller(RegSellerViewModel regSeller)
        {
            try
            {
                string hashedPassword = HashPassword(regSeller.Password);

                UserDTO userDTO = new UserDTO();
                userDTO.Name = regSeller.Name;
                userDTO.Surname = regSeller.Surname;
                userDTO.PhoneNumber = regSeller.PhoneNumber;
                userDTO.Password = hashedPassword;
                userDTO.Email = regSeller.Email;

                var userId = await _userService.CreateReturnId(userDTO);
                userDTO.Id = userId;

                SellerDTO sellerDTO = new SellerDTO();
                sellerDTO.UserId = userId;

                int sellerId = await _sellerService.Create(sellerDTO);

                await _statisticsInitializer.InitializeEmptyStatsForSeller(sellerId);

                StoreDTO storeDTO = new StoreDTO();
                storeDTO.Name = regSeller.StoreName;
                storeDTO.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                storeDTO.SellerId = sellerId;

                int storeId = await _storeService.Create(storeDTO);
                await _statisticsInitializer.InitializeEmptyStatsForStore(storeId);

                var country = await _countryService.Get(regSeller.CountryId);

                if (country == null) return BadRequest("Invalid country ID.");

                AddressDTO addressDTO = new AddressDTO();

                addressDTO.Appartement = regSeller.Appartement;
                addressDTO.Building = regSeller.Building;
                addressDTO.Street = regSeller.Street;
                addressDTO.City = regSeller.City;
                addressDTO.PostalCode = regSeller.PostalCode;
                addressDTO.CountryId = country.Id;
                addressDTO.UserId = userDTO.Id;                

                await _addressService.CreateAddress(addressDTO);

                var seller = await _sellerService.Get(sellerId);
                var role = await _userService.GetRoleByUserId(userDTO.Id);
                string jwtToken = _secureService.GenerateJwtToken(userDTO, role);
                Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                {
                    HttpOnly = true,
                    Secure = true, //  Если HTTPS то true
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });
                CreateSessions(userDTO.Id, role.Id, role.Role);

                return CreatedAtAction(nameof(RegisterSeller), new { id = seller.Id }, seller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            bool exists = await _userService.CheckEmailExists(email);
            return Ok(new { exists });
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegUserViewModel regUser)
        {
            var existingUser = await _userService.GetByCheckEmail(regUser.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            try
            {
                string hashedPassword = HashPassword(regUser.Password);

                UserDTO userDTO = new UserDTO
                {
                    Name = regUser.Name,
                    Surname = regUser.Surname,
                    PhoneNumber = regUser.PhoneNumber,
                    Password = hashedPassword,
                    Email = regUser.Email
                };

                var userId = await _userService.CreateReturnId(userDTO);

                // сначала  Customer, чтобы потом роль была найдена
                bool customerCreated = await _customerService.CreateForUser(userId);
                if (!customerCreated)
                {
                    return StatusCode(500, "Error occurred while creating customer record.");
                }

                var user = await _userService.Get(userId);
                var role = await _userService.GetRoleByUserId(user.Id);

                string jwtToken = _secureService.GenerateJwtToken(user, role);
                Response.Cookies.Append("jwt", jwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                CreateSessions(user.Id, role.Id, role.Role);

                return CreatedAtAction(nameof(RegisterUser), new { id = user.Id }, new { user, jwtToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using var hasher = new Argon2id(Encoding.UTF8.GetBytes(password));
            hasher.DegreeOfParallelism = 8;
            hasher.MemorySize = 65536;
            hasher.Iterations = 4;
            return Convert.ToBase64String(hasher.GetBytes(32));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginModel model)
        {
            UserDTO user = null;

            user = await _userService.GetByEmail(model.Email);

            if (user != null)
            {
                if (string.IsNullOrEmpty(user.GoogleId))
                {
                    return new JsonResult(new { message = "You already have an account, but it's not linked to Google." }) { StatusCode = 400 };
                }

                if (user.GoogleId == model.GoogleId)
                {
                    var role = await _userService.GetRoleByUserId(user.Id);
                    string jwtToken = _secureService.GenerateJwtToken(user, role);
                    Response.Cookies.Append("jwt", jwtToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });
                    CreateSessions(user.Id, role.Id, role.Role);
                    return Ok(new { jwtToken });
                }
                else
                {
                    return new JsonResult(new { message = "Google ID does not match." }) { StatusCode = 400 };
                }
            }
            else
            {
                var newUser = new UserDTO
                {
                    Name = model.Name,
                    Email = model.Email,
                    GoogleId = model.GoogleId
                };

                bool created = await _userService.CreateGoogle(newUser);
                if (!created)
                {
                    return StatusCode(500, new { message = "Error occurred during Google registration." });
                }

                user = await _userService.GetByEmail(model.Email);

                // Создаём запись в Customer
                bool customerCreated = await _customerService.CreateForUser(user.Id);
                if (!customerCreated)
                {
                    return StatusCode(500, new { message = "Error occurred while creating customer record." });
                }

                var role = await _userService.GetRoleByUserId(user.Id);

                string jwtToken = _secureService.GenerateJwtToken(user, role);
                Response.Cookies.Append("jwt", jwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                CreateSessions(user.Id, role.Id, role.Role);

                return Ok(new { jwtToken });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                UserDTO user = await _userService.GetByEmail(model.Email);
                if (user == null)
                    return new JsonResult(new { message = "You don't have account! Sign up!" }) { StatusCode = 401 };
                string hashedPass = HashPassword(model.Password);
                if (user.Email.Equals(model.Email) && user.Password.Equals(hashedPass))
                {
                    var role = await _userService.GetRoleByUserId(user.Id);
                    string jwtToken = _secureService.GenerateJwtToken(user, role);
                    Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                    {
                        HttpOnly = true,
                        Secure = true, //  Если HTTPS то true
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });

                    CreateSessions(user.Id, role.Id, role.Role);

                    return Ok(new { message = "Authenticated", userId = user.Id });
                }
                else
                {
                    return new JsonResult(new { message = "Wrong password or email!" }) { StatusCode = 403 };
                }

            }
            catch (ValidationExceptionFromService ex)
            {
                return new JsonResult(new { message = "You have to sign up first!" }) { StatusCode = 401 };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            }
        }

        [HttpGet("get-user-role")]
        public IActionResult GetUserRole()
        {
            // достаем токен из заголовка
            var token = HttpContext.Request.Headers["JWT"].FirstOrDefault();

            // если не нашли, то ищем в куках
            if (string.IsNullOrEmpty(token))
            {
                token = Request.Cookies["jwt"];
            }

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token provided.");
            }

            try
            {
                var roleDTO = _secureService.DecryptJwtToken(token);

                if (roleDTO == null || string.IsNullOrEmpty(roleDTO.Role))
                {
                    return Unauthorized("Invalid role or user.");
                }

                return Ok(new
                {
                    Role = roleDTO.Role,
                    UserId = roleDTO.UserId
                });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid token.");
            }
        }

        [NonAction]
        public void CreateSessions(int userId, int id, string role)
        {
            var encryptedUserId = _secureService.EncryptSessionInt(userId);
            var encryptedId = _secureService.EncryptSessionInt(id);
            var encryptedRole = _secureService.EncryptSessionString(role);
            Response.Cookies.Append("userId", encryptedUserId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, //  Если HTTPS то true
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Append("id", encryptedId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, //  Если HTTPS то true
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Append("role", encryptedRole, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, //  Если HTTPS то true
                SameSite = SameSiteMode.None
            });
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.Get(id);
                if (user == null)
                    return NotFound("User not found");

                var role = await _userService.GetRoleByUserId(user.Id);

                var profile = new UserProfileDTO
                {
                    Name = user.Name!,
                    Surname = user.Surname!,
                    Email = user.Email!,
                    Role = role.Role,
                    PhoneNumber = user.PhoneNumber!
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }  
        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // очищаем куки
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("userId");
            Response.Cookies.Delete("id");
            Response.Cookies.Delete("role");

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var user = await _userService.GetByCheckEmail(model.Email);

            if (user == null)
                return BadRequest("User not found");

            var token = await _userService.CreatePasswordResetTokenAsync(model.Email);

            var resetPasswordLink = $"http://localhost:3000/en/reset-password?token={token}&email={model.Email}";

            await _emailService.SendEmailAsync(model.Email, "Reset your password", $"Click here to reset your password: {resetPasswordLink}");

            return Ok(new { message = "Reset link sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Agora.Models.ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetByCheckEmail(model.Email);
            if (user == null)
                return BadRequest(new { message = "User not found" });

            var result = await _userService.ResetUserPasswordAsync(model.Email, model.Token, model.NewPassword);

            if (result)
                return Ok(new { message = "Password has been reset successfully." });

            return BadRequest(new { message = "Invalid token or reset failed." });
        }

        [HttpPost("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        [HttpPut("update-seller-email")]
        public async Task<IActionResult> UpdateSellerEmail([FromBody] UpdateSellerEmailDTO dto)
        {
            try
            {
                await _userService.UpdateSellerEmailAsync(dto.UserId, dto.NewEmail);
                return Ok(new { message = "Email updated successfully" });
            }
            catch (ValidationExceptionFromService ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("update-seller-phone")]
        public async Task<IActionResult> UpdateSellerPhoneNumber([FromBody] UpdateSellerPhoneNumberDTO dto)
        {
            try
            {
                await _userService.UpdateSellerPhoneNumberAsync(dto.UserId, dto.NewPhoneNumber);
                return Ok(new { message = "Phone number updated successfully" });
            }
            catch (ValidationExceptionFromService ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("update-seller-address")]
        public async Task<IActionResult> UpdateSellerAddress([FromBody] UpdateSellerAddressDTO dto)
        {
            try
            {
                await _addressService.UpdateSellerAddressAsync(dto);
                return Ok(new { message = "Address updated successfully" });
            }
            catch (ValidationExceptionFromService ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("seller-info/{sellerId}")]
        public async Task<IActionResult> GetSellerInfo(int sellerId)
        {
            try
            {
                var user = await _userService.GetById(sellerId);
                if (user == null)
                    return NotFound(new { message = "Seller not found" });

                return Ok(new
                {
                    email = user.Email,
                    phone = user.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
