using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IceSync.Core.Helpers
{
    public static class HashHelper
    {
        public static string ComputeHash<T>(IList<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(json);
            var hashBytes = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
