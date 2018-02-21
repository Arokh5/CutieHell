using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquerableBuilding : MonoBehaviour {


    private MeshRenderer meshRenderer;
    [SerializeField]
    private MeshRenderer areaOfEffect;

    [Header("Life and stuff")]
    public int initialHitPoints = 50;
    [SerializeField]
	private int hitPoints;
    private bool conquered = false;

    [Header("Damage testing")]
    public bool reset = false;
    public bool loseHitPoints = false;
    public int lifeLossPerSecond = 0;
    private float lifeToLose = 0;

    [Header("Life Restore testing")]
    public bool restoreLife = false;
    public int lifeToRestore = 0;

    private void OnApplicationQuit()
    {
        ResetAutomaticLifeLoss();
    }

    void Awake ()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

	void Start ()
    {
        hitPoints = initialHitPoints;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (reset)
        {
            reset = false;
            loseHitPoints = false;
            ResetAutomaticLifeLoss();
        }

        if (restoreLife)
        {
            restoreLife = false;
            RestoreHitPoints(lifeToRestore);
        }

        if (loseHitPoints)
        {
            lifeToLose += lifeLossPerSecond * Time.deltaTime;
            if (lifeToLose > 1)
            {
                lifeToLose -= 1;
                TakeDamage(1);
                if (conquered)
                    loseHitPoints = false;
            }
        }
        
	}

    void TakeDamage(int damage)
    {
        if (conquered)
            return;

        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            hitPoints = 0;
            Conquer();
        }

        AdjustMaterial();
    }

    void RestoreHitPoints (int toRestore)
    {
        hitPoints += toRestore;
        if (hitPoints > initialHitPoints)
            hitPoints = initialHitPoints;

        AdjustMaterial();

        if (conquered && hitPoints > 0)
            Unconquer();
    }

    void Conquer()
    {
        conquered = true;
        areaOfEffect.material.SetFloat("_Blend", 1);
    }

    void Unconquer()
    {
        conquered = false;
        areaOfEffect.material.SetFloat("_Blend", 0);
    }

    void AdjustMaterial()
    {
        float blendFactor = (initialHitPoints - hitPoints) / (float)initialHitPoints;
        meshRenderer.material.SetFloat("_Blend", blendFactor);
    }

    void ResetAutomaticLifeLoss()
    {
        hitPoints = initialHitPoints;
        lifeToLose = 0;
        meshRenderer.material.SetFloat("_Blend", 0);

        conquered = false;
        areaOfEffect.material.SetFloat("_Blend", 0);
    }
}
