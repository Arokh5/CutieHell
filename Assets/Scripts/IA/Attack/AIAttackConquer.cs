using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackConquer : AIAttackLogic {

    #region Fields
    public float attackRange;
    public float dps;

    [Tooltip("Duration expressed in seconds.")]
    public float animationDuration;

    private bool inAttackAnimation = false;
    private float elapsedTime = 0;
    [SerializeField]
    private Renderer mainModelRenderer;
    [SerializeField]
    private Renderer alternateModelRenderer;
    private AIEnemy aiEnemy;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        aiEnemy = GetComponent<AIEnemy>();
        UnityEngine.Assertions.Assert.IsNotNull(aiEnemy, "ERROR: Can't find an AIEnemy script from the AiAttackConquer script in GameOBject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target)
    {
        if (inAttackAnimation)
        {
            AttackAnimation();
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= animationDuration)
            {
                Attack(target);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                inAttackAnimation = true;
                aiEnemy.SetIsTargetable(false);
                mainModelRenderer.material.color = Color.cyan;
            }
        }
    }
    #endregion

    #region Private Methods
    private void AttackAnimation()
    {
        float progress = elapsedTime / animationDuration;

        if (progress < 0.5f)
        {
            progress *= 2;
            mainModelRenderer.material.SetFloat("_Size", 1 - progress);
        }
        else
        {
            progress = (progress - 0.5f) * 2;
            alternateModelRenderer.material.SetFloat("_Size", progress);
        }

    }

    private void Attack(Building target)
    {
        target.TakeDamage(dps * Time.deltaTime, AttackType.ENEMY);
    }
    #endregion
}
