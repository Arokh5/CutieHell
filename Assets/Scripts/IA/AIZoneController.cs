using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIZoneController : MonoBehaviour
{
    #region Fields
    [HideInInspector]
    public int zoneID;
    public int iconIndex;
    public bool isFinalZone = false;
    public Monument monument = null;
    public Trap[] traps;
    [HideInInspector]
    public bool monumentTaken = false;
    [HideInInspector]
    public bool hasMonument;

    private ScenarioController scenarioController;
    private TextureChangerSource textureChangerSource;
    [SerializeField]
    private PathsController pathsController;
    [SerializeField]
    [ShowOnly]
    private Building currentZoneTarget;

    [Header("Bridges to next zone")]
    [SerializeField]
    private LivingFogManager fogWallsManager;
    [SerializeField]
    private int[] fogWallID;
    [SerializeField]
    NavMeshObstacle[] blockages;

    [Space]
    // List that contains all AIEnemy that were spawned on this ZoneController's area and are still alive
    [SerializeField]
    private List<AIEnemy> aiEnemies;
    public List<BuildingEffects> buildingEffects;

    [ShowOnly]
    [SerializeField]
    private List<CuteEffect> cuteEffects = new List<CuteEffect>();

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (!scenarioController)
        {
            scenarioController = GetComponentInParent<ScenarioController>();
            UnityEngine.Assertions.Assert.IsNotNull(scenarioController, "ERROR: AIZoneController could not find a ScenarioController in its parent hierarchy in gameObject '" + gameObject.name + "'");
        }
        if (!textureChangerSource)
        {
            textureChangerSource = GetComponentInParent<TextureChangerSource>();
            UnityEngine.Assertions.Assert.IsNotNull(textureChangerSource, "ERROR: AIZoneController could not find a TextureChangerSource in its parent hierarchy in gameObject '" + gameObject.name + "'");
        }

        for (int i = 0; i < buildingEffects.Count; ++i)
        {
            if (buildingEffects[i] == null)
                Debug.LogError("ERROR: AIZoneController in gameObject '" + gameObject.name + "' contains a 'null' in index " + i + " of its buildingEffects!");
        }

        hasMonument = monument != null;
    }

    private void Start()
    {
        if (!hasMonument)
            monument = scenarioController.GetAlternateTarget(this);

        currentZoneTarget = monument;

        foreach (BuildingEffects effect in buildingEffects)
            textureChangerSource.AddTextureChanger(effect);
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
    }
    #endregion

    #region Public Methods
    // Called by Buildings to obtain zoneId
    public int GetZoneId()
    {
        return zoneID;
    }

    // Called by Monument when it gets conquered. The method is meant to open the door
    public void OnMonumentTaken()
    {
        monumentTaken = true;
        for(int i = 0; i < fogWallID.Length; i++)
        {
            fogWallsManager.DeactivateFogWall(fogWallID[i]);
        }
        foreach (Trap trap in traps)
        {
            trap.TakeDamage(trap.GetMaxHealth(), AttackType.NONE);
        }
        foreach(NavMeshObstacle blockage in blockages)
        {
            blockage.gameObject.SetActive(false);
        }

        if (isFinalZone)
            scenarioController.OnFinalZoneConquered();
        else
        {
            currentZoneTarget = scenarioController.GetAlternateTarget(this);
            OnTargetBuildingChanged();
        }

        UIManager.instance.indicatorsController.MonumentConquered(iconIndex);
        UIManager.instance.markersController.MonumentConquered(iconIndex);
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
        if (pathsController)
            return pathsController.GetPath(startingPos);
        return null;
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
        bool removed = aiEnemies.Remove(aiEnemy);
        if (removed)
        {
            if (aiEnemies.Count == 0)
            {
                scenarioController.OnZoneEmpty();
            }
        }
        return removed;
    }

    // Called by CuteEffects to register
    public void AddCuteEffects(CuteEffect cuteEffect)
    {
        if (!cuteEffects.Contains(cuteEffect))
        {
            cuteEffects.Add(cuteEffect);
            textureChangerSource.AddTextureChanger(cuteEffect);
        }
    }

    // Called by CuteEffects to unregister
    public bool RemoveCuteEffect(CuteEffect cuteEffect)
    {
        if (cuteEffects.Remove(cuteEffect))
        {
            textureChangerSource.RemoveTextureChanger(cuteEffect);
            return true;
        }
        return false;
    }

    // Called by Monument to pass information to CuteEffects
    public void InformMonumentDamage(float normalizedDamage)
    {
        foreach (CuteEffect cuteEffect in cuteEffects)
        {
            cuteEffect.InformMonumentDamage(normalizedDamage);
        }
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

    public int GetZoneEnemiesCount()
    {
        return aiEnemies.Count;
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
