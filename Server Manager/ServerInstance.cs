using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Manager
{
    public class ServerInstance
    {
        public string serverInstanceName;

        static int nextID = 0;
        int id;
        List<Player> players = new();
        string ip;
        ProcessStartInfo start;

        public ServerInstance(int id = -1)
        {
            if (id == -1)
            {
                id = nextID++;
            }
            else
            {
                ID = id;
                nextID = id + 1;
            }
            

            start = new();
            start.FileName = ""; //insert path here
            new Thread(RunInstance).Start();
        }

        void RunInstance()
        {
            using (Process process = Process.Start(start))
            {
                process.WaitForExit();
            }

            Thread thread = new Thread(() => LoadBalancer.SingletonInstance.DeleteInstance(id));
            thread.Start();
        }

        public int ID { get => id; set => id = value; }
        public List<Player> Players { get => players; set => players = value; }
        public int PlayerCount { get => players.Count;}
        public string Ip { get => ip; set => ip = value; }
    }
}
