using Aprotur.Common.Enums;
using AproturWeb.Data;
using AproturWeb.Data.Entities;
using AproturWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AproturWeb.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

       
        public async Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType)
        {
            User user = new User
            {
                Address = model.Address,
                Documento = model.Documento,
                Email = model.Username,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                TipoDocumento = await _context.TiposDocumento.FindAsync(model.TipoDocumentoId),
                UserName = model.Username,
                UserType = userType
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;
        }

        public User GetUserByEmail(string email)
        {
            return this._userManager.FindByEmailAsync(email).Result;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users
                .Include(x => x.TipoDocumento)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.Users
                .Include(x => x.TipoDocumento)
                .FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            User currentUser = await GetUserAsync(user.Email);
            currentUser.Apellidos = user.PasswordHash;
            currentUser.Nombres = user.Nombres;
            currentUser.TipoDocumento = user.TipoDocumento;
            currentUser.Documento = user.Documento;
            currentUser.Address = user.Address;
            currentUser.PhoneNumber = user.PhoneNumber;
            return await _userManager.UpdateAsync(currentUser);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<List<Data.Entities.Objeto>> GetAllObjectsAsync()
        {
            return await this._context.Objetos.ToListAsync();
        }

        public PermisosPorRol GetAllPermissionPerUserType(string UserTypeId, int ObjectId)
        {
            return this._context.PermisosPorRoles.Where(f => f.RolId == UserTypeId && f.Objeto.Id == ObjectId).FirstOrDefault();
        }

        public IdentityRole GetuserTypeCustomerAsync()
        {
            var usertype = this._context.Roles.Where(f => f.Name == "Operator").FirstOrDefault();
            if (usertype == null)
            {
                this._context.Roles.Add(new Rol
                {
                    Name = "Operator",
                });
                this._context.SaveChanges();

                usertype = this._context.Roles.Where(f => f.Name == "Operator").FirstOrDefault();
                var listObjects = this._context.Objetos;

                foreach (Data.Entities.Objeto obj in listObjects)
                {
                    PermisosPorRol permissionPerUserType = new PermisosPorRol
                    {
                        Objeto = obj,
                        RolId = usertype.Id,
                        Read = "0",
                        Insert = "0",
                        Update = "0",
                        Delete = "0",
                        Download = "0",
                    };

                    string _hash = $"R{permissionPerUserType.Read}-I{permissionPerUserType.Insert}-U{permissionPerUserType.Update}-D{permissionPerUserType.Delete}-L{permissionPerUserType.Download}-O{permissionPerUserType.ObjetoId}-U{permissionPerUserType.RolId}";
                    SecurityHelper securityHelper = new SecurityHelper();
                    permissionPerUserType.Hash = securityHelper.GetHashSha256(_hash);

                    this._context.Add(permissionPerUserType);

                   
                }
                this._context.SaveChanges();
            }

            return usertype;

        }

        public async Task AddPermissionPerUser(PermisosPorUsuario permissionPerUser)
        {
            this._context.PermisosPorUsuarios.Add(permissionPerUser);
            await this._context.SaveChangesAsync();
            return;
        }

        public IdentityRole GetRolUsuario()
        {
            return this._context.Roles.Where(f => f.Name == "User").FirstOrDefault();
        }

        public void AsingUserTypeToUser(User user)
        {
            var _user = this._context.Users.Find(user.Id);

            _user.UserType =  UserType.User;

            this._context.SaveChanges();
        }
    }
}
