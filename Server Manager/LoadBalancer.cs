using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Manager
{
    public class LoadBalancer
    {
        List<ServerInstance> instances = new();
        int minPlayerCount = 5;
        int maxPlayerCount = 50;

        public void PlayerJoin(Player player)
        {
            ServerInstance chosenInstance = null;

            if (instances.Count() == 0)
            {
                CreateNewInstance();
            }

            foreach (ServerInstance instance in instances)
            {
                if (chosenInstance != null)
                {
                    if (chosenInstance.Players.Count() <= minPlayerCount)
                    {
                        chosenInstance = instance;
                    }
                    else if (chosenInstance.Players.Count() > instance.Players.Count())
                    {
                        chosenInstance = instance;
                    }
                }
                else
                {
                    chosenInstance = instance;
                }
            }

            chosenInstance.Players.Add(player);
        }

        void CreateNewInstance()
        {
            int newInstanceID = 0;
            if (instances.Count() > 0)
            {
                newInstanceID = instances.Count();
            }
            instances.Add(new ServerInstance(newInstanceID));
        }

        void DeleteInstance(int instanceID)
        {
            ServerInstance instanceToDelete = null;

            foreach (ServerInstance instance in instances)
            {
                if (instance.ID == instanceID && instance.Players.Count() == 0)
                {
                    instanceToDelete = instance;
                }
            }

            if (instanceToDelete != null)
            {
                instances.Remove(instanceToDelete);
            }
        }
    }
}
