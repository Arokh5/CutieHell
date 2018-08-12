using UnityEngine;
using UnityEngine.AI;

public class AIAttackConquer : AIAttackLogic {

    #region Fields
    [SerializeField]
    private float attackRange;
    [SerializeField]
    [Tooltip("Time it takes (in seconds) to shrink the base model and grow the flow model.")]
    private float animationDuration;
    [SerializeField]
    [Tooltip("Time it takes (in seconds) to fully conquest the Building AFTER the flower has fully grown.")]
    private float conquestDuration;
    [SerializeField]
    [Tooltip("Time (in seconds) it takes for the Conqueror to be removed from the AIZoneController after the Zone has been conquered.")]
    private float suicideDelay = 2.5f;

    private float dps;
    private bool inConquest = false;
    private bool converted = false;
    private float elapsedTime = 0.0f;
    private float suicideTime = 0.0f;

    private NavMeshAgent navMeshAgent;
    private AIEnemy aiEnemy;
    private Building targetInConquest;
    private Animator animator;

    [SerializeField]
    private AudioSource attackSource;
    [SerializeField]
    private AudioClip attackClip;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        //UnityEngine.Assertions.Assert.IsNotNull(postConquestProp, "ERROR: postConquestProp was not assigned in the Inspector for the AIAttackConquer script in GameOBject " + gameObject.name);
        aiEnemy = GetComponent<AIEnemy>();
        UnityEngine.Assertions.Assert.IsNotNull(aiEnemy, "ERROR: Can't find an AIEnemy script from the AIAttackConquer script in GameOBject " + gameObject.name);
        navMeshAgent = GetComponent<NavMeshAgent>();
        UnityEngine.Assertions.Assert.IsNotNull(navMeshAgent, "ERROR: Can't find a NavMeshAgent script from the AIAttackConquer script in GameOBject " + gameObject.name);
        //UnityEngine.Assertions.Assert.IsNotNull(mainModelRenderer, "ERROR: mainModelRenderer was not assigned in the Inspector for the AIAttackConquer script in GameOBject " + gameObject.name);
        //UnityEngine.Assertions.Assert.IsNotNull(alternateModelRenderer, "ERROR: alternateModelRenderer was not assigned in the Inspector for the AIAttackConquer script in GameOBject " + gameObject.name);
        //alternateModelRenderer.material.SetFloat("_Size", 0.0f);
        attackSource.clip = attackClip;
    }

    private void Update()
    {
        if (!targetInConquest)
            return;

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

        if (converted && aiEnemy.GetZoneController().isConquered)
        {
            suicideTime += Time.deltaTime;
            if (suicideTime > suicideDelay)
            {
                targetInConquest.attachedConqueror = aiEnemy;
                aiEnemy.GetZoneController().RemoveEnemy(aiEnemy);
                UIManager.instance.roundInfoController.AddToEnemiesCount(-1);
                enabled = false;
            }
        }
    }
    #endregion

    #region Public Methods
    public override void AttemptAttack(IDamageable attackTarget, Vector3 navigationTarget)
    {
        Building building = attackTarget as Building;
        if (building && !targetInConquest)
        {
            if (building.attachedConqueror == null && IsInAttackRange(navigationTarget))
            {
                targetInConquest = building;
                targetInConquest.attachedConqueror = aiEnemy;
                //inTransformationAnimation = true;
                animator.SetTrigger("Attack");
                elapsedTime = 0;
                navMeshAgent.enabled = false;
                aiEnemy.SetIsTargetable(false);
                //mainModelRenderer.material.color = Color.cyan;
                /* Now we calculate the actual dps */
                dps = conquestDuration == 0 ? -1 : building.GetMaxHealth() / conquestDuration;
            }
        }
    }

    public override bool IsInAttackRange(Vector3 navigationTarget)
    {
        return Vector3.Distance(transform.position, navigationTarget) < attackRange;
    }

    public void EndTransformation()
    {
        inConquest = true;
        attackSource.Play();
        elapsedTime = 0.0f;
        suicideTime = 0.0f;
    }
    #endregion

    #region Private Methods
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
