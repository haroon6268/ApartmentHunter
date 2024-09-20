using Apartments.Models;
using Apartments.Services;
using ApartmentsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase{
        private readonly UserService _userService;
        private readonly PasswordService _passwordService;

        public UserController(UserService service, PasswordService passService){ 
            _userService = service;
            _passwordService = passService;
        }

        [HttpGet]
        public async Task<List<User>> Get(){
            return await _userService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id){
           var user = await _userService.GetAsync(id);
           if(user is null){
            return NotFound();
           }
           return user;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserCreation newUser){
           
        if(newUser.confirmPassword != newUser.password){
            return BadRequest(new {status="fail", message ="Passwords Do Not Match"});
        }
        var doesExist = await _userService.GetAsync(newUser.email);
        if(doesExist != null){
            return BadRequest(new {status="fail", message ="Email Already Exists"});
        }
        User user = new User{
            email = newUser.email,
            password = _passwordService.HashPassword(newUser.password),
            name = newUser.name
        };
         await _userService.CreateAsyc(user);
         return Ok(new {Status = "success", User = user});   
        }
    }

}