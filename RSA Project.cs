using System;
using System.Numerics; // BigInteger

namespace P4
{
    class Program
    {
        public static string P4(string[] args)
        {
            int p_e = int.Parse(args[0]);
            int p_c = int.Parse(args[1]);
            int q_e = int.Parse(args[2]);
            int q_c = int.Parse(args[3]);

            string TextCipher = args[4];
            string TextPlain = args[5];

            var m = BigInteger.Parse(TextPlain);
            var c = BigInteger.Parse(TextCipher);

            BigInteger p = BigInteger.Subtract(BigInteger.Pow(2, p_e), p_c);
            BigInteger q = BigInteger.Subtract(BigInteger.Pow(2, q_e), q_c);
            BigInteger n = BigInteger.Multiply(p, q);
            BigInteger phi = BigInteger.Multiply((p - 1), (q - 1));
            BigInteger e = 65537;

            var encryptedResult = BigInteger.ModPow(m, e, n);

            BigInteger d = modInverse(e, phi);

            var decryptedResult = BigInteger.ModPow(c, d, n);

            string P4_answer = String.Format("{0},{1}", decryptedResult, encryptedResult);
            
            Console.WriteLine(P4_answer);
            return P4_answer;
        }
        

        public static BigInteger modInverse(BigInteger e, BigInteger phi)
        {
            BigInteger m0 = phi;
            BigInteger y1 = 0, x1 = 1; 

            if (phi == 1)
                return 0;

            while (e > 1) 
            {
                BigInteger q = BigInteger.Divide(e, phi);
                BigInteger t = phi;

                phi = e % phi;
                e = t;
                t = y1;

                y1 = x1 - q * y1;
                x1 = t;
            }

            if (x1 < 0)
                x1 += m0;

            return x1;
        }
        
        static void Main(string[] args)
        {
            P4(args);
        }
    }
}
