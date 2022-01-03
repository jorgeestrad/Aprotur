using Aprotur.Common.Enums;
using AproturWeb.Data.Entities;
using AproturWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AproturWeb.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<User> GetUserAsync(Guid id);

        User GetUserByEmail(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> DeleteUserAsync(User user);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<List<Objeto>> GetAllObjectsAsync();

        IdentityRole GetuserTypeCustomerAsync();

        PermisosPorRol GetAllPermissionPerUserType(string UserTypeId, int ObjectId);

        Task AddPermissionPerUser(PermisosPorUsuario permissionPerUser);

        IdentityRole GetRolUsuario();

        void AsingUserTypeToUser(User user);
    }
}
