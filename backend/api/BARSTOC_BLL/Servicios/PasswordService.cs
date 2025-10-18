using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BARSTOC_BLL.Servicios
{
    public static class PasswordService
    {
        // Configuración según ISO 27001:2022
        private const int MIN_PASSWORD_LENGTH = 12;
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 32;
        private const int ITERATIONS = 100000;

        public static string HashPassword(string password)
        {
            // Validar fortaleza de contraseña
            if (!IsPasswordStrong(password))
                throw new ArgumentException("La contraseña no cumple con los requisitos de seguridad ISO 27001:2022");

            // Generar salt
            byte[] salt = new byte[SALT_SIZE];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Generar hash
            byte[] hash = PBKDF2(password, salt, ITERATIONS, HASH_SIZE);

            // Combinar salt y hash
            byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Extraer bytes
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Extraer salt
                byte[] salt = new byte[SALT_SIZE];
                Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

                // Generar hash con el mismo salt
                byte[] hash = PBKDF2(password, salt, ITERATIONS, HASH_SIZE);

                // Comparar hashes
                for (int i = 0; i < HASH_SIZE; i++)
                {
                    if (hashBytes[i + SALT_SIZE] != hash[i])
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        public static bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < MIN_PASSWORD_LENGTH)
                return false;

            // Mínimo 12 caracteres con combinación de:
            var hasUpper = new Regex(@"[A-Z]");
            var hasLower = new Regex(@"[a-z]");
            var hasDigit = new Regex(@"[0-9]");
            var hasSpecial = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]");
            var noSpaces = new Regex(@"^\S*$");

            return hasUpper.IsMatch(password) &&
                   hasLower.IsMatch(password) &&
                   hasDigit.IsMatch(password) &&
                   hasSpecial.IsMatch(password) &&
                   noSpaces.IsMatch(password);
        }

        public static string GenerateSecurePassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";

            var chars = new char[16];
            var random = new Random();

            // Asegurar al menos un carácter de cada tipo
            chars[0] = upper[random.Next(upper.Length)];
            chars[1] = lower[random.Next(lower.Length)];
            chars[2] = digits[random.Next(digits.Length)];
            chars[3] = special[random.Next(special.Length)];

            // Completar con caracteres aleatorios
            string allChars = upper + lower + digits + special;
            for (int i = 4; i < chars.Length; i++)
            {
                chars[i] = allChars[random.Next(allChars.Length)];
            }

            // Mezclar
            return new string(chars.OrderBy(x => random.Next()).ToArray());
        }
    }
}