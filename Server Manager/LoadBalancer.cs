using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Manager
{
    public class LoadBalancer
    {
        public readonly List<ServerInstance> instances = new();
        public readonly int minPlayerCount = 5;
        public readonly int maxPlayerCount = 50;

        /// <summary>
        /// Checks current <see cref="ServerInstance"/>s and assigns the <paramref name="player"/> to the optimal one, reporting back to <see cref="ServerManagerLoginMQ"/>
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that should be assigned to a <see cref="ServerInstance"/></param>
        public void PlayerJoin(Player player)
        {
            ServerInstance chosenInstance = null;

            //if there are no server instances, create one
            if (instances.Count() == 0)
            {
                CreateNewInstance();
            }

            foreach (ServerInstance instance in instances)
            {
                if (chosenInstance != null) //if an instance is chosen
                {
                    if (chosenInstance.PlayerCount <= minPlayerCount) //if the instance has less than the desired minimum amount, choose that instance
                    {
                        chosenInstance = instance;
                    }
                    else if (chosenInstance.PlayerCount > instance.PlayerCount && instance.PlayerCount != maxPlayerCount) //if the instance has less players than the current chosen instance, choose that instance
                    {
                        chosenInstance = instance;
                    }
                }
                else //set first instance as chosen
                {
                    chosenInstance = instance;
                }
            }

            //if the chosen instance has the maximum amount of players, create a new one and then choose the new instance
            if (chosenInstance.PlayerCount == maxPlayerCount)
            {
                chosenInstance = CreateNewInstance();
            }

            chosenInstance.Players.Add(player);

            //call back to the login service that player should join chosenInstance
        }

        /// <summary>
        /// Assigns a <see cref="Player"/> to a specific <see cref="ServerInstance"/> by ID
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that should be assigned to a <see cref="ServerInstance"/></param>
        /// <param name="instanceID">The ID of the <see cref="ServerInstance"/> that the <see cref="Player"/> should join</param>
        public void PlayerJoin(Player player, int instanceID)
        {
            foreach (ServerInstance instance in instances)
            {
                if (instance.ID == instanceID)
                {
                    instance.Players.Add(player);

                    //call back to the login service that player should join chosenInstance

                    return;
                }
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="ServerInstance"/> and adds it to <see cref="instances"/>
        /// </summary>
        /// <returns>The newly created <see cref="ServerInstance"/></returns>
        public ServerInstance CreateNewInstance()
        {
            int newInstanceID = 0;
            if (instances.Count() > 0)
            {
                newInstanceID = instances.Count();
            }
            ServerInstance newInstance = new ServerInstance(newInstanceID);
            instances.Add(newInstance);

            return newInstance;
        }

        /// <summary>
        /// Deletes the <see cref="ServerInstance"/> with the <see cref="ServerInstance.ID"/> of <paramref name="instanceID"/>
        /// </summary>
        /// <param name="instanceID">ID of the <see cref="ServerInstance"/> that should be deleted</param>
        public void DeleteInstance(int instanceID)
        {
            ServerInstance instanceToDelete = null;

            foreach (ServerInstance instance in instances)
            {
                if (instance.ID == instanceID && instance.PlayerCount == 0) //if the ID of the instance is the one that should be deleted and there are no players currently on it, choose that instance
                {
                    instanceToDelete = instance;
                }
            }

            if (instanceToDelete != null) //if an instance were selected
            {
                instances.Remove(instanceToDelete);
                instanceToDelete = null;
            }
        }

        /// <summary>
        /// Checks all <see cref="ServerInstance"/>s in <see cref="instances"/>, for each of them where <see cref="ServerInstance.PlayerCount"/> is 0, call <see cref="DeleteInstance(int)"/> for each of them
        /// </summary>
        public void CheckForEmptyServers()
        {
            List<ServerInstance> instancesToBeDeleted = new();

            //adds every instance with a PlayerCount of 0 to instancesToBeDeleted
            foreach (ServerInstance instance in instances)
            {
                if (instance.PlayerCount == 0)
                {
                    instancesToBeDeleted.Add(instance);
                }
            }

            //call DeleteInstance for each instance in instancesToBeDeleted
            //this might have to be a for loop instead of a foreach
            foreach (ServerInstance instance in instancesToBeDeleted)
            {
                DeleteInstance(instance.ID);
            }
        }
    }
}
