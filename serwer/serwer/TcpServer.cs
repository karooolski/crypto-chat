using serwer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Dns;
using System.Web;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using System.Runtime.CompilerServices;


class TcpServer
{

    static RichTextBox textbox = null;

    public static List<ClientHandler> clients = new List<ClientHandler>();
    private static readonly object lockObject = new object();
    public string ipServer = "";
    public string defaultGateway = "";
    public int port = 5000;

    public TcpServer(ref RichTextBox textBox1,string _port_)
    {
        textbox = textBox1;
        try
        {
            port = Int32.Parse(_port_);
        }
        catch (Exception e)
        {
            port = 5000;
            info($"-> TcpServer() : Nastapil blad podczas ladowania portu, port zostal ustawiny na 5000!, \npelna tresc bledu: {e}");
        }
    }

    public static void info(string message)
    {
        if (textbox != null)
        {
            //textbox.Text += "[TcpServer] " + message + "\n"; // tak sie nie robi tutaj
            string time = Time.getCurrentTime();
            string fullMessage = $"[TcpServer] {time} -> {message} \n";
                textbox.Invoke(new Action(delegate ()
                {
                    textbox.AppendText(Convert.ToString(fullMessage));
                })); 
        }
    }
    
    // to moze wziac nie to ipv4, np z ethernet 2 co jest zle 
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

    // poprawne odczytywanie IPV4
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
            return "failed get ipv4";
        }
    }

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

    public void StartServer()
    {
        
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        ipServer = GetIPV4_2(); //  GetIPV4
        defaultGateway = GetDefaultGateway().ToString();
        info($"Adres IP Serwera (IPV4) {ipServer}");
        info($"Default Gateway: " + defaultGateway);
        info($"Serwer nasłuchuje na porcie {port}...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            ClientHandler clientHandler = new ClientHandler(client, ref textbox);

            //info($"przypisuje clienta {clientHandler.ClientName}");
            
            lock (lockObject)                                       // zabezpieczenie przed dostepem roznych watkow
            {
                clients.Add(clientHandler);
            }
            
            Thread clientThread = new Thread(clientHandler.Handle); // handlowanie wiadomosci (przechwytywanie) 
            clientThread.Start();
        }
    }

    // tutaj patrze czy mam adresata na liscie polaczonych klientow, jezeli tak to wysylam wiadomosc 
    // jezeli nie daje info do wysylajacego ze adresat nie jest polaczony
    public static void BroadcastMessage(MessagePort messagePort, ClientHandler sender)
    {
        lock (lockObject)
        {
            ClientHandler recipient = clients.Find(c => c.ClientName == messagePort.adresat);
            if (recipient != null)
            {
                recipient.SendMessage(messagePort);
            }
            else
            {
                string adresat = messagePort.adresat;
                string senderName = sender.ClientName;
                string msg = $"[TcpSerwer] Klient {adresat} jest niedostępny";
                string infomsg = $"zwracam informacje dla {senderName}, ze {adresat} jest niedostepny.";
                info(infomsg);
                sender.SendMessage(msg,sender.ClientName, messagePort.myIP);
                //sender.SendMessage($"<START>{{\"Message\":\"Klient '{message.adresat}' nie jest dostępny\",\"Adresat\":\"{sender.ClientName}\",\"KtoPrzesyla\":\"Server\"}}<END>");
            }
        }
    }

    public static void RemoveClient(ClientHandler clientHandler)
    {
        lock (lockObject)
        {
            clients.Remove(clientHandler);
        }
    }
}

class ClientHandler
{
    static RichTextBox textbox = null;

    public TcpClient client;
    private NetworkStream stream;
    private string clientName;
    private bool logedOut = false; 

    public string ClientName { get => clientName; }

    public ClientHandler(TcpClient client, ref RichTextBox tx)
    {
        this.client = client;
        this.stream = client.GetStream();
        textbox = tx; 
    }

    // Funckja do wyswietlania lgoow w textboxie serwera
    public static void info(string message)
    {
        if (textbox != null)
        {
            try
            {
                string time = Time.getCurrentTime();
                string fullMessage = $"[TcpServer] {time} -> {message} \n";
                textbox.Invoke(new Action(delegate ()    // takie cos bo watki kluca sie o referencje do textboxa 
                {
                    textbox.AppendText(Convert.ToString(fullMessage));
                }));
            } catch (Exception e)
            {
                // nic nie rob // nie mam gdzie pokazac w tym miejscuu tego bledu 
            }
        }
    }

