using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace klient
{
    public class SelfPower {
        public static int Power(int a, int n)
        {
            int result = 1;

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
