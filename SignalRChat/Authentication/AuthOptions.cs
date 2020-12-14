namespace SignalRChat.Authentication
{
    using System.Text;

    using Microsoft.IdentityModel.Tokens;

    public class AuthOptions
    {
        /// <summary>
        /// Потребитель токена.
        /// </summary>
        public const string AUDIENCE = "MyAuthClient";

        /// <summary>
        /// Издатель токена.
        /// </summary>
        public const string ISSUER = "MyAuthServer";

        /// <summary>
        /// Ключ для шифрации.
        /// </summary>
        public const string KEY = "secretkey_secretkey!123";

        /// <summary>
        /// Время жизни токена.
        /// </summary>
        public const int LIFETIME = 10;

        /// <summary>
        /// Получить ключ безопасности.
        /// </summary>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}