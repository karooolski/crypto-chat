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

        public MessagePort() { }

        public MessagePort(string kto, string message0, string adresat0)
        {
            message = message0;
            adresat = adresat0;
            kto_przesyla = kto;
        }
    }



    class TcpClientApp
    {
        static RichTextBox textbox = null; 
        string clientIP = "";
        int port = 5000;
        NetworkStream gloabalStream = null;
        bool connected = false;
        string clientName;
        string ip;

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

        public static void info(string message)
        {
            if (textbox != null)
            {
                //textbox.Text += "[TcpServer] " + message + "\n";
                string fullMessage = "[TcpClientApp] " + message + "\n";
                try
                {
                    textbox.Invoke(new Action(delegate ()
                    {
                        textbox.AppendText(Convert.ToString(fullMessage));
                    }));
                } catch (Exception e)
                {
                    
                }
                
            }
        }

        public static void sendTxMessage(string message)
        {
            string m = "[TcpClientApp]" + message + "\n";
            info(m);
        }

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

            ///Console.Write("Podaj swoją nazwę: ");
            //string clientName = Console.ReadLine();

            TcpClient client = new TcpClient();

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

            Thread receiveThread = new Thread(() => ReceiveMessages(stream));
            receiveThread.Start();

            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    byte[] data = Encoding.ASCII.GetBytes(message);
            //    stream.Write(data, 0, data.Length);
            //}
        }

        public void sendMessage(string message, string adresat)
        {
            if (!connected)
            {
                return;
            }
            //MessagePort m = new MessagePort(clientName,message, "abdullah");

            info($"{clientName}: {message}");
            MessagePort messagePort = new MessagePort(clientName, message, adresat);
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

        private static Task ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            StringBuilder messageBuilder = new StringBuilder();
            int bytesRead;
            MessagePort message = null;
            string adresat = "";
            bool successReceivingData = false;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

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
                        message = JsonSerializer.Deserialize<MessagePort>(jsonMessage);
                        info($"Odebrano od {message.kto_przesyla}: {message.message}");
                        messageBuilder.Remove(0, endIndex + 5); // Usuń przetworzoną wiadomość z bufora
                        adresat = message.adresat;
                        successReceivingData = true;
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
