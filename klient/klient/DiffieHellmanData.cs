using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using klient;

namespace klient
{
    class DiffieHellmanData
    {
        public string you = "not defined";

        public BigInteger p = 0; // wspolna liczba pierwsza == prime number, ustalana miedzy klientami ale u mnie jeden klient ustala a drugi sie zgadza na to
        public BigInteger g = 0; // podsatwa, prymitywny pierwiastek pierwotny modulo p
        public BigInteger ab = 0; // losowa liczna calkowita klient1 ma a, klient2 ma b, ale w implementracji klienta to jedna zmienna , gdzie 1 < ab < p-1
        public BigInteger AB = 0; // klucz publiczny, g^ab, albo inaczej klient1 A = g^a klient2 B = g^b, ale skoro 1 skrypt to obsluguje to nazywam AB
        public BigInteger ABclient2 = 0; // klucz publiczny otrzymuwany od drugiego klienta, z ktorym chce miec czat szyfrowany
        public BigInteger K = 0;  // wspolny tajny klucz K = ABklient2^ab mod p // obaj klienci maja na koniec wyliczony ten sam prywanty klucz którego potem używam jako klucz do SHA256 do AES itd.

        public bool allowCalculate_g = false;
        public bool allowCalculate_ab = false;
        public bool allowCalculateAB = false;
        public static bool allowShareAB = false;
        public bool allowCalculateK = false;

        public bool allowEncryptedChat = false; // ta zmienna jest ustawiana na true dopiero pod koniec procesu uzgadniania klucza obustronnego

        //public void show(string kto)
        //{
        //    Console.WriteLine($"{kto}: p={p} g={g} ab={ab} AB={AB} ABClient2={ABclient2} K={K}");
        //}

        //public void show(string kto)
        //{
        //    Console.WriteLine($"{kto} wybiera liczbe pierwsza p={p} i uzgadania podstawe g={g}");
        //    //Console.WriteLine($"{kto}");
        //    Console.WriteLine($"{kto} wybiera tajna liczbe ab={ab}");
        //    Console.WriteLine($"{kto} wysyla do kolegi AB=g^ab mod p --> AB = {AB}");
        //    Console.WriteLine($"{kto} otrzymalo od kolegi ABClient2={ABclient2}");
        //    Console.WriteLine($"Obliczyl ABkolegi -> ABClient2^ab mod p --> K={K}");
        //}

        public BigInteger getAB()
        {
            return AB;
        }
        public BigInteger get_g()
        {
            return g;
        }
        public BigInteger getK()
        {
            return K;
        }
        public DiffieHellmanData()
        {

        }

        /// <summary>
        /// Ustaw liczbe pierwsza. 
        /// </summary>
        /// <param name="p0"></param>
        public void setPrimeNumber_p(BigInteger p0)
        {
            p = p0;
            allowCalculate_g = true;
        }

        /// <summary>
        /// Oblicz podstawe na podstawie liczby pierwszej.
        /// </summary>
        public void calculateg()
        {
            if (allowCalculate_g)
            {
                g = PrimitiveRoot.FindPrimitiveRoot(p);
                allowCalculate_ab = true;

            }
        }

        /// <summary>
        /// Wyznacz losowa liczbe calkowita, z przedzialu 1 < twoja_liczba < liczba_pierwsza-1.
        /// </summary>
        public void calculateab() // 1 < ab < p-1
        {
            if (allowCalculate_ab)
            {
                Random rnd = new Random();
                BigInteger j = 1;
                BigInteger range = p - 1;
                BigInteger rangeBigInteger = BigInteger.Parse($"{range}");
                ab = (BigInteger)rnd.Next(2, (int)rangeBigInteger);
                allowCalculateAB = true;
            }
        }

        /// <summary>
        /// Wyznacz swoj klucz publiczny - który potem przeslesz osobie z ktora chcesz miec szyfrowany czat.
        /// </summary>
        public void calculateAB()
        {
            if (allowCalculateAB)
            {
                //AB = SelfPower.Power(ab, g);
                AB = SelfPower.Power(g, ab) % p;
                allowShareAB = true;
            }
        }

        /// <summary>
        /// Zapisuje klucz publiczny otrzymany od osoby z ktora chce miec szyfrowany czat.
        /// </summary>
        /// <param name="ABkolegi"></param>
        public void setABclient2(BigInteger ABkolegi)
        {
            ABclient2 = ABkolegi;
            allowCalculateK = true;
        }

        /// <summary>
        /// Obliczam swoj klucz prywanty. 
        /// </summary>
        public void calculateK()
        {
            if (allowCalculateK && allowCalculate_g && allowCalculateAB) // te 2 zmienne na koncu sa ustawiane jak sie wylicza p i ab 
            {
                // K = ABklient2 ^ ab mod p
                BigInteger czlon1 = SelfPower.Power(ABclient2, ab);
                K = czlon1 % p;
            }
        }
    }
}

