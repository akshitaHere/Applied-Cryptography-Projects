using System;
using System.Security.Cryptography; 
using System.Numerics; 
using System.IO;
namespace P3
{
    class Program
    {
        static byte[] EncryptStringToBytes_Aes(string Plain_Text, byte[] Key, byte[] IV)
        {
            if (Plain_Text == null || Plain_Text.Length <= 0)
                throw new ArgumentNullException("Plain_Text");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

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
                            swEncrypt.Write(Plain_Text); 
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
    
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
            {
                
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                string Plain_Text = null;
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
                                Plain_Text = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return Plain_Text;
            }
        
        static byte[] StringToBytes(string input)
        {
            int counter;
            var SplitInput = input.Split(' ');
            byte[] inputBytes = new byte[SplitInput.Length];
            counter = 0;
            foreach (string item in SplitInput)
            {
                inputBytes.SetValue(Convert.ToByte(item, 16), counter);
                counter++;
            }
            return inputBytes;
        }
        
        public static string P3(string[] args)
        {
            string Inputiv = args[0]; 
            string g_e = args[1]; 
            string g_c = args[2];
            string n_e = args[3];
            string n_c = args[4];
            string Inputx = args[5];
            string Inputgy = args[6];
            string encrypt = args[7];
            string plain_text = args[8];
            
            var GY = BigInteger.Parse(Inputgy);
            var ParsedX = BigInteger.Parse(Inputx);
            var N = BigInteger.Subtract(BigInteger.Pow(2,Int32.Parse(n_e)), BigInteger.Parse(n_c));
            Console.Write(N);
            Console.WriteLine("\n");
            var Key = BigInteger.ModPow(GY,ParsedX,N);
            Console.Write(Key);
            Console.WriteLine("\n");
            byte[] iv_bytes = StringToBytes(Inputiv);
            
            string InputPt = DecryptStringFromBytes_Aes(StringToBytes(encrypt),Key.ToByteArray(),StringToBytes(Inputiv)) ;
                    
            Console.WriteLine("\n");  
            byte[] ansBytes = EncryptStringToBytes_Aes(plain_text,Key.ToByteArray(),StringToBytes(Inputiv));
                
            Console.WriteLine(BitConverter.ToString(ansBytes).Replace("-", " "));
                    
            string FinalAnswer = ","; 
            FinalAnswer = InputPt + FinalAnswer+BitConverter.ToString(ansBytes).Replace("-", " ");
            return FinalAnswer;
        }
        static void Main(string[] args)
        {
            P3(args); // This will run your project code. The autograder will grade the return value of the P1_2 function
        }
    }
}