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

    public class MessagePort
    {
        public string message { get; set; }
        public string adresat { get; set; }
        public string kto_przesyla { get; set; }
        public string action { get; set; }
        public int number { get; set; }

        public MessagePort() { }

        // to jest do wysylania wiadomosci strike czat 
        public MessagePort(string kto, string message0, string adresat0)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
            action = "message";
            number = 0;
        }
        // to jest do wys wysylania wiadomosci z akcja np. wylogowywanie sie 
        public MessagePort(string kto, string message0, string adresat0, string action0)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
            action = action0;
            number = 0;
        }
        // to jest do diffy hellman wysylanie wiadomosci z liczba  
        public MessagePort(string kto, string message0, string adresat0, string action0, int number0)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
            action = action0;
            number = number0;
        }
    }

    class DiffieHellmanData
    {
        int p = 0; // wspolna liczba pierwsza == prime number, ustalana miedzy klientami ale u mnie jeden klient ustala a drugi sie zgadza na to
        int g = 0; // podsatwa, prymitywny pierwiastek pierwotny modulo p
        int ab = 0; // losowa liczna calkowita klient1 ma a, klient2 ma b, ale w implementracji klienta to jedna zmienna , gdzie 1 < ab < p-1
        int AB = 0; // klucz pibliczny, g^ab, albo inaczej klient1 A = g^a klient2 B = g^b, ale skoro 1 skrypt to obsluguje to robie AB
        int ABclient2 = 0; // klucz publiczny otrzymuwany od drugiego klienta
        int K = 0;  // wspolny tajny klucz K = ABklient2^ab mod p

        private bool allowCalculate_g = false;
        private bool allowCalculate_ab = false;
        private bool allowCalculateAB = false;
        private static bool allowShareAB = false;
        private bool allowCalculateK = false;

        public int getAB()
        {
            return AB; 
        }
        public int get_g()
        {
            return g;
        }
        public int getK()
        {
            return K;
        }
        public DiffieHellmanData() { 
        
        }

        public void setPrimeNumber_p(int p0)
        {
            p = p0;
            allowCalculate_g = true;
            calculateg(); // podczas ustawiania p moge sobie policzyc wszystko co moge na teraz policzyc przed wysylaniem wiadomosci 
        }
        public void calculateg()
        {
            if (allowCalculate_g)
            {
                g = PrimitiveRoot.FindPrimitiveRoot(p);
                allowCalculate_ab = true;
                calculateab();
            }          
        }
        public void calculateab() // 1 < ab < p-1
        {
            if (allowCalculate_ab)
            {
                Random rnd = new Random();
                int j = 1;
                int range = p - 1;
                int rangeint = int.Parse($"{range}");
                ab = (int) rnd.Next(2, rangeint);
                allowCalculateAB = true;
                calculateAB();
            }
        }
        public void calculateAB()
        {
            if (allowCalculateAB)
            {
                AB = SelfPower.Power(ab, g);
                allowShareAB = true;
            }
        }
        public static void shareAB(MessagePort data)
        {
            if (allowShareAB)
            {
                TcpClientApp.innerSendMessage(data);
            }
        }
        public void setABclient2(int ABkolegi)
        {
            ABclient2 = ABkolegi;
            allowCalculateK = true;
        }
        public void calculateK()
        {
            if (allowCalculateK && allowCalculate_g && allowCalculateAB) // te 2 zmienne na koncu sa ustawiane jak sie wylicza p i ab 
            {
                // K = ABklient2 ^ ab mod p
                int czlon1 = SelfPower.Power(ABclient2, ab);
                K = czlon1 % p;  
            }
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
                    string fullMessage = "[TcpClientApp] " + message + "\n";
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

            info($"{clientName}: {messagePort.message}");

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
                        int number = messagePort.number;
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
                        info($"{ktoNapisal}: {message}");

                        messageBuilder.Remove(0, endIndex + 5); // Usuń przetworzoną wiadomość z bufora
                        

                        if(messagePort.action == "requestEncryptedChat")
                        {
                            
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
                                //int liczba = PrimeNumberGenerator.generate(10000);
                                diffieData.setPrimeNumber_p(number); // klient2 ustawia sobie liczbe pierwsza od klienta1 == uzgodnili 
                                // w tym miejscu sa obliczane tez inne zmienne potrzebne do tej metody 
                                
                                //int liczba = PrimitiveRoot.FindPrimitiveRoot(number); // klient2 wylicza podstawe g i przesyla ja do klienta1 
                                string newAction = "acceptEncryptedChatRequest";
                                info($"Zgodizles sie na szyfrowany czat, wysylasz liczbe pierwsza: {number}");
                                string answermsg = $"{adresat} zgodzil sie na czat szyfrowany, wyliczona podstawa g: {number}";
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, newAction, number); // odsylanie zpowrotem do ktoNapisal
                                innerSendMessage(answer);

                                // moge juz tez obliczyc i wyslac od razu obliczony klucz publiczny
                                int ABliczba = diffieData.getAB();
                                answermsg = $"{adresat} przesylam tez AB: {ABliczba}";
                                newAction = "ShareAB";
                                MessagePort shareAB = new MessagePort(adresat, answermsg, ktoNapisal, newAction, ABliczba);
                                innerSendMessage(shareAB);

                            }
                        }
                        else if(messagePort.action == "acceptEncryptedChatRequest") // tutaj klient1 otrzymal od klienta2 potiwerdzenie
                        {
                            // klient2 potwierdza ze chhce szyufrowany czat i odeslal, liczbe pierwsza 
                            int liczba = messagePort.number;
                            diffieData.setPrimeNumber_p(liczba); // klient1 ustawia sobie liczbe pierwsza i robi reszte obliczen
                            int ABliczba = diffieData.getAB();
                            string newAction = "ShareAB";
                            string answermsg = $"{adresat} Przesylam AB: {ABliczba}";
                            MessagePort shareAB = new MessagePort(adresat, answermsg, ktoNapisal, newAction, liczba);
                            innerSendMessage(shareAB);
                        }
                        else if (messagePort.action == "ShareAB") // tutaj klien1 otrzymuje klucz publiczny AB od klienta2 
                        {
                            // w tych linijkach znajdzie sie zarowno klient1 i klient2, gdy sie wymieniaja kluczem publicznym
                            int ABkolegi = messagePort.number;
                            diffieData.setABclient2(ABkolegi);
                            diffieData.calculateK();
                            string klient = messagePort.adresat; // adkutalny klient to adresat messega jaki otrzymal
                            int K = diffieData.getK();
                            info($"{klient}: Moj klucz prywatny to {K}");
                        }



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
