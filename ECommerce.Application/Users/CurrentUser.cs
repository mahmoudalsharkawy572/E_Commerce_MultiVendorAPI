using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Users
{
    public record CurrentUser(string Id, string Emaill, bool IsAuthenticated)
    {
    }
}
