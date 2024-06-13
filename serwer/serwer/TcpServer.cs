using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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

    public MessagePort() { }

    public MessagePort(string kto, string message0, string adresat0)
    {
        message = message0;
        adresat = adresat0;
        kto_przesyla = kto;
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

    //static void Main(string[] args)
    public void StartServer()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        //Console.WriteLine("Serwer nasłuchuje na porcie 5000...");
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

    public static void BroadcastMessage(MessagePort message, ClientHandler sender)
    {
        info("[Serwer : Wchodze do Broadcast Message]");
        lock (lockObject)
        {
            ClientHandler recipient = clients.Find(c => c.ClientName == message.adresat);
            if (recipient != null)
            {
                string jsonMessage = "<START>" + JsonSerializer.Serialize(message) + "<END>";

                recipient.SendMessage(jsonMessage);

                sender.SendMessage("[Serwer] Adresat odebral wiadomosc");
                // Wysłanie potwierdzenia do nadawcy
                //sender.SendMessage($"<START>{{\"Message\":\"Serwer odebrał wiadomość\",\"Adresat\":\"{sender.ClientName}\",\"KtoPrzesyla\":\"Server\"}}<END>");
            }
            else
            {
                sender.SendMessage("[Serwer] odbiorca byl null");
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


    public async void Handle()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        // Pobierz nazwę klienta
        bytesRead = stream.Read(buffer, 0, buffer.Length);
        clientName = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
        StringBuilder messageBuilder = new StringBuilder();
        //MessagePort data; 
        //byte[] bufferRead = stream.Read(buffer, 0, buffer.Length);
        //data = Serializer.deserializeObject(serializedObject: bufferRead, myClass: typeof(MessagePort));

        //BinaryFormatter formatter = new BinaryFormatter();
        //MessagePort receivedObject = (MessagePort)await formatter.Deserialize(stream);
        //info($"odebralem od kogo: {receivedObject.adresat} message: {receivedObject.message} adresat: {receivedObject.adresat}");

        //Console.WriteLine($"{clientName} połączony.");
        info($"{clientName} połączony.");

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
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
                    MessagePort message = JsonSerializer.Deserialize<MessagePort>(jsonMessage);

                    info($"Odebrano od {clientName}: {jsonMessage}");

                    TcpServer.BroadcastMessage(message, this);

                    // Usuń przetworzoną wiadomość z bufora
                    messageBuilder.Remove(0, endIndex + 5);
                }
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine($"Błąd: {e.Message}");
            info($"Błąd: {e.Message}");
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

    public void SendMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }
}