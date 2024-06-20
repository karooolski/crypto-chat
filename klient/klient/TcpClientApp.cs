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
using System.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Net.NetworkInformation;
using System.Net.Http;

// Logka klienta

namespace klient
{
    /*
     W moim programie to wyglada tak
    Alice -> (robi request Encypted chat + wyznacza s) -> Serwer -> Bob
    Bob -> (jeśli się zgodził na encrypted chat : wyznacza p) -> Serwer -> Alice 
    obie strony wyznaczaja ab,AB u siebie
    Alice i Bob wymieniaja się AB
     */

    class TcpClientApp
    {
        public static string lastErrorMessage = "Brak error message"; // zmienna pod forma zeby mogl wyswietlic message box z trescia bledu
        public static TcpClient client;
        static RichTextBox textbox = null; 
        string clientIP = "";
        public static int port = 5000;
        public static NetworkStream gloabalStream = null;
        public static bool connected = false;
        string clientName;
        static string clientNameStatic; 
        string serverIP;
        static System.Net.IPAddress address;  // tez server ip ale w odpowiedniej formie  "192.168.56.1" // 127.0.0.1
        public static bool StopTheClient = false;
        static Thread receiveThread = null;
        static string myIP = "0";
        //public static DiffieHellmanData diffieData = null;
        public static Dictionary<string, DiffieHellmanData> dict = null; // slownik key: "uztykownik", value: "dane do diffie helman"
        public static bool servrBreakup = false;

        public TcpClientApp(string clientName, string _ip_, int portAddr, ref RichTextBox textboxAddr,string _myIP)
        {
            this.clientName = clientName;
            clientNameStatic = clientName;
            this.serverIP = _ip_;
            port = portAddr;
            textbox = textboxAddr;
            textboxAddr.AppendText("[TcpClientapp]: nawiazywanie polacznia\n");
            myIP = _myIP;
            //IPAddress my_ip = GetDefaultGateway();
            //myIP = my_ip.ToString();
;
            main();
        }

        public static DiffieHellmanData getMyDataAbout(string adresat)
        {
            DiffieHellmanData myData = TryGetValueFromDictionary(dict, adresat);

            if (myData != null)
            {
                return myData;
            }

            return null;
        }

        // wypisywanie informacji do textboxa klienta 
        public static void info(string message)
        {
            if (textbox != null)
            {
                try
                {
                    string fullMessage = message + "\n";
                    textbox.Invoke(new Action(delegate ()
                    {
                        textbox.AppendText(Convert.ToString(fullMessage));
                    }));
                } catch (Exception e)
                {
                    lastErrorMessage = e.ToString();
                    return; 
                }
                
            }
        }

        // wyswietlanie w texboxie error messegow 
        public static void sendTxErrorMessage(string message)
        {
            string m = "[ERROR] [TcpClientApp]" + message + "\n";
            info(m);
        }

        public void connectToTheServer(ref TcpClient client, ref bool connected, IPAddress address, int port)
        {
            try
            {
                client.Connect(address, port);
                connected = true;
                string msgtxt = "Połączono z serwerem\n";
                textbox.AppendText(msgtxt);
            }
            catch (Exception e)
            {
                lastErrorMessage = e.ToString();
                sendTxErrorMessage("[2406190008] Nawiazaywanie polaczenia nie powiodlo sie: oto pelna tres bledu: "+e.ToString());
                return;
            }
        }

