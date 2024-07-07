using System.Security.Cryptography;
using System.Text;

namespace ires.Domain
{
    public class Utility
    {
        public static DateTime GetServerTime()
        {
            return DateTime.Now;
            //TimeZoneInfo manilaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            //return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, manilaTimeZone);
        }

        public static string CleanFileName(string input)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(input, invalidRegStr, "_");
        }
        public static string GetHash(string input)
        {
            MD5 hasher = MD5.Create();
            byte[] dBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dBytes.Length; i++)
            {
                sb.Append(dBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static string RandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        public const string EncryptionKey = "RoyetteCamahalan";
        public static string URLEncrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
#pragma warning disable SYSLIB0041 // Type or member is obsolete
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
#pragma warning restore SYSLIB0041 // Type or member is obsolete
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            clearText = clearText.Replace("/", "_slash_").Replace("+", " ");
            return clearText;
        }

        public static string URLDecrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+").Replace("_slash_", "/");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
#pragma warning disable SYSLIB0041 // Type or member is obsolete
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
#pragma warning restore SYSLIB0041 // Type or member is obsolete
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
