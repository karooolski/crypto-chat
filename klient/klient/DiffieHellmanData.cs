using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Numerics;
//using klient; https://en.wikipedia.org/wiki/Diffie%E2%80%93Hellman_key_exchange

namespace klient
{
    /// <summary>
    /// To jest klasa ktora przechowuje i przetwarza dane potrzebne do metody diffie hellman.
    /// Kazdy klient ma liste obiketow tej klasy (w TcpClientApp.cs), dzieki temu np. Alice ma z Bobem uzgodniony klucz prywatny, natomiast Alice z Piotrem ma inny klucz prywatny.  
    /// </summary>
    class DiffieHellmanData
    {
        public string you = "not defined";

        public BigInteger p = 0; // wspolna liczba pierwsza == prime number, ustalana miedzy klientami ale u mnie jeden klient ustala a drugi sie zgadza na to
        public BigInteger g = 0; // podsatwa, prymitywny pierwiastek pierwotny modulo p
        public BigInteger ab = 0; // losowa liczna calkowita klient1 ma a, klient2 ma b, ale w implementracji klienta to jedna zmienna , gdzie 1 < ab < p-1
        public BigInteger AB = 0; // klucz publiczny, g^ab, albo inaczej klient1 A = g^a klient2 B = g^b, ale skoro 1 skrypt to obsluguje to nazywam AB
        public BigInteger ABclient2 = 0; // klucz publiczny otrzymuwany od drugiego klienta, z ktorym chce miec czat szyfrowany
        public BigInteger K = 0;  // wspolny tajny klucz K = ABklient2^ab mod p // obaj klienci maja na koniec wyliczony ten sam prywanty klucz którego potem używam jako klucz do SHA256 do AES itd.

        // boole zabezpieczajace robienie operacji uzgadniania klucza po kolei 
        public bool allowCalculate_g = false;
        public bool allowCalculate_ab = false;
        public bool allowCalculateAB = false;
        public static bool allowShareAB = false;
        public bool allowCalculateK = false;

        public bool allowEncryptedChat = false; // ta zmienna jest ustawiana na true dopiero pod koniec procesu uzgadniania klucza prywatnego diffie hellman. 

        /// <summary>
        /// Zwraca twoj klucz publiczny. 
        /// </summary>
        /// <returns></returns>
        public BigInteger getAB()
        {
            return AB;
        }
        /// <summary>
        /// Zwraca twoja podstawe policzona na podstawie poczakowo wyznaczonej liczby pierwszej. 
        /// </summary>
        /// <returns></returns>
        public BigInteger get_g()
        {
            return g;
        }
        /// <summary>
        /// Zwraca twoj klucz prywatny. 
        /// </summary>
        /// <returns></returns>
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
        public void calculateK() // K = ABklient2 ^ ab mod p --> czyli (klucz_publiczny_klienta_2)^(twoja_losowa_liczba) mod (liczba_pierwsza) 
        {
            if (allowCalculateK && allowCalculate_g && allowCalculateAB) // te 2 zmienne na koncu sa ustawiane jak sie wylicza p i ab 
            {
                BigInteger czlon1 = SelfPower.Power(ABclient2, ab);
                K = czlon1 % p;
            }
        }
    }
}

