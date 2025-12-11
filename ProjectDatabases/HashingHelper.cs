using System.Security.Cryptography;

namespace ProjectDatabases;

/// <summary>
/// Provides helper methods for generating salts, hashing values using PBKDF2,
/// and verifying hashed data in a secure and consistent manner.
/// </summary>
public class Hashinghelper
{
    /// <summary>
    /// Generates a cryptographically secure random salt.
    /// </summary>
    public static string Generatesalt(int size = 16)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>
    /// Computes a PBKDF2 hash for the specified value using the provided salt.
    /// </summary>
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

    /// <summary>
    /// Verifies a plain-text value by hashing it with the provided salt
    /// and comparing it to the expected hash using a time-constant comparison.
    /// </summary>
    public static bool verify(string value, string base64Salt, string expectedBase64Hash)
    {
        var computedHash = HasWithSalt(value, base64Salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(expectedBase64Hash),
            Convert.FromBase64String(computedHash));
    }
}