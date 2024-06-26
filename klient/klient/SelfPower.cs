﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

// skrypt do potegowania wartosci typu BigInteger
// bo c# ma potegi na double, a ja chce na tym typie miec. 

namespace klient
{
    /// <summary>
    /// Potegowanie ale na zmiennych typu BigInteger
    /// </summary>
    public class SelfPower {
        public static BigInteger Power(BigInteger a, BigInteger n)
        {
            BigInteger result = 1;

            while (n > 0)
            {
                if (n % 2 == 1)
                {
                    result *= a;
                }
                a *= a;
                n >>= 1; // Przesunięcie bitowe o 1 w prawo
            }

            return result;
        }
    }

}
