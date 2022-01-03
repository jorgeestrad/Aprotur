namespace AproturWeb.Controllers.Api
{
    using Aprotur.Common.Models;
    using AproturWeb.Helpers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly IMailHelper mailHelper;

        public AccountController(
           IUserHelper userHelper,
             IMailHelper mailHelper)
        {
            this.userHelper = userHelper;
            this.mailHelper = mailHelper;
        }

        /// <summary>
        /// /Permite adicionar un Usuario
        /// </summary>
        /// <param name="request">Datos del nuevo usuario</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] NewUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Este correo electónico ya está registrado!"
                });
            }
           
            user = new Data.Entities.User
            {
                Apellidos = request.Apellidos,
                Nombres = request.Nombres,
                Email = request.Email,
                UserName = request.Email,
                UserType = Aprotur.Common.Enums.UserType.User,
                EmailConfirmed = true,
            };

            var result = await this.userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return this.BadRequest(result.Errors.GetEnumerator().Current.Description);
            }
            var userType = this.userHelper.GetuserTypeCustomerAsync();
            user = await this.userHelper.GetUserAsync(user.UserName);
            await this.userHelper.AddUserToRoleAsync(user, "Customer");

            var listObjects = await this.userHelper.GetAllObjectsAsync();
            foreach (Data.Entities.Objeto obj in listObjects)
            {
                var permissionsPerUserType = this.userHelper.GetAllPermissionPerUserType(userType.Id, obj.Id);
             
                Data.Entities.PermisosPorUsuario permissionPerUser = new Data.Entities.PermisosPorUsuario
                {
                    Objeto = obj,
                    User = user,
                    Read = permissionsPerUserType.Read,
                    Insert = permissionsPerUserType.Insert,
                    Update = permissionsPerUserType.Update,
                    Delete = permissionsPerUserType.Delete,
                    Download = permissionsPerUserType.Download,
                };

                string _hash = $"R{permissionPerUser.Read}-I{permissionPerUser.Insert}-U{permissionPerUser.Update}-D{permissionPerUser.Delete}-L{permissionPerUser.Download}-O{permissionPerUser.ObjetoId}-U{permissionPerUser.UserId}";
                SecurityHelper securityHelper = new SecurityHelper();
                permissionPerUser.Hash = securityHelper.GetHashSha256(_hash);

                await this.userHelper.AddPermissionPerUser(permissionPerUser);

               
            }

            var myToken = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            this.mailHelper.SendMail(request.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                $"To allow the user, " +
                $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>","GeoPlus", request.FullName);

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "A Confirmation email was sent. Plese confirm your account and log into the App."
            });
        }

    }
}
