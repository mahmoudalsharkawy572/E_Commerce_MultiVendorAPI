using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Users
{
    public class UserContext(IHttpContextAccessor _httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new InvalidOperationException("User context is not presented!");

            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return null;

            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
           
            return new CurrentUser(id!, email!, true );
        }
    }
}
