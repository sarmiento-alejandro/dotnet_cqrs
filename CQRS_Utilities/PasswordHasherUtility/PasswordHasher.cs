using System.Security.Cryptography;

namespace CQRS_Utilities.PasswordHasherUtility;

public class PasswordHasher
    {
        // El tamaño de la sal en bytes
        private const int SaltSize = 16;
        
        // El número de iteraciones para PBKDF2
        private const int Iterations = 10000;
        
        // Longitud de la clave derivada en bytes
        private const int KeySize = 32;
        
        public static string HashPassword(string password)
        {
            // Generar una sal aleatoria
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            // Crear hash
            byte[] hash = GenerateHash(password, salt, Iterations, KeySize);
            
            // Concatenar el número de iteraciones, la sal y el hash
            byte[] hashBytes = new byte[4 + SaltSize + KeySize];
            BitConverter.GetBytes(Iterations).CopyTo(hashBytes, 0);
            salt.CopyTo(hashBytes, 4);
            hash.CopyTo(hashBytes, 4 + SaltSize);
            
            // Convertir a formato de string para almacenamiento
            return Convert.ToBase64String(hashBytes);
        }
        
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convertir el hash almacenado de vuelta a bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            
            // Extraer el número de iteraciones, la sal y el hash
            int iterations = BitConverter.ToInt32(hashBytes, 0);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 4, salt, 0, SaltSize);
            byte[] storedHash = new byte[KeySize];
            Array.Copy(hashBytes, 4 + SaltSize, storedHash, 0, KeySize);
            
            // Generar hash con la contraseña proporcionada y la misma sal
            byte[] computedHash = GenerateHash(password, salt, iterations, KeySize);
            
            // Comparar los hashes
            return SlowEquals(storedHash, computedHash);
        }
        
        // Comparación de tiempo constante para evitar ataques de tiempo
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
        
        private static byte[] GenerateHash(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }