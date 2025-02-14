using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace APIMain.Authentication.Password {
    internal class PasswordHasher {
        public static string Hash(string password) {
            // Generates a random salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(salt);
            }

            // Derive the key using PBKDF2 (HMACSHA1)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1, // Use HMACSHA1 as the pseudorandom function
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        }

        public static bool Verify(string enteredPassword, string storedPassword) {
            // Split the stored password into salt and hashed password
            var parts = storedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHashedPassword = parts[1];

            // Derive the hash from the entered password using the same salt
            string enteredHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return storedHashedPassword == enteredHashedPassword;
        }
    }
}
