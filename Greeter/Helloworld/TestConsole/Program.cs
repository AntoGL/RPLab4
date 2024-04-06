using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;
namespace TestConsole
{
    public class GrpcSender
    {
        public static void Send(string mes, string reciver = "46.146.54.1:13000")
        {
            Channel channel = new Channel(reciver, ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = mes });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    class Program
    {
        static void Main(string[] arg)
        {
            Channel channel = new Channel("", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = "UserName" });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
