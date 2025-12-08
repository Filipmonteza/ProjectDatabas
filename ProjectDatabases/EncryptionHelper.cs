namespace ProjectDatabases;

public class EncryptionHelper
{
    private const byte Key = 0x42; // 66 bytes 

    public static string Encrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }
        
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ Key);
        }
        
        return Convert.ToBase64String(bytes);
    }
    
    public static string Decrypt(string krypteratText)
    {
        // 1
        if (string.IsNullOrEmpty(krypteratText))
        {
            return krypteratText;
        }
        
        // Gör en Base-64 till byta igen
        var bytes = Convert.FromBase64String(krypteratText);

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ Key);
        }
        
        // 3 Konverterar tillbaka från bytes -> klartext med UTF8
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}