using CSProjeDemo1.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string ad, string soyad, string email, string password, RoleType role);
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