        public void main()
        {
            try
            {
                address = System.Net.IPAddress.Parse(serverIP);
            }
            catch(Exception e)
            {
                lastErrorMessage = e.ToString();
                MessageBox.Show($"[2406171936] ustawono zle ip: {serverIP} <- !", "Blad!",MessageBoxButtons.OK,MessageBoxIcon.Question);
                return;
            }

            client = new TcpClient();

            connectToTheServer(ref client, ref connected, address, port);

            if(!connected)
            {
                info("[TcpClientApp] nie jestes podlaczony");
                return;
            }

            dict = new Dictionary<string, DiffieHellmanData>();  // klient ma swoj slownik wraz se swoimi wartosciami oraz innyi ludzmi  

            NetworkStream stream = client.GetStream();
            

            gloabalStream = stream;
            
            // wiadomosc inicjalizujaca klienta 
            byte[] nameData = Encoding.ASCII.GetBytes(clientName); 

            try
            {
                stream.Write(nameData, 0, nameData.Length); // Wysyłanie nazwy klienta na serwer
            }
            catch (Exception e)
            {
                MessageBox.Show($"[2406201722] Blad stream.Write() \nFull{e}", "Blad!",MessageBoxButtons.OK, MessageBoxIcon.Question);
                lastErrorMessage = e.ToString();
                return;
            }
            
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
            
            info($"$$$$ [TY]: {messagePort.message} do {messagePort.adresat}: "); // w twoim oknie pojawia się twoja wiadomosc // {clientName}

            //MessagePort messagePort = new MessagePort(clientName, message, adresat, action);
            string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
            byte[] data = Encoding.ASCII.GetBytes(jsonMessage);

            try
            {
                gloabalStream.Write(data, 0, data.Length);
                
            }
            catch (Exception e) {
                lastErrorMessage = e.ToString();
                sendTxErrorMessage($"[2406122113] sendMessage(): nie moge zsapisac nic do globalstream \nFull:{e}");
                gloabalStream.Close();
            }
        }


        /// <summary>
        /// ta funkcja tak strikte dla klienta, jak wysylasz zaszyfrowana wiadomosc to dla ciebie sie pojawia plaintext w texboxie
        /// a na serwer wysylana jest wiadomosc zaszyfrowana
        /// </summary>
        /// <param name="messagePort"></param>
        /// <param name="plainTextMessage"></param>
        public void sendCryptoMessage(MessagePort messagePort, string plainTextMessage) // string message, string adresat, string action
        {
            if (!connected)
            {
                return;
            }
            info($"$$$$ [TY]: {plainTextMessage} do {messagePort.adresat}: "); // w twoim oknie pojawia się twoja wiadomosc // {clientName}
            string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
            byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
            try {
                gloabalStream.Write(data, 0, data.Length);
            }
            catch (Exception e) {
                lastErrorMessage = e.ToString();
                sendTxErrorMessage($"[2406150115] sendMessage(): nie moge zsapisac nic do globalstream {e}");
            }
           // gloabalStream.Flush();
        }



