using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameClassLibrary.GameObject;
using TestConsole;
using OpenData;
using System.IO;
using System.Net;

namespace GameClassLibrary.ClientServer
{
    public static class Helper
    {
        public static string CreateServeMes(int port, int countPl, int maxCount)
        {
            return $"{GetLocalIP()}:{port} {countPl} {maxCount}";
        }

        public static string GetLocalIP()
        {
            // Получение имени компьютера.
            String host = Dns.GetHostName();
            // Получение ip-адреса.
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            return ip.ToString();
        }

        public static string GetPublicIP()
        {
            String direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                direction = stream.ReadToEnd();
            }

            //Search for the ip in the html
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);

            return direction;
        }

        public static string GetActualGrpc()
        {
            if (File.Exists("actualgrpc.txt"))
                using (StreamReader reader = new StreamReader("actualgrpc.txt"))
                {
                    return reader.ReadLine();
                }
            return "";
        }

        public static void SetActualGrpc(string actual)
        {
            using (StreamWriter writer = new StreamWriter("actualgrpc.txt", false))
            {
                writer.Write(actual);
            }
        }
        public static string[] GetGrpc()
        {
            if (File.Exists("grpc.txt"))
                using (StreamReader reader = new StreamReader("grpc.txt"))
                {
                    var list = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var str = reader.ReadLine();
                        list.Add(str);
                    }
                    return list.ToArray();
                }
            return new[] { "" };
        }


        public static int GetPort()
        {
            if (File.Exists("port.txt"))
                using (StreamReader reader = new StreamReader("port.txt"))
                {
                    string str = reader.ReadLine();
                    int res = 0;
                    if (str != "" && Int32.TryParse(str, out res))
                        return res;
                }
            return 11000;
        }

        public static void SetPort(int port)
        {
            using (StreamWriter writer = new StreamWriter("port.txt", false))
            {
                writer.Write(port);
            }
        }

        public static void SendGrpc(string mes)
        {
            var path = GetActualGrpc();
            try
            {
                GrpcSender.Send(mes, path);
                SetActualGrpc(path);
                return;
            }
            catch (Exception e)
            {

            }

            foreach (var s in GetGrpc())
            {
                try
                {
                    GrpcSender.Send(mes, s);
                    SetActualGrpc(s);
                    return;
                }
                catch (Exception e)
                {

                }
            }
            while (true)
            {
                Console.WriteLine("Введите адрес диспетчера");
                var s = Console.ReadLine();
                try
                {
                    GrpcSender.Send(mes, s);
                    SetActualGrpc(s);
                    return;
                }
                catch (Exception e)
                {

                }
            }
        }
    }

    public enum UserState
    {
        Active,
        Exit,
        Loss,
        Ready
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Server : IServer
    {
        Dictionary<Guid, IClient> names = new Dictionary<Guid, IClient>();
        Dictionary<Guid, IGame> games = new Dictionary<Guid, IGame>();
        Queue<Guid> readyForPlay = new Queue<Guid>();
        private Timer timer;
        private int num;
        private int myPort;

        IClient callback = null;

        public Server()
        {
            TimerCallback callback = new TimerCallback(StartPlay);
            Console.WriteLine("Server started");
            timer = new Timer(callback,num,0, 20000);
            myPort = Helper.GetPort();
        }

        private Guid GetActivePlayerGuid()
        {
            while (true)
            {
                if (readyForPlay.Count > 0)
                {
                    var playerGuid = readyForPlay.Dequeue();
                    if (names.ContainsKey(playerGuid))
                    {
                        try
                        {
                            var playerClient = names[playerGuid];
                            if (!playerClient.Ping())
                                return playerGuid;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    Stop(playerGuid);
                    continue;
                }
                return default(Guid);
            }
        }

        private UserState CheckPlayer(Guid plGuid)
        {
            var pl = GetPlayer(plGuid);
            if (pl == null)
                return UserState.Exit;
            try
            {
                if (pl.Ping())
                    return UserState.Active;
                return UserState.Ready;
            }
            catch (Exception e)
            {
                return UserState.Loss;
            }
        }

        private void Check(Guid sessionid)
        {
            var sess = games[sessionid];
            switch (sess.GetGameState)
            {
                case GameState.Active:
                    var pl = GetPlayer(sess.GetCurrentPlayerGuid);
                    var chRes = CheckPlayer(sess.GetCurrentPlayerGuid);
                    break;
            }
        }

        private void StartPlay(object a)
        {
            while (readyForPlay.Count > 1)
            {
                var player1 = GetActivePlayerGuid();
                var player2 = GetActivePlayerGuid();
                if (player1 != default(Guid) && player2 != default(Guid))
                {
                    var choose = Choose.Get();
                    Guid session = Guid.NewGuid();
                    var game = new GameSession(new[] { player1, player2 }, choose[0], Int32.Parse(choose[1]), Check ,session);
                    games.Add(session, game);
                    Console.WriteLine($"Начата новая игра\n\tИгрок1: {player1}\n\tИгрок2: {player2}");
                    names[player1].Turn(session);
                }
                else
                {
                    if (player1 != default(Guid))
                        readyForPlay.Enqueue(player1);
                    if (player2 != default(Guid))
                        readyForPlay.Enqueue(player2);
                }
            }
        }

        public Guid Start()
        {
            if (names.Count >= 20)
                return default(Guid);
            Guid GUID; 
            try
            {
                GUID = Guid.NewGuid();
                callback = OperationContext.Current.GetCallbackChannel<IClient>();
                Console.WriteLine($"Подключился новый пользователь: {GUID}");
                AddUser(GUID, callback);
                Helper.SendGrpc(Helper.CreateServeMes(myPort, names.Count, 20));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while registering a new user.\n{ex.Message}");
                return default(Guid);
            }

            return GUID;
        }

        public OperationResult Stop(Guid Id)
        {
            if (names.ContainsKey(Id))
            {
                names.Remove(Id);
                Console.WriteLine($"Пользователь {Id} отключился");
                return OperationResult.SuccesOperationResult();
            }

            Console.WriteLine($"Error deleting user.");
            return OperationResult.FailOperationResult("Error deleting user.");
        }

        public void GetGame(Guid userId)
        {
            readyForPlay.Enqueue(userId);
        }

        private IClient GetPlayer (Guid guidPlayer)
        {
            if (names.ContainsKey(guidPlayer))
                return names[guidPlayer];
            return null;
        }

        private void EndGame(Guid sessionGuid)
        {
            var gameSession = games[sessionGuid];
            gameSession.EndGame();
            if (CheckPlayer(gameSession.GetCurrentPlayerGuid) == UserState.Ready)
                GetPlayer(gameSession.GetCurrentPlayerGuid).Turn(sessionGuid);
        }

        public OperationResult MakeTurn(Guid sessionId, Guid userId, int x, int y)
        {
            string mes;
            if (names.ContainsKey(userId))
            {
                var gamer = names[userId];
                if (games.ContainsKey(sessionId))
                {
                    var game = games[sessionId];
                    try
                    {
                        game.MakeTurn(userId, x, y);
                        IClient NextPlayer;
                        switch (game.GetGameState)
                        {
                            case GameState.Active:
                                NextPlayer = GetPlayer(game.GetCurrentPlayerGuid);
                                if (NextPlayer != null)
                                    try
                                    {
                                        NextPlayer.Turn(sessionId);
                                        return OperationResult.SuccesOperationResult();
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                EndGame(sessionId);
                                break;
                            case GameState.End:
                                foreach (Guid gamePlayersGuid in game.GetPlayersGuids)
                                {
                                    NextPlayer = GetPlayer(gamePlayersGuid);
                                    if (NextPlayer != null)
                                        try
                                        {
                                            NextPlayer.Turn(sessionId);
                                        }
                                        catch (Exception e)
                                        {
                                        }
                                }
                                break;
                            case GameState.Unknown:
                                foreach (Guid gamePlayersGuid in game.GetPlayersGuids)
                                {
                                    NextPlayer = GetPlayer(gamePlayersGuid);
                                    if (NextPlayer != null)
                                        try
                                        {
                                            NextPlayer.Turn(sessionId);
                                        }
                                        catch (Exception e)
                                        {
                                        }
                                }
                                break;
                        }
                        return OperationResult.SuccesOperationResult();
                    }
                    catch (Exception ex)
                    {
                        mes = $"Error on make turn:\n\t{ex.Message}";
                    }
                }
                else
                {
                    mes = $"Error non-existent game:\n\t{sessionId}";
                }
            }
            else
            {
                mes = $"Error non-existent user:\n\t{userId}";
            }

            Console.WriteLine(mes);

            return OperationResult.FailOperationResult(mes);
        }

        public string GetGameMap(Guid sessionId)
        {
            string mes = "";
            if (games.ContainsKey(sessionId))
            {
                var game = games[sessionId];
                try
                {
                    return game.GetMapString;
                }
                catch (Exception ex)
                {
                    mes = $"Error on get map string:\n\t{ex.Message}";
                }
            }
            else
            {
                mes = $"Error non-existent game:\n\t{sessionId}";
            }

            Console.WriteLine(mes);
            return "";
        }

        public GameState GetGameState(Guid sessionId)
        {
            string mes = "";
            if (games.ContainsKey(sessionId))
            {
                var game = games[sessionId];
                try
                {
                    return game.GetGameState;
                }
                catch (Exception ex)
                {
                    mes = $"Error on get game state:\n\t{ex.Message}";
                }
            }
            else
            {
                mes = $"Error non-existent game:\n\t{sessionId}";
            }

            Console.WriteLine(mes);
            return GameState.Unknown;
        }

        public Guid GetWinerGuid(Guid Session)
        {
            return games.ContainsKey(Session) ? games[Session].GetWinerGuid : default(Guid);
        }

        /*public OperationResult Ping()
        {
            return OperationResult.SuccesOperationResult();
        }*/

        private void AddUser(Guid guid, IClient callback)
        {
            names.Add(guid, callback);
        }

        public string GetQuestion(Guid idSession)
        {
            if (games.ContainsKey(idSession))
                return games[idSession].GetQestion;
            return "";
        }

        public void SetAnswer(Guid idSession, Guid idPlayer, int answer)
        {
            if (!games.ContainsKey(idSession))
            {
                Console.WriteLine($"Ошибка: {idPlayer} передал ответ к несуществующей сессии {idSession}");
                return;
            }

            games[idSession].SetAnswer(idPlayer, answer);
        }
    }
}
