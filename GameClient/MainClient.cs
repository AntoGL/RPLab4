using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel.Security;
using GameClient.GameService;

namespace GameClient
{
    public delegate void ReceviedMessage(string message);
    [CallbackBehaviorAttribute(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientGame : GameService.IServerCallback
    {
        InstanceContext inst = null;
        private ServerClient Server;
        private Guid ClientGuid;
        char XorO;
        bool firstturn = true;
        Guid idGame;
        public bool isStopped = false;
        public void Start(ClientGame rc, string remoteAdress)
        {
            inst = new InstanceContext(rc);
            Server = new ServerClient(inst, new NetTcpBinding(SecurityMode.None), new EndpointAddress(remoteAdress));
            ClientGuid = Server.Start();
            Server.GetGame(ClientGuid);
            isStopped = false;
        }

        public void Stop()
        {
            Server.Stop(ClientGuid);
            isStopped = true;
        }

        public void Turn(Guid sessionGuid)
        {
            idGame = sessionGuid;
            Console.WriteLine("Your turn");
            var state = Server.GetGameState(idGame);
            if (state == GameService.GameState.Active)
            {
                var map = Server.GetGameMap(idGame);
                if (firstturn)
                {
                    XorO = map == "000000000" ? 'X' : 'O';
                    firstturn = false;
                }
                int[] coord = GameControl(XorO, map.ToArray());
                Server.MakeTurn(idGame, ClientGuid, coord[0], coord[1]);
            }
            else
            {
                
                Console.WriteLine("The winner is ");
            }
        }

        public bool Ping()
        {
            return true;
        }
        
        public int[] GameControl( char sym, char[] places)
        {
            Console.WriteLine("Вы играете за " + sym);
            for (int i = 0; i < 9; i++)
            {
                char currentPos = places[i] == '2' ? 'X' : places[i] == '1' ? 'O' : '0';
                Console.Write(currentPos + "(" + i + ") ");
                if (i % 3 == 2)
                    Console.WriteLine();
            }
            Console.WriteLine("Введите позицию: ");

            int position;

            while (true)
            {
                position = Convert.ToInt16(Console.ReadLine());
                if (places[position] != '1' || places[position] != '2')
                    break;
                else
                    Console.WriteLine("Неверная позиция, выберите другую: ");
            }

            places[position] = sym == 'X' ? '2' : '1';
            for (int i = 0; i < 9; i++)
            {
                char currentPos = places[i] == '2' ? 'X' : places[i] == '1' ? 'O' : '0';
                Console.Write(currentPos + " ");
                if (i % 3 == 2)
                    Console.WriteLine();
            }
            return new int[2] { position / 3, position % 3 };
        }

        public string GetUserName()
        {
            throw new NotImplementedException();
        }
    }
    


    class Program
    {
        static Dictionary<string, int> serverIpList;
        static string SendMessageFromSocket(string host, int port, string message)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPAddress ipAddr;
            if (!IPAddress.TryParse(host, out ipAddr))
                ipAddr = Dns.GetHostEntry(host).AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            // Соединяем сокет с удаленной точкой
            try
            {
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(ipEndPoint);

                Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
                byte[] msg = Encoding.UTF8.GetBytes(message);

                // Отправляем данные через сокет
                int bytesSent = sender.Send(msg);

                // Получаем ответ от сервера
                int bytesRec = sender.Receive(bytes);

                sender.Shutdown(SocketShutdown.Both);
                sender.Dispose();
                sender.Close();

                return Encoding.UTF8.GetString(bytes, 0, bytesRec);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                return "";
            }

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            //if (message.IndexOf("<TheEnd>") == -1)
            //  SendMessageFromSocket(host, port);

            // Освобождаем сокет
        }
        
        static void Main(string[] args)
        {
            /*int primaryServer;
            serverIpList = new Dictionary<string, int>();
            string IP = "46.146.54.1"; //ip-адрес, к которому подключаемся. 
            int PORT = 11000; //на этом порту слушает сокет
           
            string ip = SendMessageFromSocket(IP, PORT, "Get over here");
            if (ip != "")
            {
                primaryServer = Convert.ToInt32(ip[0]);
                string[] adreses = ip.Split(' ');
                for (int i = 0; i < adreses.Length; i++)
                    serverIpList.Add(adreses[i].Split(':')[0], Convert.ToInt32(adreses[i].Split(':')[1]));
                for (int i = 0; i < serverIpList.Count; i++)
                    foreach (KeyValuePair<string, int> keyValue in serverIpList)
                    {
                        Console.WriteLine(keyValue.Key + ":" + keyValue.Value);
                    }
            }
            else
                Console.WriteLine("Данные не получены");
            Console.WriteLine("Hello, welcome to the Game");
            Console.WriteLine("If you wanna play, just choose one of server");
            Console.ReadLine();
            Console.WriteLine("Ok, let's go");*/
            var Client = new ClientGame();
            int port = 11000;
            Client.Start(Client, $"net.tcp://localhost:{port}/Server/GameServer");
            //Client.Turn();
            while(!Client.isStopped)
            {
                Client.Ping();
            }
            Console.ReadKey();
        }
    }
}
