namespace GeoPlus.Controllers
{
    using GeoPlus.Data.Entities;
    using GeoPlus.Helpers;
    using GeoPlus.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity;
    using System;

    /// <summary>
    /// Adminitración de las cuentas de usaurios
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly IMailHelper mailHelper;
        private User user { get; set; }
        private PermisosPorUsuario permission;
      
        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        /// <param name="userHelper"></param>
        /// <param name="configuration"></param>
        /// <param name="mailHelper"></param>
        /// <param name="countryRepository"></param>
        /// <param name="userTypeRepository"></param>
        /// <param name="localizer"></param>
        public AccountController(IUserHelper userHelper,
                                 IMailHelper mailHelper)
        {
           this.userHelper = userHelper;
           this.mailHelper = mailHelper;
           
        }

        /// <summary>
        /// Listado de Usuarios
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.Name != null)
            {
                var user = userHelper.GetUserByEmail(this.User.Identity.Name);
              
            }
            return this.View();
        }

        /// <summary>
        /// Autenticación del usuario
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel();


            return this.View(model);
        }

        /// <summary>
        /// Valida la información de las credenciales ingresadas por el usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Usuario o contraseña no válidos!");
            return this.View(model);
        }

        /// <summary>
        /// Cierra la sesión de un usuario
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await this.userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Permite llamar el formulario de registro de los usuarios
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            ViewData["RegisterTitle"] = "Registro de Usuario";
            ViewData["RegisterButton"] = "Registrarse";

            var model = new RegisterNewUserViewModel
            {
                UserType = this.userHelper.GetRolUsuario(),
            };

            return this.View(model);
        }

        /// <summary>
        /// Registra el usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var UserType = this.userHelper.GetRolUsuario();
                    var user = await this.userHelper.GetUserAsync(model.Username);
                    if (user == null)
                    {
                        user = new User
                        {
                            Nombres = model.Nombres,
                            Apellidos = model.Apellidos,
                            Email = model.Username,
                            UserName = model.Username,
                            EmailConfirmed = true,
                            Documento = "11111",
                        };

                        var result = await this.userHelper.AddUserAsync(user, model.Password);
                        if (result != IdentityResult.Success)
                        {
                            this.ModelState.AddModelError(string.Empty, "El usuario no se pudo registrar!");
                            return this.View(model);
                        }

                        user = await this.userHelper.GetUserAsync(model.Username);
                        await this.userHelper.AddUserToRoleAsync(user, "User");

                        this.userHelper.AsingUserTypeToUser(user);

                        var listObjects = await this.userHelper.GetAllObjectsAsync();
                        foreach (Data.Entities.Objeto obj in listObjects)
                        {
                            var permissionsPerUserType = this.userHelper.GetAllPermissionPerUserType(UserType.Id, obj.Id);
                            PermisosPorUsuario permissionPerUser = new PermisosPorUsuario
                            {
                                Objeto = obj,
                                User = user,
                                Read = permissionsPerUserType.Read,
                                Insert = permissionsPerUserType.Insert,
                                Update = permissionsPerUserType.Update,
                                Delete = permissionsPerUserType.Delete,
                                Download = permissionsPerUserType.Download,
                                Hash = $"R{permissionsPerUserType.Read}-I{permissionsPerUserType.Insert}-U{permissionsPerUserType.Update}-D{permissionsPerUserType.Delete}-L{permissionsPerUserType.Download}-O{obj.Id}-U{user.UserType}",
                            };
                            SecurityHelper securityHelper = new SecurityHelper();
                            permissionPerUser.Hash = securityHelper.GetHashSha256(permissionPerUser.Hash);

                            await this.userHelper.AddPermissionPerUser(permissionPerUser);

                            
                        }
                      
                        return this.RedirectToAction("Login", "Account");
                    }

                    this.ModelState.AddModelError(string.Empty, "El usuario ya se encuentra registrado!");
                }
            }
            catch (Exception exp)
            {
                this.ModelState.AddModelError(string.Empty, exp.Message);
            }

            
            return this.View(model);
        }


        ///// <summary>
        ///// Verifica el Correo electrónico
        ///// </summary>
        ///// <param name="userId">Identifica el Usuario</param>
        ///// <param name="token">Token de seguridad</param>
        ///// <returns></returns>
        //public async Task<IActionResult> ConfirmEMail(string userId, string token)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        //    {
        //        return this.NotFound();
        //    }

        //    var user = await this.userHelper.GetUserByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return this.NotFound();
        //    }

        //    var result = await this.userHelper.ConfirmEmailAsync(user, token);
        //    if (!result.Succeeded)
        //    {
        //        return this.NotFound();
        //    }
        //    return View();
        //}

        ///// <summary>
        ///// Inicia el proceso de actualización de la información de un Usuario
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> ChangeUser()
        //{
        //    var user = await this.userHelper.GetUserAsync(this.User.Identity.Name);
        //    var model = new ChangeUserViewModel();
        //    if (user != null)
        //    {
        //        model.FirstName = user.FirstName;
        //        model.LastName = user.LastName;
        //        model.Telephone = user.Telephone;
        //        model.UserSignature = user.UserSignature;
        //        if (user.UserSignature != null) model.UserSignatureBase64 = Convert.ToBase64String(user.UserSignature);
        //        model.UserImage = user.ImageArray;

        //        var city = await this.countryRepository.GetCityAsync(user.CityId);
        //        if (city != null)
        //        {
        //            var country = await this.countryRepository.GetCountryAsync(city);
        //            if (country != null)
        //            {
        //                model.CountryId = country.Id;
        //                model.Cities = this.countryRepository.GetComboCities(country.Id);
        //                model.Countries = this.countryRepository.GetComboCountries();
        //                model.CityId = user.CityId;
        //            }
        //        }
        //    }

        //    model.Cities = this.countryRepository.GetComboCities(model.CountryId);
        //    model.Countries = this.countryRepository.GetComboCountries();

        //    ViewData["PhotoUser"] = "https://raw.githubusercontent.com/azouaoui-med/pro-sidebar-template/gh-pages/src/img/user.jpg";
        //    if (this.User.Identity.Name != null)
        //    {
        //        if (!string.IsNullOrEmpty(user.ImagePath) && user.ImagePath.Length > 1)
        //        {
        //            ViewData["PhotoUser"] = $"/a_docplus/{user.ImagePath.Substring(1)}";
        //        }
        //    }
        //    return this.View(model);
        //}

        ///// <summary>
        ///// Registra los cambios de la información de un usuario
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
        //        if (user != null)
        //        {
        //            var path = string.Empty;

        //            if (model.ImageFile != null && model.ImageFile.Length > 0)
        //            {
        //                var guid = Guid.NewGuid().ToString();
        //                var file = $"{guid}.jpg";

        //                path = Path.Combine(
        //                    Directory.GetCurrentDirectory(),
        //                    "wwwroot\\images\\users",
        //                    file);

        //                using (var stream = new FileStream(path, FileMode.Create))
        //                {
        //                    await model.ImageFile.CopyToAsync(stream);
        //                }

        //                path = $"~/images/users/{file}";
        //            }

        //            if (model.ImageFileSignature != null && model.ImageFileSignature.Length > 0)
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    model.ImageFileSignature.CopyTo(ms);
        //                    model.UserSignature = ms.ToArray();
        //                    model.UserSignatureBase64 = Convert.ToBase64String(model.UserSignature);
        //                }
        //            }

        //            if (model.ImageFile != null && model.ImageFile.Length > 0)
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    model.ImageFile.CopyTo(ms);
        //                    model.UserImage = ms.ToArray();
        //                }
        //            }

        //            var _city = await this.countryRepository.GetCityAsync(model.CityId);

        //            user.FirstName = model.FirstName;
        //            user.LastName = model.LastName;
        //            user.Telephone = model.Telephone;
        //            user.CityId = model.CityId;
        //            user.ImageArray = model.UserImage;
        //            user.UserSignature = model.UserSignature;
        //            user.City = _city;
        //            user.ImagePath = path;
        //            this.userHelper.UpdateUser(user);
        //        }
        //        else
        //        {
        //            this.ModelState.AddModelError(string.Empty, this.localizer["UserNofound"]);
        //        }
        //    }

        //    model.Cities = this.countryRepository.GetComboCities(model.CountryId);
        //    model.Countries = this.countryRepository.GetComboCountries();
        //    return this.View(model);
        //}

        ///// <summary>
        ///// Llama el formulario que permite al usuario cambiar su password
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult ChangePasswordUser()
        //{
        //    return this.View();
        //}

        ///// <summary>
        ///// Realiza el cambio del Password
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> ChangePasswordUser(ChangePasswordViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
        //        if (user != null)
        //        {
        //            var result = await this.userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return this.RedirectToAction("ChangeUser");
        //            }
        //            else
        //            {
        //                this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
        //            }
        //        }
        //        else
        //        {
        //            this.ModelState.AddModelError(string.Empty, this.localizer["UserNofound"]);
        //        }
        //    }

        //    return this.View(model);
        //}

        ///// <summary>
        ///// Llama el formulario que le permite al usaurio recuperar su password
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult RecoverPassword()
        //{
        //    return this.View();
        //}

        ///// <summary>
        ///// Realiza las acciones que le permiten al usuario cambiar su password
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await this.userHelper.GetUserByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, this.localizer["EmailDoesnotCorrespont"]);
        //            return this.View(model);
        //        }

        //        var myToken = await this.userHelper.GeneratePasswordResetTokenAsync(user);
        //        var link = this.Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
        //        var mailSender = new MailHelper(configuration);
        //        mailSender.SendMail(model.Email, this.localizer["PasswordReset"], $"<h1>{this.localizer["PasswordRecover"]}</h1>" +
        //            $"{this.localizer["ToResetPassword"]}</br></br><a href = \"{link}\">{this.localizer["PasswordReset"]}</a>");
        //        this.ViewBag.Message = this.localizer["ResetMessage"];
        //        return this.View();

        //    }

        //    return this.View(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public IActionResult ResetPassword(string token)
        //{
        //    return View();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    var user = await this.userHelper.GetUserByEmailAsync(model.UserName);
        //    if (user != null)
        //    {
        //        var result = await this.userHelper.ResetPasswordAsync(user, model.Token, model.Password);
        //        if (result.Succeeded)
        //        {
        //            this.ViewBag.Message = this.localizer["PasswordResetSuccessful"];
        //            return this.View();
        //        }

        //        this.ViewBag.Message = this.localizer["ErrorResetPassword"];
        //        return View(model);
        //    }

        //    this.ViewBag.Message = this.localizer["UserNofound"];
        //    return View(model);
        //}

        ///// <summary>
        ///// Crea un token para el usuario
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await this.userHelper.GetUserByEmailAsync(model.Username);
        //        if (user != null)
        //        {
        //            var result = await this.userHelper.ValidatePasswordAsync(
        //                user,
        //                model.Password);

        //            if (result.Succeeded)
        //            {
        //                var claims = new[]
        //                {
        //                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //                };

        //                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
        //                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //                var token = new JwtSecurityToken(
        //                    this.configuration["Tokens:Issuer"],
        //                    this.configuration["Tokens:Audience"],
        //                    claims,
        //                    expires: DateTime.UtcNow.AddDays(15),
        //                    signingCredentials: credentials);
        //                var results = new
        //                {
        //                    token = new JwtSecurityTokenHandler().WriteToken(token),
        //                    expiration = token.ValidTo
        //                };

        //                return this.Created(string.Empty, results);
        //            }
        //        }
        //    }

        //    return this.BadRequest();
        //}

    }
}
