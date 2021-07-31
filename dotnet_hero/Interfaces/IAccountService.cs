using dotnet_hero.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_hero.Interfaces
{
    public interface IAccountService
    {
        Task Register(Account account);
        Task<Account> Login(string username, string password);
        string GenerateToken(Account account);
        Account GetInfo(string accessToken);
    }
}
