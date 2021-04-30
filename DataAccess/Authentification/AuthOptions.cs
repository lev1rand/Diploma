using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataAccess.Authentification
{
    public class AuthOptions
    {
        public const string ISSUER = "TestOnServer"; // издатель токена
        public const string AUDIENCE = "TestOnClient"; // потребитель токена
        const string KEY = "eda9bced-f0f6-44cb-9e5c-ad4751f3428e";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 30 минут (6*5)
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
