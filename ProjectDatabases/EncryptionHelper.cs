namespace ProjectDatabases;

public static class EncryptionHelper
{
    private const byte Key = 0x42; // 66 bytes 
    
    /// <summary>
    /// Encrypts the specified plain text using a single-byte XOR key
    /// and returns the result as a Base64-encoded string.
    /// </summary>
    /// <param name="text">Plain text to encrypt. If null or empty, it is returned unchanged.</param>
    /// <returns>Base64-encoded XOR-obfuscated text.</returns>
    public static string Encrypt(string text)
    {
        // Return early if there is nothing to encrypt
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }
        
        // Convert the input text to UTF-8 bytes
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ Key);
        }
        
        return Convert.ToBase64String(bytes);
    }
    
    /// <summary>
    /// Decrypts a Base64-encoded string that was produced by <see cref="Encrypt"/>.
    /// </summary>
    /// <param name="krypteratText">
    /// Base64-encoded XOR-obfuscated text. If null or empty, it is returned unchanged.
    /// </param>
    /// <returns>Decrypted plain text as UTF-8.</returns>
    public static string Decrypt(string krypteratText)
    {
        // Return early if there is nothing to decrypt
        if (string.IsNullOrEmpty(krypteratText))
        {
            return krypteratText;
        }
        
        // Decode the Base64 string back into the obfuscated bytes
        var bytes = Convert.FromBase64String(krypteratText);

        // Apply the same XOR operation with the static key to restore the original bytes
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ Key);
        }
        
        // Convert the decrypted bytes back into a UTF-8 string
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}