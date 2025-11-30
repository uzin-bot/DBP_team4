using System.Text;
using System.Security.Cryptography;

namespace leehaeun
{
    internal class Sha256
    {
        private Sha256() { }

        public static Sha256 Instance = new Sha256();

        public string HashSHA256(string pw)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(pw);
                byte[] hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
