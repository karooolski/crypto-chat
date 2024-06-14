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
        public BigInteger p = 0; // wspolna liczba pierwsza == prime number, ustalana miedzy klientami ale u mnie jeden klient ustala a drugi sie zgadza na to
        public BigInteger g = 0; // podsatwa, prymitywny pierwiastek pierwotny modulo p
        public BigInteger ab = 0; // losowa liczna calkowita klient1 ma a, klient2 ma b, ale w implementracji klienta to jedna zmienna , gdzie 1 < ab < p-1
        public BigInteger AB = 0; // klucz pibliczny, g^ab, albo inaczej klient1 A = g^a klient2 B = g^b, ale skoro 1 skrypt to obsluguje to robie AB
        public BigInteger ABclient2 = 0; // klucz publiczny otrzymuwany od drugiego klienta
        public BigInteger K = 0;  // wspolny tajny klucz K = ABklient2^ab mod p

        public bool allowCalculate_g = false;
        public bool allowCalculate_ab = false;
        public bool allowCalculateAB = false;
        public static bool allowShareAB = false;
        public bool allowCalculateK = false;

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

        public void setPrimeNumber_p(BigInteger p0)
        {
            p = p0;
            allowCalculate_g = true;
           //calculateg(); // podczas ustawiania p moge sobie policzyc wszystko co moge na teraz policzyc przed wysylaniem wiadomosci 
        }
        public void calculateg()
        {
            if (allowCalculate_g)
            {
                g = PrimitiveRoot.FindPrimitiveRoot(p);
                allowCalculate_ab = true;
                //calculateab();
            }
        }
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
                //calculateAB();
            }
        }
        public void calculateAB()
        {
            if (allowCalculateAB)
            {
                //AB = SelfPower.Power(ab, g);
                AB = SelfPower.Power(g, ab) % p;
                allowShareAB = true;
            }
        }
        //public static void shareAB(MessagePort data)
        //{
        //    if (allowShareAB)
        //    {
        //        TcpClientApp.innerSendMessage(data);
        //    }
        //}
        public void setABclient2(BigInteger ABkolegi)
        {
            ABclient2 = ABkolegi;
            allowCalculateK = true;
            //calculateK();
        }
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

