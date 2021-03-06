using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FantasyCritic.Lib.Domain;
using Microsoft.AspNetCore.Identity;

namespace FantasyCritic.Lib.Interfaces
{
    public interface IFantasyCriticUserStore : IReadOnlyFantasyCriticUserStore, IUserStore<FantasyCriticUser>, IUserEmailStore<FantasyCriticUser>, IUserPasswordStore<FantasyCriticUser>, IUserRoleStore<FantasyCriticUser>, IUserSecurityStampStore<FantasyCriticUser>
    {
        Task<IReadOnlyList<string>> GetRefreshTokens(FantasyCriticUser user);
        Task AddRefreshToken(FantasyCriticUser user, string refreshToken);
        Task RemoveRefreshToken(FantasyCriticUser user, string refreshToken);
        Task RemoveAllRefreshTokens(FantasyCriticUser user);
        Task ClearOldRefreshTokens(FantasyCriticUser user);
    }
}
