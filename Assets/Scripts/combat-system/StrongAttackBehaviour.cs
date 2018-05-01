using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackBehaviour : PooledParticleSystem
{
    #region Fields
    public LayerMask layerMask;
    public int damage;
    public StrongAttackExplosionDetection enemiesList;
    public int enemiesToCombo;
    public int evilComboReward;
    public float hurtEnemiesDelay;
    public float returnToPoolDelay;
    private int comboCount;
    private float timer;
    private float timerToDisable;
    #endregion

    #region MonoBehaviour Methods
	private void Update () {
        timer -= Time.deltaTime;
        timerToDisable -= Time.deltaTime;
        if(timer <= 0.0f)
        {
            HurtEnemies();
            timer = 1000.0f;
        }
        if(timerToDisable <= 0.0f)
        {
            ReturnToPool();
        }
    }
    #endregion

    #region Public Methods
    public override void Restart()
    {
        enemiesList.currentStrongAttackTargets.Clear();
        timer = hurtEnemiesDelay;
        timerToDisable = returnToPoolDelay;
        comboCount = 0;
    }
    #endregion

    #region Private Methods
    private void HurtEnemies()
    {
        foreach (AIEnemy aiEnemy in enemiesList.currentStrongAttackTargets)
        {
            aiEnemy.MarkAsTarget(false);
            aiEnemy.TakeDamage(damage, AttackType.STRONG);
            comboCount++;
        }

        CheckIfCombo();
        enemiesList.currentStrongAttackTargets.Clear();
        GameManager.instance.GetPlayer1().timeSinceLastStrongAttack = 0f;
    }

    private void CheckIfCombo()
    {
        if (comboCount >= enemiesToCombo)
        {
            //Debug.Log("COMBO!!");
            GameManager.instance.GetPlayer1().SetEvilLevel(evilComboReward);
            UIManager.instance.ShowComboText(UIManager.ComboTypes.StrongCombo);
        }

        comboCount = 0;
    }
    #endregion
}
