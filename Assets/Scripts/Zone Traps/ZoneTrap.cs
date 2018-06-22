using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZoneTrap : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float usageCost;
    [SerializeField]
    protected float animationCooldownTime = 1.0f;
    private float cooldownTimeleft = 0;

    protected AIZoneController zoneController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        zoneController = GetComponentInParent<AIZoneController>();
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "ERROR: A AIZoneController Component could not be found by ZoneTrap in GameObject " + gameObject.name);
    }

    void Update ()
    {
		if (!GameManager.instance.gameIsPaused && cooldownTimeleft > 0)
        {
            UpdateTrapEffect();
            cooldownTimeleft -= Time.deltaTime;
            if (cooldownTimeleft < 0)
            {
                cooldownTimeleft = 0;
                EndTrapEffect();
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            player.zoneTrap = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player && player.zoneTrap == this)
        {
            player.zoneTrap = null;
        }
    }
    #endregion

    #region Public Methods
    public bool CanUse()
    {
        return cooldownTimeleft <= 0 && zoneController.monumentTaken == false;
    }

    public float GetUsageCost()
    {
        return usageCost;
    }

    public void UseZoneTrap()
    {
        cooldownTimeleft = animationCooldownTime;
        StartTrapEffect();
    }
    #endregion

    #region Protected Methods
    protected abstract void StartTrapEffect();
    protected abstract void UpdateTrapEffect();
    protected abstract void EndTrapEffect();
    #endregion
}
