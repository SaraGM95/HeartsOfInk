using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using NETCoreServer.Models;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class EM_IsPlayerVictory
{
    private EndgameLogic checker;

    [SetUp]
    public void SetUp()
    {
        checker = new EndgameLogic();
    }
    [Test]
    public void Player_HasCities_ShouldReturnTrue()
    {
        var player1 = new PlayerStatistics(new Player { Name = "Alice", Alliance = 0 }) { CitiesAtEnd = 3 };
        var player2 = new PlayerStatistics(new Player { Name = "Bob", Alliance = 0 }) { CitiesAtEnd = 0 };

        var allPlayers = new List<PlayerStatistics> { player1, player2 };

        bool result = checker.IsPlayerVictory(player1, allPlayers);

        Assert.IsTrue(result);
    }

    [Test]
    public void Player_NoCities_NoAlliance_ShouldReturnFalse()
    {
        var player1 = new PlayerStatistics(new Player { Name = "Charlie", Alliance = 0 }) { CitiesAtEnd = 0 };
        var player2 = new PlayerStatistics(new Player { Name = "Diana", Alliance = 0 }) { CitiesAtEnd = 2 };

        var allPlayers = new List<PlayerStatistics> { player1, player2 };

        bool result = checker.IsPlayerVictory(player1, allPlayers);

        Assert.IsFalse(result);
    }

    [Test]
    public void Player_NoCities_Alliance_HasAllyWithCities_ShouldReturnTrue()
    {
        var player1 = new PlayerStatistics(new Player { Name = "Eve", Alliance = 1 }) { CitiesAtEnd = 0 };
        var ally = new PlayerStatistics(new Player { Name = "Frank", Alliance = 1 }) { CitiesAtEnd = 2 };
        var other = new PlayerStatistics(new Player { Name = "Grace", Alliance = 2 }) { CitiesAtEnd = 0 };

        var allPlayers = new List<PlayerStatistics> { player1, ally, other };

        bool result = checker.IsPlayerVictory(player1, allPlayers);

        Assert.IsTrue(result);
    }

    [Test]
    public void Player_NoCities_Alliance_AlliesWithoutCities_ShouldReturnFalse()
    {
        var player1 = new PlayerStatistics(new Player { Name = "Hannah", Alliance = 1 }) { CitiesAtEnd = 0 };
        var ally = new PlayerStatistics(new Player { Name = "Ian", Alliance = 1 }) { CitiesAtEnd = 0 };
        var other = new PlayerStatistics(new Player { Name = "Julia", Alliance = 2 }) { CitiesAtEnd = 5 };

        var allPlayers = new List<PlayerStatistics> { player1, ally, other };

        bool result = checker.IsPlayerVictory(player1, allPlayers);

        Assert.IsFalse(result);
    }

    [Test]
    public void Player_NoCities_Alliance_ButIsOnlyMember_ShouldReturnFalse()
    {
        var player1 = new PlayerStatistics(new Player { Name = "Kevin", Alliance = 3 }) { CitiesAtEnd = 0 };
        var other = new PlayerStatistics(new Player { Name = "Laura", Alliance = 2 }) { CitiesAtEnd = 5 };

        var allPlayers = new List<PlayerStatistics> { player1, other };

        bool result = checker.IsPlayerVictory(player1, allPlayers);

        Assert.IsFalse(result);
    }

    [Test]
    public void MultipleAlliances_OnlyCorrectAllianceCounts()
    {
        var player1 = new PlayerStatistics(new Player { Name = "Mike", Alliance = 1 }) { CitiesAtEnd = 0 };
        var ally = new PlayerStatistics(new Player { Name = "Nina", Alliance = 1 }) { CitiesAtEnd = 1 };
        var enemy = new PlayerStatistics(new Player { Name = "Oscar", Alliance = 2 }) { CitiesAtEnd = 5 };

        var allPlayers = new List<PlayerStatistics> { player1, ally, enemy };

        bool result = checker.IsPlayerVictory(player1, allPlayers);

        Assert.IsTrue(result, "Only allies from the same alliance should be considered.");
    }
}
