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
        //    int p = 17;
        //    int g = FindPrimitiveRoot(p);

        //    Console.WriteLine($"Primitive root modulo {p} is: {g}");
        //}

        public static int FindPrimitiveRoot(int p)
        {
            for (int g = 2; g < p; g++)
            {
                if (IsPrimitiveRoot(g, p))
                {
                    return g;
                }
            }
            return 5; // No primitive root found -1 bylo 
        }

        static bool IsPrimitiveRoot(int g, int p)
        {
            HashSet<int> seen = new HashSet<int>();
            for (int i = 1; i < p; i++)
            {
                int modExp = ModuloExponentiation(g, i, p);
                if (seen.Contains(modExp))
                {
                    return false;
                }
                seen.Add(modExp);
            }
            return true;
        }

        static int ModuloExponentiation(int baseValue, int exponent, int modulus)
        {
            int result = 1;
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
