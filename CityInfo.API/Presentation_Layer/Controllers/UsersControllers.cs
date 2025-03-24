using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.BusinessLayer.Repos;
using ToDoListInfo.API.Data_AccessLayer.Entities;
using ToDoListInfo.API.Data_AccessLayer.Services;


namespace ToDoList.API.Presentation_Layer.Controllers
{
    //"DbConnectionString": "Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;UserId=sa;Password=BT.Cj#9628517;TrustedConnection=True;",

    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/users")]
    //[Route("api/[controller]")]
    [Asp.Versioning.ApiVersion(3)]


    public class UsersControllers: ControllerBase
    {

        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ListsControllers> _logger;
        //private readonly string _jwt = "mare_secret_mare";



        public UsersControllers(IUserRepo userRepo, IMapper mapper, ILogger<ListsControllers> logger)
        {
            _userRepo = userRepo ??
                throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var usersEntities = await _userRepo.GetUsersAsync();

            return Ok(_mapper.Map<IEnumerable<UserDTO>>(usersEntities));

        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> InsertUser(UserInsertDTO user)
        {
            if (user == null)
            {
                return NotFound();
            }

            var existsEmail = await _userRepo.GetUserAsync(user.Email);

            if (existsEmail != null)
            {
                return Conflict(new { message = "Email is already registered!" }); 

            }

            _userRepo.AddUser(user);

            await _userRepo.SaveChangesAsync();

            return Ok(new { message = "Registration successful!" });

        }


        [HttpGet("{emailUser}")]
        public async Task<ActionResult<UserDTO>> GetUser(
            string emailUser)
        {

            var user = await _userRepo.GetUserAsync(emailUser);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDTO>(user));

        }

        //[HttpGet("{emailUser}", Name = "Exist")]
        //public async Task<ActionResult> UserExist(string emailUser)
        //{
        //    var user = await _userRepo.UserExistsAsync(emailUser);

        //    if (user == false)
        //    {
        //        return NotFound();
        //    }

        //    return Ok();
        //}

        [HttpGet("isAdmin/{emailUser}", Name = "Exist")]
        public async Task<ActionResult> IsAdmin(string emailUser)
        {
            var user = await _userRepo.IsAdminAsync(emailUser);

            if (user == false)
            {
                return Ok(new {flag = false, message = "User is not an admin!" });
            }

            return Ok(new {flag = true, message = "User is an admin!" });
        }


        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login(LoginDTO user)
        {
            if (user == null)
            {
                return NotFound();
            }

            var existsUser = await _userRepo.GetUserAsync(user.Email);

            if (existsUser == null)
            {
                return NotFound(new { message = "Email is not registered!" });

            }

            bool verification = BCrypt.Net.BCrypt.EnhancedVerify(user.Pass, existsUser.Pass);

            //var token = GenerateJwtToken(user.Email);


            if (verification)
            {
                return Ok(new { message = "Login successful!" });
                //return Ok(new { token });

            }

            return Unauthorized(new { message = "Password is incorrect!" });


        }


    }
}