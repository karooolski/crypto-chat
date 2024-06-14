using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System;
using System.Text.Json;
using System.Xml.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;
using System.Xml.Schema;


namespace klient
{
    // serializowanie obiektow jakie wysylam na serwer 
    public class Serializer {
        public static byte[] serializeObject(Object obj1ect)
        {
            byte[] serializedData = JsonSerializer.SerializeToUtf8Bytes(obj1ect);
            return serializedData;
        }
        public static Object deserializeObject(byte[] serializedObject, Type myClass)
        {
            Object deserializedObject = JsonSerializer.Deserialize(serializedObject, myClass);
            
            if(deserializedObject != null)
            {
                return deserializedObject;
            }

            return null;
        }
    }

   

    /*
     W moim programie to wyglada tak
    Alice -> (robi request Encypted chat + wyznacza s) -> Serwer -> Bob
    Bob -> (jeśli się zgodził na encrypted chat : wyznacza p) -> Serwer -> Alice 
    obie strony wyznaczaja ab,AB u siebie
    Alice i Bob wymieniaja się AB
     */

    class TcpClientApp
    {
        static RichTextBox textbox = null; 
        string clientIP = "";
        int port = 5000;
        static NetworkStream gloabalStream = null;
        bool connected = false;
        string clientName;
        string ip;
        public static bool StopTheClient = false;
        Thread receiveThread = null;
        public static DiffieHellmanData diffieData = null;

        public TcpClientApp(string clientName, string ip, int portAddr, ref RichTextBox textboxAddr)
        {
            this.clientName = clientName;
            this.ip = ip;
            this.port = portAddr;
            textbox = textboxAddr;
            textboxAddr.Text += "[TcpClientapp]: nawiazywanie polacznia\n";
            
            //textbox = new RichTextBox();
            //textbox = textboxAddr;
           main();
        }

        public void stopTheTread()
        {
            StopTheClient = true;
        }

        public static void info(string message)
        {
            if (textbox != null)
            {
                try
                {
                    //textbox.Text += "[TcpServer] " + message + "\n";
                    string fullMessage = message + "\n";
                    textbox.Invoke(new Action(delegate ()
                    {
                        textbox.AppendText(Convert.ToString(fullMessage));
                    }));
                } catch (Exception e)
                {
                    return; 
                }
                
            }
        }

        public static void loginfo(string message)
        {
            if (textbox != null)
            {
                try
                {
                    //textbox.Text += "[TcpServer] " + message + "\n";
                    string fullMessage = "[TcpClientApp] " + message + "\n";
                    textbox.Invoke(new Action(delegate ()
                    {
                        textbox.AppendText(Convert.ToString(fullMessage));
                    }));
                }
                catch (Exception e)
                {
                    return;
                }

            }
        }

        public static void sendTxMessage(string message)
        {
            string m = "[TcpClientApp]" + message + "\n";
            info(m);
        }

        // wyswietlanie w texboxie error messegow 
        public static void sendTxErrorMessage(string message)
        {
            string m = "[ERROR] [TcpClientApp]" + message + "\n";
            info(m);
        }

        public void main()
        {
            if (ip == "")
            {
                clientIP = ip;
            } else
            {
                clientIP = "127.0.0.1";
            }
            if ( port  == 0 )
            {
                port = 5000;
            } 

            System.Net.IPAddress address = System.Net.IPAddress.Parse("127.0.0.1");

            TcpClient client = new TcpClient();

            diffieData = new DiffieHellmanData();

            try
            {
                client.Connect(address, port);
            }
            catch (Exception e)
            {
                sendTxErrorMessage(e.ToString());
                return; 
            }

            
            NetworkStream stream = client.GetStream();
            gloabalStream = stream;
            //Console.WriteLine("Połączono z serwerem.");
            

            connected = true;
            textbox.Text += "Połączono z serwerem\n";
            // Wysyłanie nazwy klienta do serwera
            byte[] nameData = Encoding.ASCII.GetBytes(clientName);
            stream.Write(nameData, 0, nameData.Length);

            receiveThread = new Thread(() => ReceiveMessages(stream));
            receiveThread.Start();

            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    byte[] data = Encoding.ASCII.GetBytes(message);
            //    stream.Write(data, 0, data.Length);
            //}
        }

        public void sendMessage(MessagePort messagePort) // string message, string adresat, string action
        {
            if (!connected)
            {
                return;
            }
            
            info($"$$$$ [TY]: {messagePort.message}"); // w twoim oknie pojawia się twoja wiadomosc // {clientName}

            //MessagePort messagePort = new MessagePort(clientName, message, adresat, action);
            string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
            byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
            
            try
            {
                gloabalStream.Write(data, 0, data.Length);
                //gloabalStream.Close();
            }
            catch (Exception e) {
                sendTxErrorMessage($"[2406122113] sendMessage(): nie moge zsapisac nic do globalstream {e}");
            }
        }

