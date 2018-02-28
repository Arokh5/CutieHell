﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquerableBuilding : MonoBehaviour {

    [HideInInspector]
    public GameScenariosManager gameScenariosManager;

    [Header("Elements setup")]

    [SerializeField]
    private MeshRenderer buildingRenderer;
    [SerializeField]
    private MeshRenderer alternateBuildingRenderer;
    [SerializeField]
    private MeshRenderer areaRenderer;

    [Header("Life and stuff")]
    [Tooltip("The initial amount of hit points for the conquerable building.")]
    [SerializeField]
    private float initialHitPoints = 50;
    // Duration in seconds
    [Tooltip("The duration (in seconds) for which the conquerable object is considered to be \"under attack\" after the last actual attack happened.")]
    [SerializeField]
    private float underAttackStateDuration = 1;
    [Tooltip("The duration (in seconds) that the dark to cute conversion takes.")]
    [SerializeField]
    private float conquerEffectDuration = 1;

    private float underAttackElapsedTime = 0;
    [SerializeField]    // TEST
    private float conquerEffectElapsedTime = 0;
    [SerializeField]    // TEST
	private float hitPoints;
    private bool inUse = false;
    private bool underAttack = false;
    private bool conquering = false;
    private bool conquered = false;

    [Header("Model shaking")]
    [Range(0, 1)]
    [SerializeField]
    private float shakeAmplitude;
    [SerializeField]
    private float shakeSpeed;

    [Header("Damage testing")]
    public bool reset = false; // TEST
    public bool loseHitPoints = false; // TEST
    public int lifeLossPerSecond = 0; // TEST
    public bool restoreLife = false;
    

    private void OnApplicationQuit()
    {
        Reset();
    }

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(buildingRenderer, "Building Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateBuildingRenderer, "Alternate Building Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(areaRenderer, "Area Renderer not assigned for ConquerableBuilding script in GameObject " + gameObject.name);
        buildingRenderer.gameObject.SetActive(true);
        alternateBuildingRenderer.gameObject.SetActive(false);
        areaRenderer.gameObject.SetActive(true);
    }

    private void Start ()
    {
        Reset();
        hitPoints = initialHitPoints;
        buildingRenderer.material.SetFloat("_ShakeAmplitude", shakeAmplitude);
        buildingRenderer.material.SetFloat("_ShakeSpeed", shakeSpeed);
    }
	
	// Update is called once per frame
	private void Update ()
    {
        Test();

        if (underAttack)
        {
            underAttackElapsedTime += Time.deltaTime;
            if (underAttackElapsedTime >= underAttackStateDuration)
            {
                SetUnderAttack(false);
                underAttackElapsedTime = 0;
            }
        }

        if (conquering)
        {
            conquerEffectElapsedTime += Time.deltaTime;
            if (conquerEffectElapsedTime < conquerEffectDuration)
            {
                ConquerEffect();
            }
            else
            {
                Conquer();
            }
        }
	}

    public void TakeDamage(float damage)
    {
        if (conquered)
            return;

        // Reset the currentUnderAttackElapsedTime timer
        SetUnderAttack(true);
        underAttackElapsedTime = 0;

        hitPoints -= damage;


        if (hitPoints <= 0)
        {
            hitPoints = 0;
        }

        AdjustMaterials();

        if (hitPoints == 0)
        {
            ConquerEffect();
        }

    }

    public void RestoreHitPoints()
    {
        hitPoints = initialHitPoints;

        AdjustMaterials();

        if (conquered)
        {
            Reset();
        }
    }

    public bool IsCute()
    {
        return conquered;
    }

    public float GetCurrentHitPoints()
    {
        return hitPoints;
    }

    public bool GetBeingUsed()
    {
        return inUse;
    }

    public void SetBeingUsed(bool inUseState)
    {
        inUse = inUseState;
    }

    public void SetConquered(bool conqueredState)
    {
        if (conqueredState != conquered)
        {
            if (conqueredState)
            {
                Conquer();
            }
            else
            {
                Reset();
            }
        }
    }

    private void Test()
    {
        if (reset)
        {
            reset = false;
            loseHitPoints = false;
            Reset();
        }

        if (restoreLife)
        {
            restoreLife = false;
            RestoreHitPoints();
        }

        if (loseHitPoints)
        {
            TakeDamage(lifeLossPerSecond * Time.deltaTime);
            if (conquered)
                loseHitPoints = false;
        }
    }

    private void SetUnderAttack(bool underAttackState)
    {
        if (underAttack != underAttackState)
        {
            if (underAttackState)
            {
                buildingRenderer.material.SetFloat("_UnderAttack", 1);
            }
            else
            {
                buildingRenderer.material.SetFloat("_UnderAttack", 0);
            }
        }
        underAttack = underAttackState;
    }

    private void ConquerEffect()
    {
        if (!conquering)
        {
            conquering = true;
            conquerEffectElapsedTime = 0;
            buildingRenderer.material.SetFloat("_ShakeAmplitude", 0);
            alternateBuildingRenderer.material.SetFloat("_SizeFactor", 0);
            alternateBuildingRenderer.gameObject.SetActive(true);
        }

        float progress = conquerEffectElapsedTime / conquerEffectDuration;
        areaRenderer.material.SetFloat("_AlternateStartRadius", progress);

        if (progress < 0.5f)
        {
            buildingRenderer.material.SetFloat("_ShakeAmplitude", progress * 2);
        }
        else if (progress < 0.6f)
        {
            alternateBuildingRenderer.material.SetFloat("_SizeFactor", (progress - 0.5f) * 4);
        }
        else
        {
            // Rescale te progress to fall in the range [0,1]
            progress = (progress - 0.6f) / (1 - 0.6f);
            alternateBuildingRenderer.material.SetFloat("_SizeFactor", 1 + (0.2f / (1 + progress)) * Mathf.Sin(2 * Mathf.PI * 2 * progress));
        }
    }

    private void Conquer()
    {
        gameScenariosManager.updateEnemiesTarget(false);
        conquering = false;
        conquerEffectElapsedTime = 0;
        alternateBuildingRenderer.material.SetFloat("_SizeFactor", 1);
        areaRenderer.material.SetFloat("_Conquered", 1);
        conquered = true;
    }

    private void AdjustMaterials()
    {
        float conqueredFactor = (initialHitPoints - hitPoints) / (float)initialHitPoints;
        buildingRenderer.material.SetFloat("_ConquerFactor", conqueredFactor);
        areaRenderer.material.SetFloat("_ConquerFactor", conqueredFactor);
    }

    private void Reset()
    {
        hitPoints = initialHitPoints;
        buildingRenderer.material.SetFloat("_ConquerFactor", 0);
        buildingRenderer.material.SetFloat("_ShakeAmplitude", shakeAmplitude);
        buildingRenderer.material.SetFloat("_ShakeSpeed", shakeSpeed);
        alternateBuildingRenderer.material.SetFloat("_SizeFactor", 0);
        areaRenderer.material.SetFloat("_AlternateStartRadius", 0);
        areaRenderer.material.SetFloat("_ConquerFactor", 0);
        areaRenderer.material.SetFloat("_Conquered", 0);

        conquered = false;
    }
}
