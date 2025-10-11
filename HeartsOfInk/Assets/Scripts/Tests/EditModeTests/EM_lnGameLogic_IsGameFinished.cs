using Assets.Scripts.Logic;
using NETCoreServer.Models;
using NUnit.Framework;
using System.Collections.Generic;

public class EM_lnGameLogic_IsGameFinished
{
    private InGameLogic logic;

    [SetUp]
    public void Setup()
    {
        logic = new InGameLogic();
    }

    [Test]
    public void GameShouldThrowException_WhenNoOwners()
    {
        Assert.Throws<System.Exception>(() => logic.IsGameFinished(new List<Player>()));
    }

    [Test]
    public void GameShouldFinish_WhenOnlyOnePlayerOwnsCities()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 0 }
        };
        Assert.IsTrue(logic.IsGameFinished(owners));
    }

    [Test]
    public void GameShouldNotFinish_WhenAllPlayersWithoutAlliance()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 0 },
            new Player { Name = "Bob", Alliance = 0 },
            new Player { Name = "Charlie", Alliance = 0 }
        };
        Assert.IsFalse(logic.IsGameFinished(owners));
    }

    [Test]
    public void GameShouldNotFinish_WhenPlayersBelongToDifferentAlliances()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 1 },
            new Player { Name = "Bob", Alliance = 2 },
            new Player { Name = "Charlie", Alliance = 1 },
            new Player { Name = "Diana", Alliance = 2 }
        };
        Assert.IsFalse(logic.IsGameFinished(owners));
    }

    [Test]
    public void GameShouldFinish_WhenAllPlayersInSameAlliance()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 3 },
            new Player { Name = "Bob", Alliance = 3 },
            new Player { Name = "Charlie", Alliance = 3 }
        };
        Assert.IsTrue(logic.IsGameFinished(owners));
    }

    [Test]
    public void GameShouldNotFinish_WhenMixedAllianceAndNoAlliance()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 0 },
            new Player { Name = "Bob", Alliance = 1 },
            new Player { Name = "Charlie", Alliance = 1 }
        };
        // There’s still a non-alligned player with cities → game not finished
        Assert.IsFalse(logic.IsGameFinished(owners));
    }

    [Test]
    public void GameShouldFinish_WhenAllInSameAlliance_AndNoNeutrals()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 2 },
            new Player { Name = "Bob", Alliance = 2 },
            new Player { Name = "Charlie", Alliance = 2 },
            new Player { Name = "Diana", Alliance = 2 }
        };
        Assert.IsTrue(logic.IsGameFinished(owners));
    }

    [Test]
    public void GameShouldNotFinish_WhenTwoAlliancesExist()
    {
        var owners = new List<Player>
        {
            new Player { Name = "Alice", Alliance = 1 },
            new Player { Name = "Bob", Alliance = 1 },
            new Player { Name = "Charlie", Alliance = 2 },
            new Player { Name = "Diana", Alliance = 2 }
        };
        Assert.IsFalse(logic.IsGameFinished(owners));
    }
}
