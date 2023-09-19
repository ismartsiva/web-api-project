using System.Security.Claims;
using WebApplication1.Models.IdentityModel;

namespace WebApplication1.Models.Functions
{
    public interface IRoleId
    {
        public Ident GetIden(ClaimsIdentity identity);
    }
}
