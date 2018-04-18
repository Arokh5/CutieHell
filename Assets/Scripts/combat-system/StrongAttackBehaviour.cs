using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackBehaviour : MonoBehaviour {

    public LayerMask layerMask;
    public int damage;
    public StrongAttackExplosionDetection enemiesList;
    public int enemiesToCombo;
    public int evilComboReward;
    private int comboCount;
    private float timer;
    private float timerToDisable;

    private void OnEnable () {
        enemiesList.currentStrongAttackTargets.Clear();
        timer = 0.5f;
        timerToDisable = 2.8f;
        comboCount = 0;
	}
	
	void Update () {
        timer -= Time.deltaTime;
        timerToDisable -= Time.deltaTime;
        if(timer <= 0.0f)
        {
            HurtEnemies();
            timer = 1000.0f;
        }
        if(timerToDisable <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
	}

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
            Debug.Log("COMBO!!");
            GameManager.instance.GetPlayer1().SetEvilLevel(evilComboReward);
            UIManager.instance.ShowComboText(UIManager.ComboTypes.StrongCombo);
        }

        comboCount = 0;
    }
}