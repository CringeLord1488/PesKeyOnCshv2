using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerApp.Utils;

public static class AesEncryption
{
    // 🔐 Генерация ключа из мастер-пароля
    private static byte[] DeriveKeyFromPassword(string masterPassword)
    {
        byte[] salt = Encoding.UTF8.GetBytes("your-salt-here"); // Не меняй после первого запуска!
        using var deriveBytes = new Rfc2898DeriveBytes(masterPassword, salt, 10000, HashAlgorithmName.SHA512);
        return deriveBytes.GetBytes(32); // 32 байта для AES-256
    }

    // 🔒 Шифрование пароля (принимает мастер-пароль)
    public static string Encrypt(string plainText, string masterPassword)
    {
        byte[] key = DeriveKeyFromPassword(masterPassword); // ✅ Теперь masterPassword передан
        byte[] iv = new byte[16]; // Новый IV при каждом шифровании

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(plainText);
        sw.Flush();
        cs.FlushFinalBlock();

        byte[] encryptedData = ms.ToArray();
        byte[] combined = new byte[iv.Length + encryptedData.Length];

        Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
        Buffer.BlockCopy(encryptedData, 0, combined, iv.Length, encryptedData.Length);

        return Convert.ToBase64String(combined);
    }

    // 🔓 Расшифровка пароля (тоже принимает мастер-пароль)
    public static string Decrypt(string cipherText, string masterPassword)
    {
        try
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);

            byte[] encryptedData = new byte[fullCipher.Length - iv.Length];
            Buffer.BlockCopy(fullCipher, iv.Length, encryptedData, 0, encryptedData.Length);

            byte[] key = DeriveKeyFromPassword(masterPassword); // ✅ Теперь masterPassword используется
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(encryptedData);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка расшифровки: {ex.Message}");
            return "[Ошибка]";
        }
    }
}