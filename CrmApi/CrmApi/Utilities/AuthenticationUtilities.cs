using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CrmApi.Utilities
{
    public static class AuthenticationUtilities
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey) =>
            new(Encoding.ASCII.GetBytes(secretKey));
    }
}