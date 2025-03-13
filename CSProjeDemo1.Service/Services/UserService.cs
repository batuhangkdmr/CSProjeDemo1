using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Core.Enums;
using CSProjeDemo1.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // ✅ Kullanıcı Kaydetme İşlemi
        public async Task<bool> RegisterAsync(string ad, string soyad, string email, string password, RoleType role)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Ad = ad,
                Soyad = soyad
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Eğer rol yoksa önce rolü oluştur
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }

                // Kullanıcıya rol atama
                await _userManager.AddToRoleAsync(user, role.ToString());
                return true;
            }
            return false;
        }

        // ✅ Kullanıcı Girişi
        public async Task<bool> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            return result.Succeeded;
        }

        // ✅ Kullanıcı Çıkışı
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
