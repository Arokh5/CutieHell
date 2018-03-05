using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#region enums

public enum EnemyType { BASIC, RANGE, CONQUEROR }
public enum AttackType { ENEMY, WEAK, STRONG, WOLF, FOG, TRAP_BASIC, TRAP_AREA }
public enum SubZoneType { WEAK_TRAP, STRONG_TRAP, MONUMENT }

#endregion

#region interfaces

public interface IDamageable
{
    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    bool IsDead();
    // Called by Player (for AIEnemy) and by AIEnemy (for Building)
    bool TakeDamage(float dmg, AttackType attacktype);
}

public interface IRepairable
{
    // Called by Player
    bool HasFullHealth();
    // Called by Player
    void FullRepair();
    // Called by Player
    int GetRepairCost();
}

public interface IUsable
{
    // Called by Player
    bool CanUse();
    // Called by Player
    int GetUsageCost();
    // Called by Player
    bool Activate();
    // Called by Player
    void Deactivate();
}

#endregion

#region AI

public class AISpawnController
{
    float elapsedTime;
    uint currentWave;
    List<WaveInfo> wavesInfo;
    List<AISpawner> aiSpawners;
}

public class WaveInfo
{
    float waveDuration;
    List<SpawnInfo> spawnInfo;
}

public class SpawnInfo
{
    // Used by AISpawnController
    float spawnTime;
    // Used by AISpawnController
    uint spawnerID;
    // Used by AISpawner
    float spawnDuration;
    // Used by AISpawner
    List<EnemyType> enemiesToSpawn;
}

public abstract class AISpawner
{
    AIZoneController zonecontroller;
    List<SpawnInfo> activeSpawnInfos;

    // Called by AISpawnController
    public abstract void Spawn(SpawnInfo spawnInfo);
}

public abstract class AIEnemy : IDamageable
{
    private AIZoneController zoneController;
    SubZoneType currentSubZone;
    IDamageable currentTarget;

    // Called 

    // Called by AISpawner when instantiating an AIEnemy. This method should inform the ZoneController about this AIEnemy's creation
    public abstract void SetZoneController(AIZoneController zoneController);
    // Called by the ZoneController in case the Monument gets repaired (this will cause all AIEnemy to return to the ZoneController's area)
    // or when a Trap gets deactivated or when the area-type Trap explodes
    public abstract void SetCurrentTarget(IDamageable target);

    // IDamageable
    // Called by the AIPlayer or an Attack to determine if this AIEnemy should be targetted
    public abstract bool IsDead();
    // Called by the AIPlayer or an Attack to damage the AIEnemy
    public abstract bool TakeDamage(float dmg, AttackType attacktype);
    // Called by the Area-type Trap to retarget the AIEnemy after exploding
    public abstract void UpdateTarget();
}

public abstract class ScenarioController
{
    // Called by a ZoneController when its Monument has been conquered and an AIEnemy request for a Target
    public abstract AIZoneController GetAlternateZone(AIZoneController currentZone);
}

public abstract class AIZoneController
{
    uint zoneID;
    Monument monument;

    Building currentZoneTarget;

    // List that contains all AIEnemy that were spawned on this ZoneController's area and are still alive
    List<AIEnemy> aiEnemies;

    // Called by Monument when it gets repaired
    public abstract void OnMonumentRepaired();
    // Called by Monument when it gets conquered. The method is meant to open the door
    public abstract void OnMonumentTaken();

    // Called by Trap when it gets activated by Player
    public void OnTrapActivated(Building trap)
    {
    }
    // Called by Trap when it gets deactivated by Player
    public abstract void OnTrapDeactivated();

    // Called by AIEnemy when it finishes conquering a Building or when the trap it was attacking becomes inactive
    public abstract IDamageable GetTargetBuilding();
    // Called by AIEnemy during its configuration to add it to the aiEnemies list
    public abstract void AddEnemy(AIEnemy aiEnemy);
    // Called by AIEnemy in its OnDestroy method to remove from the aiEnemies list
    public abstract bool RemoveEnemy(AIEnemy aiEnemy);
}

public abstract class ZoneConnection
{
    uint zoneConnetionID;
    AIZoneController zone1;
    AIZoneController zone2;
    // Called by one of two Zonecontroller when its Monument gets conquered
    public abstract void Open();
    // Called by one of two Zonecontroller when its Monument gets repaired
    public abstract void Close();
}

#endregion

#region Buildings

public abstract class Building : IDamageable, IRepairable
{
    AIZoneController zoneController;
    float baseHealth;
    float health;
    SubZoneType zoneType;

    // IDamageable
    public abstract bool IsDead();
    public abstract bool TakeDamage(float dmg, AttackType attacktype);
    // IRepairable
    public abstract void FullRepair();
    public abstract bool HasFullHealth();
    public abstract int GetRepairCost();
}

public abstract class Trap : Building, IUsable
{
    uint trapID;
    bool isInUse;

    // IDamageable
    // If a call to this method causes the Trap to die, it should inform Player to get off the trap and call Deactivate
    public abstract override bool TakeDamage(float dmg, AttackType attacktype);

    // IUsable
    // Called by Player
    public abstract bool CanUse();
    // Called by Player
    public abstract int GetUsageCost();
    // Called by Player. A call to this method should inform the ZoneController
    public abstract bool Activate();
    // Called by Player. A call to this method should inform the ZoneController
    public abstract void Deactivate();
}

public abstract class Monument : Building
{
    // IDamageable
    // If a call to this method causes the Monument to die, it should inform the ZoneController
    public abstract override bool TakeDamage(float dmg, AttackType attacktype);
    // If this method is called, it should inform the ZoneController
    public abstract override void FullRepair();

}

#endregion


public abstract class Player
{
    // Called by Trap in case it gets conquered by AIEnemy
    public abstract void StopTrapUse();
}

public abstract class UIManager
{
    // Called by Player when using or earning Evil Points
    public abstract void SetEvilBarValue(int value);
    // Called by Player when increasing its maximum of Evil Point
    public abstract void SetEvilBarMaxValue(int maxValue);
    // Called by ZonesConnection when the connection gets opened
    public abstract void ZoneConnectionOpened(uint zoneConnectionID);
    // Called by Trap to update its remaining health
    public abstract void SetTrapHealth(uint zoneID, uint TrapID, float normalizedHealth);
    // Called by Trap to update its remaining health
    public abstract void SetMonumentHealth(uint zoneID, float normalizedHealth);
    // Called by AISpawnController to move the Wave indicator forward
    public abstract void SetWaveNumberAndProgress(uint waveNumber, float normalizedProgress);
}

public abstract class StatsManager
{
    // Called by AIEnemy upon dying
    public abstract void RegisterKill(EnemyType enemyType, AttackType attackType);
    // Called by Player when gaining EP (Evil Points)
    public abstract void RegisterEPGained(int epGained);
    // Called by Player when gaining EP but losing it due to having reached the max EP (Evil Points)
    public abstract void RegisterEPLost(int epLost);
    // Called by Player when using EP (Evil Points)
    public abstract void RegisterEPUsed(int epUsed);
}