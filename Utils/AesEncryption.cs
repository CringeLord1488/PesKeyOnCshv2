using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerApp.Utils;

public static class AesEncryption
{
    // 🔑 Генерация ключа из пароля пользователя
    private static byte[] DeriveKeyFromPassword(string password)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA512);
        return deriveBytes.GetBytes(32); // 32 байта для AES-256
    }

    // 🔒 Шифрование с мастер-паролем
    public static string Encrypt(string plainText, string userPassword)
    {
        byte[] key = DeriveKeyFromPassword(userPassword);
        byte[] iv = new byte[16]; // IV всегда 16 байт

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

    // 🔓 Расшифровка с мастер-паролем
    public static string Decrypt(string cipherText, string userPassword)
    {
        try
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            byte[] iv = new byte[16];
            byte[] encryptedData = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, encryptedData, 0, encryptedData.Length);

            byte[] key = DeriveKeyFromPassword(userPassword);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(encryptedData);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
        catch
        {
            return "[Ошибка расшифровки]";
        }
    }
}