        // nie moge wywolac sendMessage z poziomu statycznej metody (ktora odpowiada za odbieranie wiadomosci) wiec tworze tez statyczna metode do wysylania
        // chodzi o sytuacje w ktorej klient dostal requesta i od razu sam z siebie odsyla wiadomosc : diffie helamnn
        /// <summary>
        /// Statyczna metoda do wysylania wiadomosci z wewnatrz statycznych metod, np. wyslanie wiadomosci przez klienta  zaraz po jej otrzymaniu wewnatrz statycznej metody.
        /// sluzacej do odbierania wiadomosci z serwera.
        /// </summary>
        /// <param name="messagePort"></param>
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
                lastErrorMessage = e.ToString();
                sendTxErrorMessage($"[2406131903] innerSendMessage(): nie moge zsapisac nic do globalstream {e}");
            }
        }


        // a to jest zrobione na potrzebe gdy klient jest aktywny ale serwer zrestartuje, 
        // wtedy zanim klient wysle na serwer wiadomosc, to najpierw wysle mu swoj nick 
        public static void innerSendMessageTextOnly(string myText)
        {
            byte[] data = Encoding.ASCII.GetBytes(myText);
            try
            {
                gloabalStream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                lastErrorMessage = e.ToString();
                sendTxErrorMessage($"[2406190330] innerSendMessage(): nie moge zsapisac nic do globalstream {e}");
            }
        }

        // wyciaganie ze slownika obiektu DiffieHellmanData, klient robi to np. po to zeby zobaczyc czy ma z drugim klientem
        // szyfrowany czat, proba wyciagania danych dzieje za kazdym razem jak ktos wysle do klienta wiadomosc 
        private static DiffieHellmanData TryGetValueFromDictionary(Dictionary<string,DiffieHellmanData> dict, string username)
        {
            try
            {
                DiffieHellmanData data = new DiffieHellmanData();
                data = dict[username];
                return data; 
            } catch(KeyNotFoundException e)
            {
                lastErrorMessage = e.ToString();
                return null; 
            }
        }

        // glowa petla ktora pobiera wiadomsoci przez caly czas dzialania klienta 
        private static Task ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            StringBuilder messageBuilder = new StringBuilder();
            int bytesRead;
            MessagePort messagePort = null;
            string adresat = "";
            bool successReceivingData = false;

            // jakis dziwne error mialem i to niby go roziwazuje 
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0 || !StopTheClient)
                {
                    messageBuilder.Clear();
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(receivedData);
                    string completeMessage = messageBuilder.ToString();
                    int startIndex = completeMessage.IndexOf("<START>"); // Sprawdź, czy wiadomość zawiera oba znaczniki
                    int endIndex = completeMessage.IndexOf("<END>");

                    if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                    {
                        string jsonMessage = completeMessage.Substring(startIndex + 7, endIndex - startIndex - 7);
                        messagePort = JsonSerializer.Deserialize<MessagePort>(jsonMessage);

                        if (messagePort == null)
                        {
                            continue; // nie idz dalej w tej iteracji whilea
                        }

                        string ktoNapisal = messagePort.kto_przesyla;
                        adresat = messagePort.adresat;
                        string numberstr = messagePort.getNumber();
                        BigInteger number = BigInteger.Parse(numberstr);
                        string message = messagePort.message;
                        string action = messagePort.action;

                        successReceivingData = true;

                        // po otrzymaniu wiadomosci od kogos klient patrzy czy ma go w slowniku
                        DiffieHellmanData diffieData;
                        diffieData = TryGetValueFromDictionary(dict, ktoNapisal);

                        if (diffieData == null) // jezeli uzytkownik nie ma danych diffiedata odnosnie innego uzytkownika to go sobie tworzy 
                        {
                            diffieData = new DiffieHellmanData();
                            dict[$"{ktoNapisal}"] = diffieData;
                        }
                        // tuaj wyswieltam wiadomosc w kliencie, otrzymana z serwera od innego klienta
                        if (messagePort.action == "message" && diffieData.allowEncryptedChat == false)
                        {
                            info($"----> [{ktoNapisal}]: {message}");
                        }
                        // w tym przypadku dostales zaszyfrowana wiadomosc // oznacza to tez za musisz miec slownik w ktorym masz klucz prywatny do rozszyfrowania wiadomosci
                        else if (messagePort.action == "message" && diffieData.allowEncryptedChat == true) {
                            string my_private_key = diffieData.getK().ToString();
                            string decrypted = AES.Decrypt(message, my_private_key);
                            info($"----> [{ktoNapisal}]: {decrypted}");
                        }

                        messageBuilder.Remove(0, endIndex + 5); // Usuń przetworzoną wiadomość z bufora
                        
                        // TUTAJ ZACZYNA SIE USTALANIE KLUCZA PRYWANTEGO DIFFIE HELLMAN: (wszystko jest po kolei tak jak klieneci przesylaja miedzy soba)

                        if(messagePort.action == "requestEncryptedChat")
                        {
                            // (klient 1 wyslal wiadomosc z requestem z poziomu buttona wiec kod jego jest w From2.cs)
                            // w tym ifie: klient 2 odbiera wiadomosc od klient1 wraz z akcja requestEncryptedChat

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
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, newAction,myIP);
                                innerSendMessage(answer);
                            }
                            else if (result == DialogResult.Yes)
                            {
                                // klient2 potwierdza ze chce szyfrowany czat 
                                // w tym ifie sa obliczane tez inne zmienne potrzebne do tej metody, az do klucza publicznego 
                                // na koncu tego ifa klien2 liczbe pierwsza zeby klient1 mogl sobie tez zaczac liczyc u siebie

                                info($"Zgodizles sie na szyfrowany czat, wysylasz liczbe pierwsza: {number}");

                                string klient = messagePort.adresat;

                                if(number == 0) // tu byl problem ze BigInteger nie serializowal sie do stringa i wychodzilo 0 
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

                                string newAction = "acceptEncryptedChatRequest";
                                string answermsg = $"{adresat} zgodzil sie na czat szyfrowany, wyliczona podstawa g: {number}";
                                string number_str = number.ToString();
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, newAction, number_str, myIP); // odsylanie liczby pierwszej zpowrotem do ktoNapisal: klien1

                                dict[$"{ktoNapisal}"] = diffieData; // klient2 zapisuje sobie dane do slownika odnosnie klienta1  
                                
                                innerSendMessage(answer);
                            }
                        }
                        else if(messagePort.action == "acceptEncryptedChatRequest") // tutaj klient1 otrzymal od klienta2 potiwerdzenie
                        {
                            // klient1, po tym jak otrzymal zgode od drugiego uzytkownika na polaczenie krypto, oraz dostal od niego z powrotem liczbe pierwsza
                            // teraz ten klient musi obliczyc u siebie tyle ile sie da - az do klucza publicznego, ktorego przesle na koniec tego ifa

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
                            MessagePort shareAB = new MessagePort(adresat, answermsg, ktoNapisal, newAction, ABliczbastr,myIP);

                            dict[$"{ktoNapisal}"] = diffieData; // klient1 zapisuje sobie dane do slownika odnosnie klienta2 

                            innerSendMessage(shareAB);
                        }
                        else if (messagePort.action == "ShareAB") // tutaj klien1 otrzymuje klucz publiczny AB od klienta2 
                        {
                            // klient 2, otrzymal klucz publiczny od klienta 1 ("ABkolegi_str")
                            // teraz moze policzyc on ostatnie eteapy diffie hellman czyli wyznaczyc juz klucz prywatny 
                            // ale jeszcze musi przeslac klientowi1 swoj klucz publiczny, co zrobi na koniec tego ifa

                            string klient = messagePort.adresat; // adkutalny klient to adresat messega jaki otrzymal
                            //info($"{klient} Odebralem: {jsonMessage}");
                            info($"{klient} dostalem od kolegi klucz publiczny = {messagePort.number}");

                            string ABkolegi_str = messagePort.getNumber();
                            BigInteger ABkolegi = BigInteger.Parse(ABkolegi_str);
                            diffieData.setABclient2(ABkolegi);                      // ustawiam w slowniku ze to jest klucz publiczny kolegi co przeslal wlasnie swoj klucz publiczny
                            diffieData.calculateK();                                // olbiczam klucz prywatny 
                            BigInteger K = diffieData.getK();

                            info($"{klient}: Obliczam klucz prywatny: publicznyKolegi^ab mod p = {ABkolegi}^{diffieData.ab} mod {diffieData.p} = {K}");
                            info($"{klient}: Moj klucz prywatny to {K}");

                            string answermsg = $"{adresat} Teraz ja Wysylam swoj klucz publiczny AB: {diffieData.AB}";
                            string newAction = "ShareAB_2";
                            string liczba_str = diffieData.AB.ToString(); ;

                            diffieData.allowEncryptedChat = true; // klient2 oznacza ze bedzie juz szyfrowac wiadomosci

                            dict[$"{ktoNapisal}"] = diffieData; // klient2 zapisuje sobie dane do slownika odnosnie klienta1 

                            MessagePort shareAB_2 = new MessagePort(adresat, answermsg, ktoNapisal, newAction, liczba_str,myIP);
                            innerSendMessage(shareAB_2); // klient2 wysyla swoj klucz publiczny do klienta1 


                        }
                        else if (messagePort.action == "ShareAB_2")
                        {
                            
                            // klient1, po tym jak otrzymal klucz publiczny od klient2,  
                            // teraz on oblicza ostatnie etapy z diffie hellman wraz z kluczem prywantym na koncu

                            string klient = messagePort.adresat; // tutaj wiadomosc z kluczem publicznym klienta2 idzie do klienta1 
                            //info($"{klient} Odebralem: {jsonMessage}");
                            info($"{klient} dostalem od kolegi klucz publiczny = {messagePort.number}");
                            string ABkolegi_str = messagePort.getNumber();
                            BigInteger ABkolegi = BigInteger.Parse(ABkolegi_str);
                            diffieData.setABclient2(ABkolegi);
                            diffieData.calculateK();
                            BigInteger K = diffieData.getK();

                            diffieData.allowEncryptedChat = true;   // teraz klient 1 oznacza ze on tez bedzie szyfrowac wiadomosci

                            dict[$"{ktoNapisal}"] = diffieData; // klient1 zapisuje sobie dane do slownika odnosnie klienta2

                            info($"{klient}: Obliczam klucz prywatny: publicznyKolegi^ab mod p = {ABkolegi}^{diffieData.ab} mod {diffieData.p} = {K}");
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
                lastErrorMessage = e.ToString();
                sendTxErrorMessage($"Nastąpiło rozłączenie z serwerem lub do polaczenia nie doszlo");
                connected = false;
                servrBreakup = true; 
            }

            
            if (successReceivingData)
            {
                string a = adresat;
            }
            return Task.Delay(100);
        }


        // pozyskiwanie ip klienta - default gateway
        public static IPAddress GetDefaultGateway()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
                .FirstOrDefault();
        }

        public static string GetIPV4()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in ipHostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address.ToString();
            }

            return string.Empty;
        }