    // przechwytywanie wiadomosci 
    public async void Handle()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        this.stream = client.GetStream();
        
        bytesRead = stream.Read(buffer, 0, buffer.Length);

        //string bytesreadstr = Convert.ToBase64String(buffer); // badanie strumienia bajtow przesylanych na serwer
        //info($"odczyt buffera: {bytesreadstr}");
        
        clientName = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim(); // Pobierz nazwę klienta
        StringBuilder messageBuilder = new StringBuilder();

        info($"{clientName} połączony.");

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0) // ta petla dziala caly czas w trakcie dzialania serwera
            {

                // serwer odczytuje wiadomosc 
                string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                messageBuilder.Clear(); // to jest wazne zeby wiadomosci starych nie przesylalo po restarcie serwera
                messageBuilder.Append(receivedData);
                string completeMessage = messageBuilder.ToString();
                int startIndex = completeMessage.IndexOf("<START>"); // Sprawdź, czy wiadomość zawiera oba znaczniki
                int endIndex = completeMessage.IndexOf("<END>");

                if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                {
                    string jsonMessage = completeMessage.Substring(startIndex + 7, endIndex - startIndex - 7); // usuwam znaczniki json z poczatku i z konca
                    
                    // deserializacja wiadomosci od uzytkownika na obiekt w ktorym trzymam dane jakie chce
                    MessagePort message = JsonSerializer.Deserialize<MessagePort>(jsonMessage);

                    // akcja serwera wobec wiadomosci
                    if (message == null) 
                    {
                        info($"message == null");
                        return;
                    }
                    string sender_action = message.action;              // zeby to bylo poprawnie przypisane wazne zeby messagePort mial get; set;

                    info($"Odebrano od {clientName}: \n{jsonMessage}\n");   // pokazuje w texboxie caly json 

                    bool allow_send_forward_message = actionBetweenUsers(sender_action);

                    if (sender_action == "requestEncryptedChat") // to jest na potrzeby pokazania projektu
                    {
                        info(" -------------- TUTAJ SIE ZACZYNA DIFFIE HELLMAN --------------");
                    }
                    if (allow_send_forward_message)                     // tutaj okreslam dozwolone akcje jakie serwer przesyla dalej miedzy uzytkownikami
                    {
                        TcpServer.BroadcastMessage(message, this);      // this czyli ClientHandler, przesyla sam siebie
                    }
                    else if (sender_action == "logout")                 // to jest request od uzytkownika 
                    {
                        messageBuilder.Remove(0, endIndex + 5);
                        client.Close();
                        TcpServer.RemoveClient(this);
                        logedOut = true;
                        break;
                    }
                    else if (!logedOut) // gdy clientHandler != null, czyli wtedy kidy nie wylogowywal sie
                    {
                        messageBuilder.Remove(0, endIndex + 5);             // Usun przetworzona wiadomosc z bufora
                    }
                    else
                    {
                        info("Wychwycilem nieoblsugiwana akcje! ");
                    }
                }
            }
        }
        catch (Exception e)
        {
            info($"[2406130311] Błąd: \n{e.Message}");
        }
        finally
        {
            stream.Close();
            client.Close();
            TcpServer.RemoveClient(this);
            info($"{clientName} rozłączony.");
        }
    }

    public void SendMessage(string message, string adresat, string senderIP)
    {
        MessagePort messagePort = new MessagePort(ClientName, message, adresat, senderIP);
        string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
        byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
        stream.Write(data, 0, data.Length);
    }
    public void SendMessage(MessagePort messagePort)
    {
        string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
        byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
        stream.Write(data, 0, data.Length);
    }

    /// <summary>
    /// dozwolone akcje ktore sa przesylane miedzy klientami np. miedzy klientem a, do klienta b, akcja ktora jest jako action nie dotyczy serwera
    /// a drugiego uzytkownika do ktorego wysylana jest wiadomosc
    /// np. uzytkonik a prosi uzytkownika b o ustanowienie prywatengo czatu
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private bool actionBetweenUsers(string action)
    {
        string [] actions = { "message", "requestEncryptedChat" , "cancelEncryptedChatRequest", "acceptEncryptedChatRequest", "ShareAB", "ShareAB_2"

        };

        foreach (string obj in actions) // patrzye czy przslana akcja zgadza sie z dozwolonymi akcjami
        {
            if (obj == action)
            {
                return true;
            }
        }
        return false;
    }

}