        // nie moge wywolac sendMessage z poziomu statycznej metody wiec tworze tez statyczna metode do wysylania
        // chodzi o sytuacje w ktorej klient dostal requesta i od razu sam z siebie odsyla wiadomosc
        public static void innerSendMessage(MessagePort messagePort)
        {
            string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
            byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
            try
            {
                gloabalStream.Write(data, 0, data.Length);
                //gloabalStream.Close();
            }
            catch (Exception e)
            {
                sendTxErrorMessage($"[2406131903] innerSendMessage(): nie moge zsapisac nic do globalstream {e}");
            }
        }

        private static Task ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            StringBuilder messageBuilder = new StringBuilder();
            int bytesRead;
            MessagePort messagePort = null;
            string adresat = "";
            bool successReceivingData = false;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0 || !StopTheClient)
                {
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(receivedData);

                    string completeMessage = messageBuilder.ToString();

                    // Sprawdź, czy wiadomość zawiera oba znaczniki
                    int startIndex = completeMessage.IndexOf("<START>");
                    int endIndex = completeMessage.IndexOf("<END>");

                    if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                    {
                        string jsonMessage = completeMessage.Substring(startIndex + 7, endIndex - startIndex - 7);
                        messagePort = JsonSerializer.Deserialize<MessagePort>(jsonMessage);

                        

                        string ktoNapisal = messagePort.kto_przesyla;
                        adresat = messagePort.adresat;
                        string numberstr = messagePort.getNumber();
                        BigInteger number = BigInteger.Parse(numberstr);
                        //loginfo($"i jak wyciagam liczbe to number mam taki {number} a w string jest {numberstr}");
                        string message = messagePort.message;
                        string action = messagePort.action;

                        successReceivingData = true;

                        if (messagePort == null)
                        {
                            continue; // nie idz dalej w tej iteracji whilea
                        }

                        // tuaj wyswieltam wiadomosc w kliencie, otrzymana z serwera od innego klienta
                        //info($"Odebrano od {message.kto_przesyla}: {message.message}");
                        //info($"odebrana wiadomsc : kto: {messagePort.kto_przesyla}: co: {messagePort.message} akcja: {messagePort.action}");
                        info($"----> [{ktoNapisal}]: {message}");

                        messageBuilder.Remove(0, endIndex + 5); // Usuń przetworzoną wiadomość z bufora
                        

                        if(messagePort.action == "requestEncryptedChat")
                        {
                            // klient 2 odbiera wiadomosc od klient1 wraz z akcja requestEncryptedChat
                            // (klient 1 wyslal wiadomosc z requestem z poziomu buttona wiec kod jego jest w From2.cs) 

                            info($"(requestEncryptedChat) Odebralem: {jsonMessage}");
                            string infos = $"Uzytkownik {ktoNapisal} prosi ciebie o ustanowienia szyfrowanego czatu";
                            info(infos);
                            string message0 = infos;
                            const string caption = "Form Closing";
                            var result = MessageBox.Show(message0, caption,
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Question);

                            if (result == DialogResult.No)
                            {
                                info("nie zgodziles sie na szyfrowany czat");
                                string answermsg = $"{adresat} nie zgodzil sie na czat szyfrowany";
                                string newAction = "cancelEncryptedChatRequest";
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, newAction);
                                innerSendMessage(answer);
                            }
                            else if (result == DialogResult.Yes)
                            {
                                // klient2 - decyzja yes

                                info($"Zgodizles sie na szyfrowany czat, wysylasz liczbe pierwsza: {number}");
                                //int liczba = PrimeNumberGenerator.generate(10000);
                                //diffieData.setPrimeNumber_p(number); // klient2 ustawia sobie liczbe pierwsza od klienta1 == uzgodnili 
                                // w tym miejscu sa obliczane tez inne zmienne potrzebne do tej metody 

                                string klient = messagePort.adresat;
                                // klient2 potwierdza ze chhce szyufrowany czat i odeslal, liczbe pierwsza 
                                //BigInteger liczba = messagePort.getNumber();

                                if(number == 0)
                                {
                                    info($"{klient}: Cos poszlo nie tak, liczba jaka zem otrzymal to 0");
                                    continue;
                                }

                                diffieData.setPrimeNumber_p(number); // klient2 ustawia sobie liczbe pierwsza i robi reszte obliczen
                                info($"{klient} ustawiam liczbe pierwsza p = {number}");

                                diffieData.calculateg();
                                info($"{klient} uzgadniam podstawe g = {diffieData.g}");

                                diffieData.calculateab();
                                info($"{klient} wybieram tajna liczbe ab = {diffieData.ab}");

                                diffieData.calculateAB();
                                info($"{klient} obliczam klucz publiczny AB=g^ab mod p -->  AB = {diffieData.AB}");


                                //int liczba = PrimitiveRoot.FindPrimitiveRoot(number); // klient2 wylicza podstawe g i przesyla ja do klienta1 
                                string newAction = "acceptEncryptedChatRequest";
                                string answermsg = $"{adresat} zgodzil sie na czat szyfrowany, wyliczona podstawa g: {number}";
                                string number_str = number.ToString();
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, newAction, number_str); // odsylanie liczby pierwszej zpowrotem do ktoNapisal: klien1
                                innerSendMessage(answer);

                                

                            }
                        }
                        else if(messagePort.action == "acceptEncryptedChatRequest") // tutaj klient1 otrzymal od klienta2 potiwerdzenie
                        {
                            // klient1, po tym jak otrzymal zgode od drugiego uzytkownika na polaczenie krypto, oraz dostal od niego z powrotem liczbe pierwsza

                            string klient = messagePort.adresat; 

                            string liczba_str = messagePort.getNumber(); // klient2 potiwerdzil ze chhce szyufrowany czat i odeslal liczbe pierwsza 
                            BigInteger liczba = BigInteger.Parse(liczba_str);

                            diffieData.setPrimeNumber_p(liczba); // klient1 ustawia sobie liczbe pierwsza i robi reszte obliczen
                            info($"{klient} ustawiam liczbe pierwsza p = {liczba}");
                            
                            diffieData.calculateg();
                            info($"{klient} uzgadniam podstawe g = {diffieData.g}");
                            
                            diffieData.calculateab();
                            info($"{klient} wybieram tajna liczbe ab = {diffieData.ab}");

                            diffieData.calculateAB();
                            info($"{klient} obliczam klucz publiczny AB=g^ab mod p -->  AB = {diffieData.AB}");

                            BigInteger ABliczba = diffieData.AB;
                            string ABliczbastr = ABliczba.ToString();
                            string newAction = "ShareAB";
                            string answermsg = $"{adresat}: Wysylam do Ciebie klucz publiczny AB: {ABliczba} do {ktoNapisal}";
                            MessagePort shareAB = new MessagePort(adresat, answermsg, ktoNapisal, newAction, ABliczbastr);
                            innerSendMessage(shareAB);
                        }
                        else if (messagePort.action == "ShareAB") // tutaj klien1 otrzymuje klucz publiczny AB od klienta2 
                        {
                            // klient 2, otrzymal klucz publiczny od klienta 1

                            string klient = messagePort.adresat; // adkutalny klient to adresat messega jaki otrzymal
                            //info($"{klient} Odebralem: {jsonMessage}");
                            info($"{klient} dostalem od kolegi klucz publiczny = {messagePort.number}");

                            string ABkolegi_str = messagePort.getNumber();
                            BigInteger ABkolegi = BigInteger.Parse(ABkolegi_str);
                            diffieData.setABclient2(ABkolegi);
                            diffieData.calculateK();
                            BigInteger K = diffieData.getK();

                            info($"{klient}: Obliczam klucz prywatny: publicznyKolegi^ab mod p = {ABkolegi}^{diffieData.ab} mod {diffieData.p} = {K}");
                            info($"{klient}: Moj klucz prywatny to {K}");

                            string answermsg = $"{adresat} Teraz ja Wysylam swoj klucz publiczny AB: {diffieData.AB}";
                            string newAction = "ShareAB_2";
                            string liczba_str = diffieData.AB.ToString(); ;

                            MessagePort shareAB_2 = new MessagePort(adresat, answermsg, ktoNapisal, newAction, liczba_str);
                            innerSendMessage(shareAB_2); // klient2 wysyla swoj klucz publiczny do klienta1 


                        }
                        else if (messagePort.action == "ShareAB_2")
                        {
                            
                            // klient1, po tym jak otrzymal klucz publiczny od klient2,  

                            string klient = messagePort.adresat; // tutaj wiadomosc z kluczem publicznym klienta2 idzie do klienta1 
                            //info($"{klient} Odebralem: {jsonMessage}");
                            info($"{klient} dostalem od kolegi klucz publiczny = {messagePort.number}");
                            string ABkolegi_str = messagePort.getNumber();
                            BigInteger ABkolegi = BigInteger.Parse(ABkolegi_str);
                            diffieData.setABclient2(ABkolegi);
                            diffieData.calculateK();
                            BigInteger K = diffieData.getK();
                            info($"{klient}: Obliczam klucz prywatny: publicznyKolegi^ab mod p = {ABkolegi}^{diffieData.ab} mod {diffieData.p} = {K}");
                            info($"{klient}: Moj klucz prywatny to {K}");
                        }
                        //else if(messagePort.action == "sendPublicKey")
                        //{
                        //    // moge juz tez obliczyc i wyslac od razu obliczony klucz publiczny
                        //    BigInteger ABliczba = diffieData.AB;
                        //    string ABliczba_str = ABliczba.ToString();
                        //    answermsg = $"{adresat} przesylam tez AB: {ABliczba}";
                        //    newAction = "ShareAB";
                        //    MessagePort shareAB = new MessagePort(adresat, answermsg, ktoNapisal, newAction, ABliczba_str);
                        //    innerSendMessage(shareAB);
                        //}


                    }
                    if (klient.StopClient.stopClient) // statyczna zmienna ktora aktywuje sie przy wcisnieciu wylogowywania
                    {
                        info("Aborting thread :) ");
                        Thread.ResetAbort();
                        break;
                    }
                }
            } catch (Exception e)
            {
                sendTxErrorMessage($" [2406122129] ReceiveMessages() problem z whilem do receive messages : Blad->> {e}");
            }

            
            if (successReceivingData)
            {
                string a = adresat;
            }
            return Task.Delay(100);
        }
    }
}
