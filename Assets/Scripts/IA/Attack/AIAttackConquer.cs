using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackConquer : AIAttackLogic {

    #region Fields
    public float attackRange;
    [Tooltip("Time it takes (in seconds) to shrink the base model and grow the flow model.")]
    public float animationDuration;
    [Tooltip("Time it takes (in seconds) to fully conquest the Building AFTER the flower has fully grown.")]
    public float conquestDuration;
    private float dps;

    private bool inTransformationAnimation = false;
    private bool inConquest = false;
    private bool converted = false;
    private float elapsedTime = 0;
    [SerializeField]
    private Renderer mainModelRenderer;
    [SerializeField]
    private Renderer alternateModelRenderer;
    [SerializeField]
    private Transform postConquestProp;

    private NavMeshAgent navMeshAgent;
    private AIEnemy aiEnemy;
    private Building targetInConquest;

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(postConquestProp, "ERROR: postConquestProp was not assigned in the Inspector for the AIAttackConquer script in GameOBject " + gameObject.name);
        aiEnemy = GetComponent<AIEnemy>();
        UnityEngine.Assertions.Assert.IsNotNull(aiEnemy, "ERROR: Can't find an AIEnemy script from the AIAttackConquer script in GameOBject " + gameObject.name);
        navMeshAgent = GetComponent<NavMeshAgent>();
        UnityEngine.Assertions.Assert.IsNotNull(navMeshAgent, "ERROR: Can't find a NavMeshAgent script from the AIAttackConquer script in GameOBject " + gameObject.name);
    }

    private void Update()
    {
        if (!targetInConquest)
            return;

        if (converted)
        {
            Instantiate(postConquestProp, transform.position, transform.rotation);
            aiEnemy.GetZoneController().RemoveEnemy(aiEnemy);
            Destroy(gameObject);
        }

        if (inConquest)
        {
            Attack(targetInConquest);
            elapsedTime += Time.deltaTime;
            if (elapsedTime > conquestDuration)
            {
                converted = true;
                inConquest = false;
                elapsedTime = 0;
                dps = 0;
            }
        }
        else if (inTransformationAnimation)
        {
            if (elapsedTime < animationDuration)
            {
                AttackAnimation();
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= animationDuration)
                {
                    inConquest = true;
                    inTransformationAnimation = false;
                    elapsedTime = 0;
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(Building target)
    {
        if (!targetInConquest)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                targetInConquest = target;
                inTransformationAnimation = true;
                elapsedTime = 0;
                navMeshAgent.enabled = false;
                aiEnemy.SetIsTargetable(false);
                mainModelRenderer.material.color = Color.cyan;
                /* Now we calculate the actual dps */
                dps = conquestDuration == 0 ? -1 : target.GetMaxHealth() / conquestDuration;
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
        if (dps != -1)
        {
            target.TakeDamage(dps * Time.deltaTime, AttackType.ENEMY);
        }
        else
        {
            target.TakeDamage(target.GetMaxHealth(), AttackType.ENEMY);
        }
    }
    #endregion
}
