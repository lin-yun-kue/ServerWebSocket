using PushDataWebSocket.Service;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushDataWebSocket
{
    class Program
    {
        private static WebSocketServer wsServer;
        private static bool IsRun = false;

        static void Main(string[] args)
        {
            try
            {
                wsServer = new WebSocketServer();
                int port = 8080;
                wsServer.Setup(port);
                wsServer.NewSessionConnected += WsServerNewSessionConnected;
                wsServer.NewMessageReceived += WsServerNewMessageReceived;
                wsServer.NewDataReceived += WsServerNewDataReceived;
                wsServer.SessionClosed += WsServerSessionClosed;
                IsRun = wsServer.Start();
                //Start push data to client
                PushData();
                Console.WriteLine($"Server is running on port {port}");
                Console.WriteLine("Press enter exit");
                Console.ReadLine();
                wsServer.Stop();
            }catch(Exception e)
            {

            }
            finally
            {
                wsServer.Stop();
                IsRun = false;
            }
            
        }

        private static void WsServerSessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("SessionClosed");
        }

        private static void WsServerNewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("NewDataReceived");
        }

        private static void WsServerNewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine("NewMessage");
        }

        private static void WsServerNewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("NewSessionConnected");
        }

        private static void PushData()
        {
            var task = Task.Factory.StartNew(() => {
                while (IsRun)
                {
                    Thread.Sleep(1000);
                    //todo: wait api
                    var data = ChainDataService.GetBlockByID(100);
                    var sessions = wsServer.GetAllSessions();
                    sessions.AsParallel().ForAll(x => {

                        x.Send();
                    });
                }
            });
        }
    }
}
