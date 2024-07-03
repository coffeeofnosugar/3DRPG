using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    [HideInInspector] public CharacterStats playerStats;

    private List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    private void Update()
    {
        if (playerStats.IsDeath)
        {
            Debug.Log("”Œœ∑Ω· ¯");
            NotifyObserver();
        }
    }

    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
