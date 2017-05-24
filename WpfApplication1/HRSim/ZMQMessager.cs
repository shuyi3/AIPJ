using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace HRSim
{
    class ZMQMessager
    {
        private ZSocket requester;
        private static ZMQMessager instance;

        private ZMQMessager()
        {
            Init();
        }

        public static ZMQMessager Instance
        {
            get
            {
                return instance ?? (instance = new ZMQMessager());
            }
        }

        public void Init()
        {
            Connect();
        }

        public string sendMessage(string message)
        {
            string result = null;
            using (var requester = new ZSocket(ZSocketType.REQ))
            {
                // Connect
                requester.Connect("tcp://127.0.0.1:5556");

                for (int n = 0; n < 10; ++n)
                {
                    Console.Write("Sending {0}...", message);

                    // Send
                    requester.Send(new ZFrame(message));

                    // Receive
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        result = reply.ReadString();
                        Console.WriteLine(" Received: {0}", reply.ReadString());
                    }
                }
            }
            return result;
        }

        private string generateMessage(int result, Playfield state) // for player 1
        {
            //Message message = new Message();
            //message.result = result;
            //return JsonConvert.SerializeObject(message);
            return "1,2,3,4";
        }

        public void Connect()
        {
            requester = new ZSocket(ZSocketType.REQ);
            // Connect
            requester.Connect("tcp://127.0.0.1:5556");
        }

        public void Disconnect()
        {
            requester.Disconnect("tcp://127.0.0.1:5556");
        }

        public void Dispose()
        {
            requester.Dispose();
        }

        public string send(String message)
        {
            requester.Send(new ZFrame(message));
            string ret = null;
            // Receive
            using (ZFrame reply = requester.ReceiveFrame())
            {
                ret = reply.ReadString();
            }
            return ret;
        }

        public void Test()
        {
            Connect();

            for (int n = 0; n < 10000; ++n)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 100; i++)
                {
                    sb.Append(i.ToString());
                    if (i < 99) sb.Append(",");
                }

                send(sb.ToString());
            }
        }
    }
}
