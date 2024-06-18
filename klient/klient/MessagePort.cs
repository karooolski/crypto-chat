﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace klient
{
    public class MessagePort
    {
        public string message { get; set; }
        public string adresat { get; set; }
        public string kto_przesyla { get; set; }
        public string action { get; set; }
        public string number { get; set; } // json serializer nie serializuje wartosci zmiennoprzecinkowych, dlatego string zeby wartosci zapisywac jako string i przeslyac klient-serwer
        public string myIP { get; set; }

        public MessagePort() {
            message = "None";
            adresat = "None";
            kto_przesyla = "None";
            action = "message";
            number = "10";
            myIP = "none88";
        }

        //public BigInteger getNumber()
        //{
        //    try
        //    {
        //        return BigInteger.Parse(number);
        //    }
        //    catch { return BigInteger.Zero; }
        //}

        public string getNumber()
        {
            return number;
        }

        // to jest do wysylania wiadomosci strike czat 
   


        // to jest do wysylania wiadomosci strike czat 
        public MessagePort(string kto, string message0, string adresat0,string _myIP)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
            action = "message";
            number = "-999";
            myIP = _myIP;

        }
        // to jest do wys wysylania wiadomosci z akcja np. wylogowywanie sie 
        public MessagePort(string kto, string message0, string adresat0, string action0, string _myIP)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
            action = action0;
            number = "-999";
            myIP = _myIP;
        }
        // to jest do diffy hellman wysylanie wiadomosci z liczba  
        public MessagePort(string kto, string message0, string adresat0, string action0, string number0,string _myIP)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
            action = action0;
            number = number0;
            myIP = _myIP;
        }
    }
}
