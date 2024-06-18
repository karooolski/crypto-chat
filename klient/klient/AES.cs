using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    class AES
    {
        //public static void Main()
        //{
        //    string original = "Here is some data to encrypt!";

        //    using (Aes myAes = Aes.Create())
        //    {
        //        byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);
        //        string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

        //        Console.WriteLine("Original:   {0}", original);
        //        Console.WriteLine("Round Trip: {0}", roundtrip);
        //    }
        //}

        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            try
                            {
                                return srDecrypt.ReadToEnd();
                            }
                            catch (System.Security.Cryptography.CryptographicException)
                            {
                                Console.WriteLine("Padding is invalid and cannot be removed.");
                                return "[ERROR] Padding is invalid and cannot be removed.";
                            }

                        }
                    }
                }
            }
        }


       static byte[] generateIV() // generowanie wektora inicjalizujacego 
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }
        }

        // ok dziala generowanie iv
        static byte[] GetLegalIV(string key)
        {
            byte[] IV = generateIV();
            byte[] bytTemp = IV;
            int IVLength = bytTemp.Length;
            if (key.Length > IVLength)
                key = key.Substring(0, IVLength);
            else if (key.Length < IVLength)
                key = key.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(key);
        }
        // https://github.dev/ScutGame/Scut/blob/b30dcfacb9d2bc81e0e81dfd39b970a9af1e6f2a/Source/Middleware/ZyGames.Framework.Game/Context/EncryptionManager.cs#L109#L120

        // uzgadnianie krotszego klczua do AES 
        static string IVtestgenarator(string key)
        {
            byte[] ivbyte = GetLegalIV(key);
            string iv = Convert.ToBase64String(ivbyte);
            Console.WriteLine($"wygenerowany iv: {iv}");
            return iv;
        }

        // uzgadanianie klucza do AES : 
        // czyli przepusc klucz prywatny z diffie hellman (liczba w postaci string) przez SHA256
        static string uzgodnij_klucz_do_AES(string klucz_prywanty) // Tworzenie klucza symetrycznego (np. z hasła "1234")
        {
            byte[] key;
            using (var sha256 = SHA256.Create())
            {
                key = sha256.ComputeHash(Encoding.UTF8.GetBytes(klucz_prywanty));
            }
            string keystr = Convert.ToBase64String(key);
            //Console.WriteLine($"{keystr}");
            return keystr;
        }


        public static string Encrypt(string message, string key)
        {
            string klucz_do_AES = uzgodnij_klucz_do_AES(key);                       // AES wymaga 2 kluczy symetrycznych
            string klucz_IV = IVtestgenarator(key);
            byte[] byte_AES_key = Convert.FromBase64String(klucz_do_AES);           // AES z dokumentacji Microsoftu wymaga kluczy w postaci bajtow,tutaj SHA256 base64 to klucz1
            byte[] byte_IV = Convert.FromBase64String(klucz_IV);
            byte[] encrypted = AES.EncryptStringToBytes_Aes(message, byte_AES_key, byte_IV);
            string enrypted_str = Convert.ToBase64String(encrypted);
            return enrypted_str; // musze zamianiac na string bo chce przesylac obiekt z danymi zserializowany przez json serializer do stringa miedzy klient - serwer - klient
        }
        public static string Decrypt(string encrypted, string key)
        {
            string klucz_do_AES = uzgodnij_klucz_do_AES(key);
            string klucz_IV = IVtestgenarator(key);
            byte[] byte_encrypted = Convert.FromBase64String(encrypted);
            byte[] byte_AES_key = Convert.FromBase64String(klucz_do_AES);
            byte[] byte_IV = Convert.FromBase64String(klucz_IV);
            string decrypted = AES.DecryptStringFromBytes_Aes(byte_encrypted, byte_AES_key, byte_IV);
            return decrypted;
        }


    }

}
