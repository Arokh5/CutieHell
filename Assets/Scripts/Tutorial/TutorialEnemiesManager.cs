using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemiesManager
{
    #region Fields
    private List<AIEnemy> enemies = new List<AIEnemy>();
    #endregion

    #region Public Methods
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
            enemy.agent.enabled = false;
        }
    }

    public void ResumeEnemies()
    {
        foreach (AIEnemy enemy in enemies)
        {
            enemy.agent.enabled = true;
        }
    }
    #endregion
}
