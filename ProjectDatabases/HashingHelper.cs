using System.Security.Cryptography;

namespace ProjectDatabases;

public class Hashinghelper
{
    public static string Generatesalt(int size = 16)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }

    public static string HasWithSalt(string value, string base64Salt, int interations = 100_000, int hasLength = 32)
    {
        var saltBytes = Convert.FromBase64String(base64Salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password: value,
            salt: saltBytes,
            iterations: interations,
            hashAlgorithm: HashAlgorithmName.SHA256);

        var hash = pbkdf2.GetBytes(hasLength);
        return Convert.ToBase64String(hash);
    }

    public static bool verify(string value, string base64Salt, string expectedBase64Hash)
    {
        var computedHash = HasWithSalt(value, base64Salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(expectedBase64Hash),
            Convert.FromBase64String(computedHash));
    }
}