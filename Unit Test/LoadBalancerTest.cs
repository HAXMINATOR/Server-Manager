using NUnit.Framework;
using Server_Manager;

namespace Unit_Test
{
    public class Tests
    {
        LoadBalancer lb;

        [SetUp]
        public void Setup()
        {
            lb = new LoadBalancer();
        }

        [Test]
        public void PlayerJoin()
        {
            lb.CreateNewInstance();
            Player player = new Player();

            lb.PlayerJoin(player);

            Assert.IsTrue(lb.instances[0].Players.Contains(player));
        }

        [Test]
        public void PlayerJoinLessPopulated()
        {
            for (int i = 0; i < 2; i++)
            {
                lb.CreateNewInstance();
            }
            for (int i = 0; i < lb.maxPlayerCount; i++)
            {
                lb.PlayerJoin(new Player(), 0);
            }

            lb.PlayerJoin(new Player());

            Assert.IsTrue(lb.instances[0].PlayerCount == lb.maxPlayerCount && lb.instances[1].PlayerCount == 1);
        }

        [Test]
        public void CreateNewInstanceIfFull()
        {
            lb.CreateNewInstance();
            for (int i = 0; i < lb.maxPlayerCount; i++)
            {
                lb.PlayerJoin(new Player(), 0);
            }

            lb.PlayerJoin(new Player());

            Assert.IsTrue(lb.instances.Count == 2);
        }

        [Test]
        public void CreateNewInstanceIfNonExists()
        {
            lb.PlayerJoin(new Player());

            Assert.IsTrue(lb.instances.Count == 1);
        }

        [Test]
        public void DeleteSpecificInstance()
        {
            lb.CreateNewInstance();
            if (lb.instances.Count != 0)
            {
                lb.DeleteInstance(lb.instances[0].ID);
            }

            Assert.IsTrue(lb.instances.Count == 0);
        }

        [Test]
        public void CheckForEmptyServers()
        {
            for (int i = 0; i < 3; i++)
            {
                lb.CreateNewInstance();
            }

            lb.PlayerJoin(new Player());

            lb.CheckForEmptyServers();

            Assert.IsTrue(lb.instances.Count == 1);
        }
    }
}