using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Manager
{
    public class Player
    {
        static int nextID = 0;
        public readonly string name;
        public readonly int id;

        public Player(string name)
        {
            this.name = name;
            id = nextID++;
        }
    }
}
