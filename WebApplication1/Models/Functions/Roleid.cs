using System.Security.Claims;
using WebApplication1.Models.IdentityModel;

namespace WebApplication1.Models.Functions
{
    public class Roleid : IRoleId
    {
        public Ident GetIden(ClaimsIdentity identity)
        {
            var userclaims = identity.Claims;
            Ident ident = new Ident()
            {
                Id = long.Parse(userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Name).Value),
                Role = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value
            };
            return ident;
        }
    }
}
