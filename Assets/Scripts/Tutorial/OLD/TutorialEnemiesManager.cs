using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemiesManager
{
    #region Fields
    private List<AIEnemy> enemies = new List<AIEnemy>();
    private List<AIEnemy> toRemove = new List<AIEnemy>();
    #endregion

    #region Public Methods
    public int GetEnemiesCount()
    {
        return enemies.Count;
    }

    public void RemoveDeadEnemies()
    {
        foreach (AIEnemy enemy in enemies)
        {
            if (enemy.IsDead())
                toRemove.Add(enemy);
        }
        foreach (AIEnemy enemy in toRemove)
        {
            enemies.Remove(enemy);
        }
        toRemove.Clear();
    }

    public bool AddEnemy(AIEnemy enemy)
    {
        bool success = false;
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            success = true;
        }
        return success;
    }

    public bool RemoveEnemy(AIEnemy enemy)
    {
        return enemies.Remove(enemy);
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }

    public void HaltEnemies()
    {
        foreach (AIEnemy enemy in enemies)
        {
            enemy.SetAgentEnable(false);
        }
    }

    public void ResumeEnemies()
    {
        foreach (AIEnemy enemy in enemies)
        {
            enemy.SetAgentEnable(true);
        }
    }
    #endregion
}
