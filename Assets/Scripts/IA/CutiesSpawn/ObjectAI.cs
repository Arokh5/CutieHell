using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectAI {

    public string AIGroupName { get { return m_AIGroupName; } }
    public GameObject objectPrefab { get { return m_prefab; } }
    public int maxAI { get { return m_maxAI; } }
    public int spawnRate { get { return m_spawnRate; } }
    public int spawnAmount { get { return m_spawnAmount; } }
    public bool enableSpawner { get { return m_enableSpawner; } }

    [Header("AI Group Stats")]
    [SerializeField]
    private string m_AIGroupName;
    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    [Range(0f, 100f)]
    private int m_maxAI;
    [SerializeField]
    [Range(0f, 20f)]
    private int m_spawnRate;
    [SerializeField]
    [Range(0f, 10f)]
    private int m_spawnAmount;

    [Header("Main Settings")]
    [SerializeField]
    private bool m_enableSpawner;


    public ObjectAI(string name, GameObject prefab, int maxAI, int spawnRate, int spawnAmount) {
        m_AIGroupName = name;
        m_prefab = prefab;
        m_maxAI = maxAI;
        m_spawnRate = spawnRate;
        m_spawnAmount = spawnAmount;
    }

    public void SetValues(int maxAI, int spawnRate, int spawnAmount) {
        m_maxAI = maxAI;
        m_spawnRate = spawnRate;
        m_spawnAmount = spawnAmount;
    }

    public GameObject GetGameObject()
    {
        return this.objectPrefab;
    }
}
