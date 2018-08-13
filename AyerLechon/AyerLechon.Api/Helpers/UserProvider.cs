using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AyerLechon.Api.Helpers
{
    public static class UserProvider
    {
        public static int GetId()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return int.Parse(identity.Claims.FirstOrDefault(a => a.Type == "userid")?.Value);
        }
    }
}