using TestConsoleServer;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using GameClassLibrary.ClientServer;
using TestConsole;
using System.Collections.Generic;

namespace GameServerService
{
    class Program
    {
        private static string GetLocalIP()
        {
            // Получение имени компьютера.
            String host = Dns.GetHostName();
            // Получение ip-адреса.
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            return ip.ToString();
        }

        private static string GetPublicIP()
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

        static string GetActualGrpc()
        {
            if (File.Exists("actualgrpc.txt"))
                using (StreamReader reader = new StreamReader("actualgrpc.txt"))
                {
                    return reader.ReadLine();
                }
            return "";
        }

        static void SetActualGrpc(string actual)
        {
            using (StreamWriter writer = new StreamWriter("actualgrpc.txt", false))
            {
                writer.Write(actual);
            }
        }
        static string[] GetGrpc()
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


        static int GetPort()
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

        static void SetPort(int port)
        {
            using (StreamWriter writer = new StreamWriter("port.txt", false))
            {
                writer.Write(port);
            }
        }

        static void Main(string[] args)
        {
            int port = GetPort();

            try
            {
                ServiceHost host = null;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        SetPort(port + i);
                        Console.WriteLine($"Попытка подключиться к порту: {port + i}");
                        host = new ServiceHost(typeof(Server), new Uri($"net.tcp://localhost:{port + i}/Server/"), new Uri($"http://localhost:{port + 11 + i}/Server/"));
                        host.AddServiceEndpoint(typeof(IServer), new NetTcpBinding(SecurityMode.None), "GameServer");

                        var metaData = new ServiceMetadataBehavior { HttpGetEnabled = true };
                        host.Description.Behaviors.Add(metaData);
                        host.Authentication.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

                        host.Open();
                        Console.WriteLine("Подключение успешно");
                        port += i;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Неудалось подключиться к порту: {port + i}\nОшибка: {ex.Message}");
                    }
                }
                SendGrpc($"{GetLocalIP()}:{port}");
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.ReadLine();
                host.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.ReadKey();
            }
        }

        private static void SendGrpc(string mes)
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
            while(true)
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
}
