using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Repos;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.AspNetCore.Identity;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace ToDoList.API.Presentation_Layer.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/users")]
    //[Route("api/[controller]")]
    [Asp.Versioning.ApiVersion(1)]


    public class UsersControllers : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ListsControllers> _logger;
        //private readonly string _googleUserInfoUrl = configuration["Authorization:Google:UserInfoUrl"]!;
        //private readonly SignInManager<User> _signInManager;



        public UsersControllers(IUserRepo userRepo, IMapper mapper, ILogger<ListsControllers> logger)
        {
            _userRepo = userRepo ??
                throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            //_signInManager = signInManager ??
            //    throw new ArgumentNullException(nameof(signInManager));
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

            var userToAdd = _mapper.Map<ToDoListInfo.API.DBLayer.Entities.Users>(user);


            _userRepo.AddUser(userToAdd);

            //await _userRepo.SaveChangesAsync();

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

        [HttpGet("isAdmin/{emailUser}", Name = "Exist")]
        public async Task<ActionResult> IsAdmin(string emailUser)
        {
            var user = await _userRepo.IsAdminAsync(emailUser);

            if (user == false)
            {
                return Ok(new { flag = false, message = "User is not an admin!" });
            }

            return Ok(new { flag = true, message = "User is an admin!" });
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



            if (verification)
            {
                return Ok(new { message = "Login successful!" });

            }

            return Unauthorized(new { message = "Password is incorrect!" });


        }

        //[GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        //public async Task<ActionResult> DriveFileList([FromServices] IGoogleAuthProvider auth)
        //{
        //    GoogleCredential cred = await auth.GetCredentialAsync();
        //    var service = new DriveService(new BaseClientService.Initializer
        //    {
        //        HttpClientInitializer = cred
        //    });
        //    var files = await service.Files.List().ExecuteAsync();
        //    var fileNames = files.Files.Select(x => x.Name).ToList();
        //    return View(fileNames);
        //}

        //private ActionResult View(List<string> fileNames)
        //{
        //    throw new NotImplementedException();
        //}

        //public IActionResult ExternalLogin(string provider, string returnUrl = "")
        //{
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { ReturnUrl = returnUrl });

        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        //    return new ChallengeResult(provider, properties);
        //}

        //[HttpPost("LoginGoogle")]
        //public IResult LoginGoogle([FromQuery] string returnUrl, LinkGenerator linkGenerator, SignInManager<User> signInManager, HttpContext context)
        //{
        //    var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", linkGenerator.GetPathByName(context, "GoogleLoginCallback") + $"?returnUrl={returnUrl}");

        //    return Results.Challenge(properties, ["Google"]);

        //}

        //[HttpPost("LoginGoogleCallback")]
        //public async Task<IResult> LoginGoogleCallback([FromQuery] string returnUrl, HttpContext context, IUserRepo userRepo)
        //{
        //    var result = await context.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    if (!result.Succeeded)
        //    {
        //        return Results.Unauthorized();
        //    }

        //    await userRepo.LoginGoogleAsync(result.Principal);

        //    return Results.Redirect(returnUrl);
        //}

    }
}