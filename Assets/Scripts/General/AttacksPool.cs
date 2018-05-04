// Warning CS0649: Field '#fieldname#' is never assigned to, and will always have its default value null (CS0649) (Assembly-CSharp)
// Warning was raised for the following fields: EnemyAttackPrefab::enemyType and EnemyAttackPrefab::attackPrefab
// Warning was disabled because these private fields are serialized and assigned through the inspector
#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksPool : MonoBehaviour
{
    [System.Serializable]
    private class EnemyAttackPrefab
    {
        public EnemyType enemyType;
        public GameObject attackPrefab;
    }

    #region Fields
    public static AttacksPool instance;
    private Dictionary<EnemyType, ObjectPool<Transform>> attacksPool = new Dictionary<EnemyType, ObjectPool<Transform>>();

    [SerializeField]
    private EnemyAttackPrefab[] attackPrefabs;
    public Transform pooledAttacks;
    public Transform activeAttacks;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        UnityEngine.Assertions.Assert.IsNotNull(pooledAttacks, "ERROR: pooledAttacks Transform not assigned for BearAttacksPool in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(activeAttacks, "ERROR: activeAttacks Transform not assigned for BearAttacksPool in gameObject '" + gameObject.name + "'");
    }

    private void Start()
    {
        foreach (EnemyAttackPrefab enemyAttackPrefab in attackPrefabs)
        {
            attacksPool[enemyAttackPrefab.enemyType] = new ObjectPool<Transform>(enemyAttackPrefab.attackPrefab.transform, pooledAttacks);
        }
    }
    #endregion

    #region Public Methods
    public GameObject GetAttackObject(EnemyType enemyType, Vector3 position, Quaternion rotation)
    {
        if (attacksPool.ContainsKey(enemyType))
            return attacksPool[enemyType].GetObject(activeAttacks, position, rotation, true, true).gameObject;
        else
            return null;
    }

    public void ReturnAttackObject(EnemyType enemyType, GameObject attack)
    {
        if (attacksPool.ContainsKey(enemyType))
            attacksPool[enemyType].ReturnToPool(attack.transform);
        else
            Destroy(attack);
    }
    #endregion
}
