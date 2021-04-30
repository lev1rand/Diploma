using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;
using DiplomaServices.Models;

namespace DiplomaServices.Services
{
    public class PasswordHasher
    {
        #region

        private string salt;

        #endregion

        public PasswordHashedInfo GetEncodedInfoWithSaltGenerated(string originalPassword)
        {
            GenerateSalt();

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: originalPassword,
            salt: Encoding.ASCII.GetBytes(this.salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            return new PasswordHashedInfo { Salt = salt, PasswordHash = hashed };
        }

        public string GetEncodedInfoWithSaltExisting(string originalPassword, string saltExisting)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: originalPassword,
            salt: Encoding.ASCII.GetBytes(saltExisting),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            return hashed;
        }
        private void GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            this.salt = new string(Encoding.ASCII.GetChars(salt));
        }
    }
}