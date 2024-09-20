using Apartments.Models;
using Apartments.Services;
using ApartmentsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace Apartments.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase{
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        private readonly ListingService _listingService;

        public AuthController(UserService userService, JwtService jwtService, PasswordService passwordService, ListingService listingService){
            _userService = userService;
            _jwtService = jwtService;
            _passwordService = passwordService;
            _listingService = listingService;

        }
        [HttpPost]
        public async Task<IActionResult> Post(UserLogin user){
            var userFound = await  _userService.GetAsync(user.email);

            if(userFound is null){
                return NotFound();
            }
            
            bool confirmed = _passwordService.VerifyPassword(user.password, userFound.password);
    

            if(confirmed == false){
                return BadRequest(new {title = "Password is incorrect"});
            }
            
            var token = _jwtService.GenerateToken(userFound);
            HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions{
                Expires = DateTime.Now.AddDays(90)
            });
            var tasks = userFound.listings.Select(x => _listingService.GetAsync(x)).ToList();
            var results = await Task.WhenAll(tasks);

            var listings = results.Where(x => x != null).ToList();
        
            return Ok(new {Status =  "success", name= userFound.name, listings = listings });

        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> checkIfVerified(){
            
            var email = Request.Headers["email"];
            var user = await _userService.GetAsync(email);
            return Ok(new {Status = "success", user});
        }
    }
}