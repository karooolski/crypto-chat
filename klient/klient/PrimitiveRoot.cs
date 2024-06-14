using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


// 2024 06 13 20 37
// podsatawa tego skryotu jest implementacja algorytm,u diffie hemlan
// pierw ala przesyla do olo liczbe pierwsa p
// obie strony ustalaja podstawe inaczej zwana g albo primitive root czyli prymitywny pierwiastek : g
// i ten g wlasie jest wyznaczany w tym skrypcie

namespace klient
{
    class PrimitiveRoot
    {
        //static void Main7()
        //{
        //    BigInteger p = 17;
        //    BigInteger g = FindPrimitiveRoot(p);

        //    Console.WriteLine($"Primitive root modulo {p} is: {g}");
        //}

        public static BigInteger FindPrimitiveRoot(BigInteger p)
        {
            for (BigInteger g = 2; g < p; g++)
            {
                if (IsPrimitiveRoot(g, p))
                {
                    return g;
                }
            }
            return 5; // No primitive root found -1 bylo 
        }

        static bool IsPrimitiveRoot(BigInteger g, BigInteger p)
        {
            HashSet<BigInteger> seen = new HashSet<BigInteger>();
            for (BigInteger i = 1; i < p; i++)
            {
                BigInteger modExp = ModuloExponentiation(g, i, p);
                if (seen.Contains(modExp))
                {
                    return false;
                }
                seen.Add(modExp);
            }
            return true;
        }

        static BigInteger ModuloExponentiation(BigInteger baseValue, BigInteger exponent, BigInteger modulus)
        {
            BigInteger result = 1;
            baseValue = baseValue % modulus;
            while (exponent > 0)
            {
                if ((exponent % 2) == 1)
                {
                    result = (result * baseValue) % modulus;
                }
                exponent = exponent >> 1;
                baseValue = (baseValue * baseValue) % modulus;
            }
            return result;
        }
    }


}
