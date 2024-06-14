using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace klient
{
    public class PrimeNumberGenerator
    {
        public PrimeNumberGenerator()
        {

        }

        public static BigInteger generate(BigInteger range)
        {
            Random rnd = new Random();
            BigInteger randomVal = rnd.Next(2, (int)range);
            while (!isPrime(randomVal))
            {
                randomVal = rnd.Next(1, (int)range);
            }
            return (BigInteger)randomVal;
        }

        // Checks whether the provided number is a prime number.
        public static bool isPrime(BigInteger num)
        {
            if (num <= 1)
                return false; // 1 or less is never prime.
            if (num == 2)
                return true; // 2 is always a prime number.

            // Trial Division: Tries to divide the number with all numbers in the range 2 to square root of the number.
            var boundary = (BigInteger)Math.Floor(Math.Sqrt((int)num));
            for (BigInteger i = 2; i <= boundary; i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }

    }
}
