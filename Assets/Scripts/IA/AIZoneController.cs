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

    [Header("Zone Conquering")]
    public TeleportTarget playerExpelTarget;
    public List<BuildingEffects> buildingEffects;

    private List<IZoneTakenListener> zoneTakenListeners = new List<IZoneTakenListener>();

    [ShowOnly]
    [SerializeField]
    private List<CuteEffect> cuteEffects = new List<CuteEffect>();

    #endregion

    #region Properties
    public bool isConquered
    {
        get
        {
            return !hasMonument || monumentTaken;
        }
    }
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
        {
            textureChangerSource.AddTextureChanger(effect);
            foreach (EvilEffect evilEffect in effect.evilEffects)
                textureChangerSource.AddTextureChanger(evilEffect);
        }
    }
    #endregion

    #region Public Methods
    // Called by Buildings to obtain zoneId
    public int GetZoneId()
    {
        return zoneID;
    }

    public void UpdateZoneTarget()
    {
        if (isConquered)
        {
            Monument newTarget = scenarioController.GetAlternateTarget(this);
            if (!hasMonument)
                monument = newTarget;

            currentZoneTarget = newTarget;
            OnTargetBuildingChanged();
        }
    }

    // Called by Monument when it gets conquered.
    public void OnMonumentTaken()
    {
        monumentTaken = true;
        for(int i = 0; i < fogWallID.Length; i++)
        {
            fogWallsManager.DeactivateFogWall(fogWallID[i]);
        }
        foreach(NavMeshObstacle blockage in blockages)
        {
            blockage.gameObject.SetActive(false);
        }
        if(!isFinalZone)
            GameManager.instance.GetPlayer1().currentZonePlaying = 3 - iconIndex;


        if (isFinalZone)
            scenarioController.OnFinalZoneConquered();
        else
        {
            currentZoneTarget = scenarioController.GetAlternateTarget(this);
            OnTargetBuildingChanged();
        }

        if (playerExpelTarget != null)
            GameManager.instance.GetPlayer1().ExpelFromZone(this, playerExpelTarget);

        foreach (IZoneTakenListener listener in zoneTakenListeners)
            listener.OnZoneTaken();

        UIManager.instance.markersController.MonumentConquered(iconIndex);
    }

    // Called by AIEnemy when it finishes conquering a Building or when the trap it was attacking becomes inactive
    public Building GetTargetBuilding(Transform location)
    {
        return currentZoneTarget;
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
        }
    }

    // Called by CuteEffects to unregister
    public bool RemoveCuteEffect(CuteEffect cuteEffect)
    {
        return cuteEffects.Remove(cuteEffect);
    }

    // Called by Monument to pass information to CuteEffects
    public void InformMonumentDamage(float normalizedDamage)
    {
        foreach (CuteEffect cuteEffect in cuteEffects)
        {
            cuteEffect.InformMonumentDamage(normalizedDamage);
        }
    }

    // Called by PathsChanger to modify the Zone's Paths
    public void SetPathsController(PathsController newController)
    {
        if (newController)
            pathsController = newController;
    }

    // Called by IZoneTakenListeners to register
    public void AddIZoneTakenListener(IZoneTakenListener listener)
    {
        if (!zoneTakenListeners.Contains(listener))
        {
            zoneTakenListeners.Add(listener);
        }
    }

    // Called by IZoneTakenListeners to unregister
    public bool RemoveIZoneTakenListener(IZoneTakenListener listener)
    {
        return zoneTakenListeners.Remove(listener);
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
        foreach (AIEnemy enemy in aiEnemies)
        {
            enemy.SetCurrentTarget(currentZoneTarget);
        }
    }
    #endregion
}
