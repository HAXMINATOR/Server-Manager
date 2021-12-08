using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Manager
{
    public class ServerInstance
    {
        int id;
        List<Player> players = new();

        public ServerInstance(int id)
        {
            ID = id;
        }

        public int ID { get => id; set => id = value; }
        public List<Player> Players { get => players; set => players = value; }
    }
}
