using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttack : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float sphereCastRadius;
    [SerializeField]
    private float basicAttackCadency;
    [SerializeField]
    private float turretTrapCadency;
    [SerializeField]
    private Player player;


    private AIEnemy currentTarget = null;
    private RaycastHit hit;
    private float time;

    private float seductiveLastLaunchedTime = 0;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        time = basicAttackCadency;
    }

    private void Update()
    {
        if (player.state == Player.PlayerStates.MOVE || player.state == Player.PlayerStates.TURRET)
        {
            FocusBasicAttack();
            seductiveLastLaunchedTime = 0;
            Debug.Log("Find a way to avoid this cheap but repetitive assignation");
        }
        else if (player.state == Player.PlayerStates.SEDUCTIVE)
        {
            seductiveLastLaunchedTime += Time.deltaTime;
            FocusSeductiveAttack();
        }
      
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void FocusBasicAttack()
    {
        if (GameManager.instance.gameIsPaused)
        {
            return;
        }

        time += Time.deltaTime;
        
        if (InputManager.instance.GetR2Button() && !InputManager.instance.GetL2Button())
        {
            float cadency = 0f;

            if (player.state == Player.PlayerStates.MOVE)
                cadency = basicAttackCadency;

            if (player.state == Player.PlayerStates.TURRET)
                cadency = turretTrapCadency;

            if (time >= cadency)
            {
                Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 2);

                if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, 100, layerMask.value))
                {
                    if (hit.transform.GetComponent<AIEnemy>())
                    {
                        GameManager.instance.GetPlayer1().GetComponent<AttackController>().InstantiateAttack(hit.transform, hit.point);
                        //Debug.Log("Hit enemy");
                    }
                    else
                    {
                        GameManager.instance.GetPlayer1().GetComponent<AttackController>().InstantiateAttack(null, Vector3.zero);
                    }
                }
                else
                {
                    GameManager.instance.GetPlayer1().GetComponent<AttackController>().InstantiateAttack(null, Vector3.zero);
                }

                time = 0f;
            }
        }

        AIEnemy newTarget = null;
        if (!InputManager.instance.GetL2Button() && Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, 100, layerMask.value))
        {
            newTarget = hit.transform.GetComponent<AIEnemy>();
        }


        if (currentTarget)
        {
            if (!newTarget)
            {
                currentTarget.MarkAsTarget(false);
                currentTarget = null;
            }
            else if (currentTarget != newTarget)
            {
                currentTarget.MarkAsTarget(false);
                newTarget.MarkAsTarget(true);
                currentTarget = newTarget;
            }
        }
        else if (newTarget)
        {
            newTarget.MarkAsTarget(true);
            currentTarget = newTarget;
        }

    }

    private void FocusSeductiveAttack()
    {
        if (InputManager.instance.GetR2ButtonDown())
        {
            Trap attackingTrap = GameManager.instance.GetTrapBeingUsed();
            if (seductiveLastLaunchedTime >= attackingTrap.cooldownBetweenSeductiveProjections || 
                attackingTrap.GetLandedEnemyProjectionsCount() == 0)
            {
                if (GameManager.instance.GetTrapBeingUsed() != null)
                {
                    attackingTrap.LandSeductiveEnemyProjection();
                    attackingTrap.InstantiateSeductiveEnemyProjection();
                    seductiveLastLaunchedTime = Time.deltaTime;
                }
            }
        }
    }
    #endregion
}