public static string GetIPV4_2()
    {

            try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("10.0.1.20", 1337); // doesnt matter what it connects to
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return (endPoint.Address.ToString()); //ipv4
            }
        }
        catch (Exception e)
            {
                lastErrorMessage = e.ToString();
                return "failed";
            }
        }

    public static bool checkClientConnectionWithoutTryToLogIn()
        {
            if (client == null)
            {
                return false;
            }
            else if (client.Connected)
            {
                return true;
            }
            return false;
        }
    public static bool IsConnected()
        {
            if(client == null)
            {
                connected = false;
                //client = new TcpClient();
                // bool tryConnectbool = tryConnect();
                return false;

            }
            try
            {
                //client = new TcpClient();
               // client.Connect(address, port);
                if (client.Connected)
                {
                    connected = true;
                    return true;
                }
                else
                {
                    client.Close();
                    bool tryConnectbool = tryConnect();
                    return tryConnectbool;
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.ToString();
                //Console.WriteLine($"Błąd połączenia: {e.Message}");
                connected = false;
                return false;
            }
        }

        // taki potworek skryptowy, gdy proboje wyslac wiadomosc np. to sprawdzam czy mam polaczenie,
        // jezeli nie to musze utworzyc nowa istnanncje TcpClient i sprobowac sie polaczyc z serwerem ponownie
        public static bool tryConnect()
        {
            if (client == null)
            {
                return false;
            }
            try
            {
                client = new TcpClient();
                client.Connect(address, port);

                if (client.Connected)  
                {
                    NetworkStream stream = client.GetStream();
                    gloabalStream = stream;

                    // Wysyłanie nazwy klienta do serwera
                    byte[] nameData = Encoding.ASCII.GetBytes(clientNameStatic);
                    stream.Write(nameData, 0, nameData.Length); // przedstaw sie serwerowi poraz drugi

                    receiveThread = new Thread(() => ReceiveMessages(gloabalStream));
                    receiveThread.Start();

                    connected = true; // jednoczesnie musze poinformowac czy polaczenie sie udalo czy nie 
                    return true;
                }
                else
                {
                    connected = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                lastErrorMessage = "[TcpClient -> tryConnect()\n]" + ex.ToString();
                //Console.WriteLine($"Błąd połączenia: {ex.Message}");
                return false;
            }
        }
        public static bool IsConnected2()
        {
            bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            return networkUp;
        }

    }
}
