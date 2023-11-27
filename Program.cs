using System.Text;
using System.Security.Cryptography;


string originalText = @"Hello, this is a secret message!";
string password = "MySecretPassword";

string encryptedText = EncryptString(originalText, password);
Console.WriteLine($"Encrypted Text: {encryptedText}");

string decryptedText = DecryptString(encryptedText, password);
Console.WriteLine($"Decrypted Text: {decryptedText}");

//Console.WriteLine(DecryptString("qA+aAYSkax0tI1aNzPUrabOVktRUgFCIFHhB/Z4Jbl6wu2/UvYaj4DnOenCRQ9PaslbBfhtPKtw5c0+GBHSTlFd5N4J7+aWiqCQgyJo/icRSLbUIm1tYCbitqBXnP2mCUPMHONV8kF5ZYcgjcycAa2hb3vSc3tEXtFJ5feD4geLite/k03uWpxDZHVqS6t36z3+07KISPiYX7Z8w0Oc2Njk9un6+0eeKAxSaZTB7e0X3sJsDBXzXn7A8pmYOIwCEZqSOT5d/VlsB7PHreJjoot8dLvhUTXXaJ+kfcSO/0yGqjYFaKz1m0QLqz2TiBKHYId8AvuSI0XOyiAZTOkatX6tGc+plLsFKgC/217gem8A=",Console.ReadLine()));


static string EncryptString(string plainText, string password)
{
    using (Aes aesAlg = Aes.Create())
    {
        Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("salt123"), 10000,HashAlgorithmName.SHA512);
        aesAlg.Key = keyDerivation.GetBytes(32); // 256 bits
        aesAlg.IV = keyDerivation.GetBytes(16); // 128 bits

        using (MemoryStream msEncrypt = new MemoryStream()){
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        
    }
}

static string DecryptString(string cipherText, string password)
{
    using (Aes aesAlg = Aes.Create())
    {
        Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("salt123"), 10000,HashAlgorithmName.SHA512);
        aesAlg.Key = keyDerivation.GetBytes(32); // 256 bits
        aesAlg.IV = keyDerivation.GetBytes(16); // 128 bits

        using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
            return srDecrypt.ReadToEnd();
        }
        
    
    }
}