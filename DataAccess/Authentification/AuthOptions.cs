using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataAccess.Authentification
{
    public class AuthOptions
    {
        public const string ISSUER = "TestOnServer"; // издатель токена
        public const string AUDIENCE = "TestOnClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
