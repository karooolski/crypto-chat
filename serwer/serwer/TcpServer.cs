using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

// serializowanie obiektow jakie wysylam na serwer 
public class Serializer
{
    public static byte[] serializeObject(Object obj1ect)
    {
        byte[] serializedData = JsonSerializer.SerializeToUtf8Bytes(obj1ect);
        return serializedData;
    }
    public static Object deserializeObject(byte[] serializedObject, Type myClass)
    {
        Object deserializedObject = JsonSerializer.Deserialize(serializedObject, myClass);

        if (deserializedObject != null)
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

class TcpServer
{

    static RichTextBox textbox = null;

    private static List<ClientHandler> clients = new List<ClientHandler>();
    private static readonly object lockObject = new object();

    public TcpServer(ref RichTextBox textBox1)
    {
        textbox = textBox1;
    }

    public static void info(string message)
    {
        if (textbox != null)
        {
            //textbox.Text += "[TcpServer] " + message + "\n";
            string fullMessage = "[TcpServer] " + message + "\n";
            textbox.Invoke(new Action(delegate ()
            {
                textbox.AppendText(Convert.ToString(fullMessage));
            }));
        }
    }


    public void StartServer()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();

        info("Serwer nasłuchuje na porcie 5000...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            ClientHandler clientHandler = new ClientHandler(client, ref textbox);
            lock (lockObject)
            {
                clients.Add(clientHandler);
            }
            Thread clientThread = new Thread(clientHandler.Handle); // handlowanie wiadomosci 
            clientThread.Start();
        }
    }

    public static void BroadcastMessage(MessagePort messagePort, ClientHandler sender)
    {
        lock (lockObject)
        {
            ClientHandler recipient = clients.Find(c => c.ClientName == messagePort.adresat);
            if (recipient != null)
            {
                string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
                //recipient.SendMessage(messagePort.message, recipient.ClientName);
                recipient.SendMessage(messagePort.kto_przesyla, messagePort.message,messagePort.adresat,messagePort.action);

                // Wysłanie potwierdzenia do nadawcy
                //sender.SendMessage($"<START>{{\"Message\":\"Serwer odebrał wiadomość\",\"Adresat\":\"{sender.ClientName}\",\"KtoPrzesyla\":\"Server\"}}<END>");
                //sender.SendBackMessage($"[TcpServer] (potwierdzenie): adresat {recipient.ClientName} == {messagePort.adresat} oderbal wiadomosc");
            }
            else
            {
                string msg = $"[TcpSerwer] Klient {messagePort.adresat} jest niedostępny";
                sender.SendMessage(msg,sender.ClientName);
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

    private TcpClient client;
    private NetworkStream stream;
    private string clientName;

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
                string fullMessage = "[TcpServer] " + message + "\n";
                textbox.Invoke(new Action(delegate ()    // takie cos bo watki kluca sie o textbox 
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

        // Pobierz nazwę klienta
        bytesRead = stream.Read(buffer, 0, buffer.Length);
        clientName = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
        StringBuilder messageBuilder = new StringBuilder();

        info($"{clientName} połączony.");

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0) // ta petla dziala caly czas w trakcie dzialania serwera
            {
                // serwer odczytuje wiadomosc 
                string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                messageBuilder.Append(receivedData);
                string completeMessage = messageBuilder.ToString();
                int startIndex = completeMessage.IndexOf("<START>"); // Sprawdź, czy wiadomość zawiera oba znaczniki
                int endIndex = completeMessage.IndexOf("<END>");

                if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                {
                    string jsonMessage = completeMessage.Substring(startIndex + 7, endIndex - startIndex - 7);
                    
                    // deserializacja wiadomosci od uzytkownika na obiekt w ktorym trzymam dane jakie chce
                    MessagePort message = JsonSerializer.Deserialize<MessagePort>(jsonMessage);
                    
                    info($"Odebrano od {clientName}: {jsonMessage}"); // pokazuje w texboxie caly json 
                    
                    // akcja serwera wobec wiadomosci
                    if (message == null)
                    {
                        return;
                    }
                    if (actionBetweenUsers(message.action)) // to jest zwykla wiaodmosc od uztykownika lub akcja jednego uzytkownika wobec drugiego
                    {
                        TcpServer.BroadcastMessage(message, this); // this czyli ClientHandler, przesyla sam siebie
                    }
                    else if (message.action == "logout") // to jest request od uzytkownika 
                    {
                        client.Close();
                        TcpServer.RemoveClient(this);
                    }
                    messageBuilder.Remove(0, endIndex + 5); // Usun przetworzona wiadomosc z bufora
                }
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine($"Błąd: {e.Message}");
            info($"[2406130311] Błąd: {e.Message}");
        }
        finally
        {
            stream.Close();
            client.Close();
            TcpServer.RemoveClient(this);
            //Console.WriteLine($"{clientName} rozłączony.");
            info($"{clientName} rozłączony.");
        }
    }

    public void SendMessage(string message, string adresat)
    {
        MessagePort messagePort = new MessagePort(ClientName, message, adresat);
        string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
        byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
        stream.Write(data, 0, data.Length);
    }
    public void SendBackMessage(string message)
    {
        MessagePort messagePort = new MessagePort("[Serwer]", message, "[z powrotem]");
        string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
        byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
        stream.Write(data, 0, data.Length);
    }
    public void SendMessage(string kto_przesyla, string wiaodmosc, string adresat, string akcja)
    {
        MessagePort messagePort = new MessagePort(kto_przesyla, wiaodmosc, adresat, akcja);
        string jsonMessage = "<START>" + JsonSerializer.Serialize(messagePort) + "<END>";
        byte[] data = Encoding.ASCII.GetBytes(jsonMessage);
        stream.Write(data, 0, data.Length);
    }


    // dozwolone akcje ktore sa przesylane miedzy klientami np. miedzy klientem a, do klienta b, akcja ktora jest jako action nie dotyczy serwera
    // a drugiego uzytkownika do ktorego wysylana jest wiadomosc
    // np. uzytkonik a prosi uzytkownika b o ustanowienie prywatengo czatu
    private bool actionBetweenUsers(string action)
    {
        if(action == "message" || action == "requestEncryptedChat" || action == "cancelEncryptedChatRequest")
            return true;
        return false;
    }

}

// allowedMessageActionsAsMessages