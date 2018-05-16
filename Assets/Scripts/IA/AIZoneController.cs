using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZoneController : MonoBehaviour
{
    #region Fields
    [HideInInspector]
    public int zoneID;
    public bool isFinalZone = false;
    public Monument monument = null;
    public Trap[] traps;
    [HideInInspector]
    public bool monumentTaken = false;

    [SerializeField]
    private ScenarioController scenarioController;
    [SerializeField]
    private PathsController pathsController;
    [SerializeField]
    [ShowOnly]
    private Building currentZoneTarget;

    // List that contains all AIEnemy that were spawned on this ZoneController's area and are still alive
    [SerializeField]
    private List<AIEnemy> aiEnemies;
    public List<BuildingEffects> buildingEffects;

    // Properties used to draw an alternate texture in the proximity of enemies
    private int maxElements = 128; // IMPORTANT: This number must be reflected in the TextureChanger.shader file
    private int maxBuildings = 8; // IMPORTANT: This number must be reflected in the TextureChanger.shader file
    private Vector4[] aiPositions;
    private float[] buildingsBlendStartRadius;
    private int activeEnemies = 0;


    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (!scenarioController)
        {
            scenarioController = GetComponentInParent<ScenarioController>();
            UnityEngine.Assertions.Assert.IsNotNull(scenarioController, "ERROR: Scenario not set for AIZoneController in gameObject '" + gameObject.name + "'");
        }

        UnityEngine.Assertions.Assert.IsTrue(buildingEffects.Count < maxBuildings, "ERROR: The amount of buildings is larger than the maximum amount of building accepted by the TextureChanger shader. The behaviour of the shader will be undefined!");
        aiPositions = new Vector4[maxElements];
        buildingsBlendStartRadius = new float[maxBuildings];
    }

    private void Start()
    {
        if (!monument)
        {
            monumentTaken = true;
            monument = scenarioController.GetAlternateTarget(this);
        }
        currentZoneTarget = monument;
    }

    private void Update()
    {
        /* Trap as target */
        if (currentZoneTarget.GetType() == typeof(Trap))
        {
            foreach (AIEnemy aiEnemy in aiEnemies)
            {
                /* Consider distance in XZ-Plane */
                Vector3 buildingToEnemy = aiEnemy.transform.position - currentZoneTarget.transform.position;
                buildingToEnemy.y = 0;
                if (buildingToEnemy.sqrMagnitude < currentZoneTarget.attractionRadius * currentZoneTarget.attractionRadius)
                {
                    aiEnemy.SetCurrentTarget(currentZoneTarget);
                }
            }
        }
        int skippedEnemies = 0;
        activeEnemies = 0;

        /* Shader data update */
        for (int i = 0; i < buildingEffects.Count + aiEnemies.Count && i < maxElements; ++i)
        {
            if (i < buildingEffects.Count)
            {
                aiPositions[i].x = buildingEffects[i].transform.position.x;
                aiPositions[i].y = buildingEffects[i].transform.position.y;
                aiPositions[i].z = buildingEffects[i].transform.position.z;
                /* The w component is used to pass the effectOnMapRadius of the building */
                aiPositions[i].w = buildingEffects[i].effectOnMapRadius;
                
                buildingsBlendStartRadius[i] = buildingEffects[i].GetBlendRadius();
            }
            else
            {
                if (aiEnemies[i - buildingEffects.Count].gameObject.activeSelf)
                {
                    aiPositions[i - skippedEnemies].x = aiEnemies[i - buildingEffects.Count].transform.position.x;
                    aiPositions[i - skippedEnemies].y = aiEnemies[i - buildingEffects.Count].transform.position.y;
                    aiPositions[i - skippedEnemies].z = aiEnemies[i - buildingEffects.Count].transform.position.z;
                    aiPositions[i - skippedEnemies].w = aiEnemies[i - buildingEffects.Count].effectOnMapRadius;
                    ++activeEnemies;
                }
                else ++skippedEnemies;
            }
        }
    }
    #endregion

    #region Public Methods
    public void UpdateMaterialWithEnemyPositions(Material material)
    {
        material.SetInt("_ActiveEnemies", activeEnemies);
        material.SetInt("_BuildingsCount", buildingEffects.Count);
        material.SetVectorArray("_AiPositions", aiPositions);
        material.SetFloatArray("_BuildingsBlendStartRadius", buildingsBlendStartRadius);
    }

    // Called by Buildings to obtain zoneId
    public int GetZoneId()
    {
        return zoneID;
    }

    // Called by Monument when it gets repaired
    public void OnMonumentRecovered()
    {
        monumentTaken = false;
        currentZoneTarget = monument;
        OnTargetBuildingChanged();
    }

    // Called by Monument when it gets conquered. The method is meant to open the door
    public void OnMonumentTaken()
    {
        monumentTaken = true;

        foreach (Trap trap in traps)
        {
            trap.TakeDamage(trap.GetMaxHealth(), AttackType.NONE);
        }

        if (isFinalZone)
            scenarioController.OnFinalZoneConquered();
        else
        {
            currentZoneTarget = scenarioController.GetAlternateTarget(this);
            OnTargetBuildingChanged();
        }
    }

    // Called by Trap when it gets activated by Player
    public void OnTrapActivated(Building trap)
    {
        currentZoneTarget = trap;
        OnTargetBuildingChanged();
    }

    // Called by Trap when it gets deactivated by Player
    public void OnTrapDeactivated()
    {
        currentZoneTarget = monument;
        OnTargetBuildingChanged();
    }

    // Called by AIEnemy when it finishes conquering a Building or when the trap it was attacking becomes inactive
    public Building GetTargetBuilding(Transform location)
    {
        Vector3 buildingToLocation = location.position - currentZoneTarget.transform.position;
        buildingToLocation.y = 0;
        if (buildingToLocation.sqrMagnitude < currentZoneTarget.attractionRadius * currentZoneTarget.attractionRadius)
        {
            return currentZoneTarget;
        }
        else
        {
            return monument;
        }
    }

    public List<PathNode> GetPath(Vector3 startingPos)
    {
        return pathsController.GetPath(startingPos);
    }


    // Called by AIEnemy during its configuration to add it to the aiEnemies list
    public void AddEnemy(AIEnemy aiEnemy)
    {
        if (!aiEnemies.Contains(aiEnemy))
        {
            aiEnemies.Add(aiEnemy);
            /* If we just added a first enemy*/
            if (aiEnemies.Count == 1)
            {
                scenarioController.OnZoneNotEmpty();
            }
        }
    }

    // Called by AIEnemy when dying or changing zoneController
    public bool RemoveEnemy(AIEnemy aiEnemy)
    {
        bool removed;
        removed = aiEnemies.Remove(aiEnemy);
        if (aiEnemies.Count == 0)
        {
            scenarioController.OnZoneEmpty();
        }
        return removed;
    }

    public bool HasEnemies()
    {
        return aiEnemies.Count > 0;
    }

    public void DestroyAllEnemies()
    {
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            aiEnemy.DieAfterMatch();
            //Destroy(aiEnemy.gameObject);
        }
        aiEnemies.Clear();
    }

    public List<AIEnemy> GetEnemiesWithinRange(Transform originPoint, float range)
    {
        List<AIEnemy> enemiesInRange = new List<AIEnemy>();
        for (int i = 0; i < aiEnemies.Count; i++)
        {
            AIEnemy aIEnemy = aiEnemies[i];
            if (Vector3.Distance(aIEnemy.transform.position, originPoint.transform.position) < range)
            {
                enemiesInRange.Add(aIEnemy);
            }
        }
        return enemiesInRange;
    }

    public List<AIEnemy> GetZoneEnemies()
    {
        return aiEnemies;
    }
    
    #endregion

    #region Private Methods
    private void OnTargetBuildingChanged()
    {
        if (currentZoneTarget.attractionRadius != 0)
        {
            foreach (AIEnemy enemy in aiEnemies)
            {
                if (currentZoneTarget.attractionRadius < 0)
                {
                    enemy.SetCurrentTarget(currentZoneTarget);
                }
                else
                {
                    /* Consider distance in XZ-Plane */
                    Vector3 buildingToEnemy = enemy.transform.position - currentZoneTarget.transform.position;
                    buildingToEnemy.y = 0;
                    if (buildingToEnemy.sqrMagnitude < currentZoneTarget.attractionRadius * currentZoneTarget.attractionRadius)
                    {
                        enemy.SetCurrentTarget(currentZoneTarget);
                    }
                }
            }
        }
    }
    #endregion
}
