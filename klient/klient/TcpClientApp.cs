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
        private static void innerSendMessage(MessagePort messagePort)
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
                        
                        if(messagePort == null)
                        {
                            continue; // nie idz dalej w tej iteracji whilea
                        }

                        // tuaj wyswieltam wiadomosc w kliencie, otrzymana z serwera od innego klienta
                        //info($"Odebrano od {message.kto_przesyla}: {message.message}");
                        //info($"odebrana wiadomsc : kto: {messagePort.kto_przesyla}: co: {messagePort.message} akcja: {messagePort.action}");
                        info($"{messagePort.kto_przesyla}: {messagePort.message}");

                        messageBuilder.Remove(0, endIndex + 5); // Usuń przetworzoną wiadomość z bufora
                        adresat = messagePort.adresat;
                        successReceivingData = true;

                        if(messagePort.action == "requestEncryptedChat")
                        {
                            string ktoNapisal = messagePort.kto_przesyla;
                            string infos = $"Uzytkownik {ktoNapisal} prosi ciebie o ustanowienia szyfrowanego czatu";
                            info(infos);
                            string message = infos;
                            const string caption = "Form Closing";
                            var result = MessageBox.Show(message, caption,
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Question);

                            if (result == DialogResult.No)
                            {
                                info("nie zgodziles sie na szyfrowany czat");
                                string answermsg = $"{adresat} nie zgodzil sie na czat szyfrowany";
                                string action = "cancelEncryptedChatRequest";
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, action);
                                innerSendMessage(answer);
                            }
                            else if (result == DialogResult.Yes)
                            {
                                
                                string action = "cancelEncryptedChatRequest";
                                int liczba = PrimeNumberGenerator.generate(10000);
                                info($"Zgodizles sie na szyfrowany czat, wysylasz liczbe pierwsza: {liczba}");
                                string answermsg = $"{adresat} zgodzil sie na czat szyfrowany, otrzymana liczba pierwsza: {liczba}";
                                MessagePort answer = new MessagePort(adresat, answermsg, ktoNapisal, action, liczba);
                                innerSendMessage(answer);
                            }
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
