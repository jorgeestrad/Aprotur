namespace AproturWeb.Helpers
{
    using System.Security.Cryptography;
    using System.Text;
    public class SecurityHelper : ISecurityHelper
    {
        private SHA256 Sha256 = SHA256.Create();
        public string GetHashSha256(byte[] data)
        {
            byte[] hash = Sha256.ComputeHash(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public string GetHashSha256(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            byte[] hash = Sha256.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
