using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Repos;

namespace ToDoList.API.Presentation_Layer.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    [Asp.Versioning.ApiVersion(1)]


    public class UsersControllers : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ListsControllers> _logger;
       
        public UsersControllers(IUserRepo userRepo, IMapper mapper, ILogger<ListsControllers> logger)
        {
            _userRepo = userRepo ??
                throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        // se obtin userii
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var usersEntities = await _userRepo.GetUsersAsync();

            return Ok(_mapper.Map<IEnumerable<UserDTO>>(usersEntities));

        }

        // se adauga un user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> InsertUser(UserInsertDTO user)
        {
            try {
                if (user == null)
                {
                    return NotFound();
                }

                var existsEmail = await _userRepo.GetUserAsync(user.Email);

                if (existsEmail != null)
                {
                    return Conflict(new { message = "Email is already registered!" });

                }

                var userToAdd = _mapper.Map<ToDoListInfo.API.DBLayer.Entities.Users>(user);


                _userRepo.AddUser(userToAdd);

                return Ok(new { message = "Registered successfully!" });

            }catch(Exception ex)
            {
                return StatusCode(500, "Server error");

            }
        }


        // se obtine un user dupa email
        [HttpGet("{emailUser}")]
        public async Task<ActionResult<UserDTO>> GetUser(string emailUser)
        {
            try
            {
                var user = await _userRepo.GetUserAsync(emailUser);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }

            }

        // se verifica daca un user este admin
        [HttpGet("isAdmin/{emailUser}", Name = "Exist")]
        public async Task<ActionResult> IsAdmin(string emailUser)
        {
            try
            {
                var user = await _userRepo.IsAdminAsync(emailUser);

                if (user == false)
                {
                    return Ok(new { flag = false, message = "User is not an admin!" });
                }

                return Ok(new { flag = true, message = "User is an admin!" });
            }catch(Exception ex)
            {
                return StatusCode(500, "Server error");

            }
        }

        // login clasic
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login(LoginDTO user)
        {
            try
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



                if (verification)
                {
                    return Ok(new { message = "Login successful!" });

                }

                return Unauthorized(new { message = "Password is incorrect!" });
            }catch(Exception ex)
            {
                return StatusCode(500, "Server error");

            }

        }

        //login cu google
        [HttpGet("LoginGoogle")]
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var existsUser = await _userRepo.GetUserAsync(email);  // se verifica daca exista user-ul

            if(existsUser == null)
            {
                UserInsertDTO user = new UserInsertDTO();
                user.Name = name;
                user.PhoneNr = "null";
                user.Email = email;
                user.IsAdmin = false;
                user.Pass = "null";

                var userToAdd = _mapper.Map<ToDoListInfo.API.DBLayer.Entities.Users>(user);  // daca nu, este adaugat

                _userRepo.AddUser(userToAdd);
            }
            var redirectUrl = $"http://localhost:5173/google-callback?email={Uri.EscapeDataString(email)}";

            return Redirect(redirectUrl);

        }

        // schimbare parola
        [HttpPost("changePassword")]
        public async Task<ActionResult> ChangePassword(LoginDTO user)
        {

            try
            {
                if(user == null)
                {
                    return NotFound();
                }

                var userExist = await _userRepo.GetUserAsync(user.Email);

                if (userExist == null)
                {
                    return NotFound($"Invalid email");
                }

                var newPass = user.Pass;
                var email = user.Email;

                var userChanged = await _userRepo.ChangePasswordUser(newPass, email);

                return Ok(userChanged);
            }

            catch (Exception ex)
            {
                return BadRequest();

            }
        }

    }
}