using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAI : MonoBehaviour {

    [Header("Global Stats")]
    [SerializeField]
    private Color m_spawnColor = new Color(1.0f, 0.0f, 0.0f, 0.3f);
    [SerializeField]
    private Vector3 m_spawnArea;

    [Header("AI Group Settings")]
    public ObjectAI[] AIObjects = new ObjectAI[3];
    public Vector3 spawnArea { get { return m_spawnArea; } }

    private Vector3 averagePosition = Vector3.zero;

    private int subTeamID = 0;
    private GameObject average;

    private List<GameObject> spawnedElements = new List<GameObject>();


    // Use this for initialization
    void Start() {
        CreateAIGroups();

        for (int i = 0; i < AIObjects.Length; i++) {
            StartCoroutine(SpawnNPC(i));
        }

    }

    public Vector3 GetAveragePosition() {
        averagePosition = Vector3.zero;
        int count = 0;
        for (int i = 0; i < AIObjects.Length; i++) {
            GameObject tempGroup = GameObject.Find(AIObjects[i].AIGroupName);
            Transform[] enemies = tempGroup.GetComponentsInChildren<Transform>();
            for (int n = 0; n < enemies.Length; n++) {
                averagePosition += enemies[n].position;
                count++;
            }
        }

        average.transform.position = averagePosition / count;

        return averagePosition / count;
    }

    IEnumerator SpawnNPC(int objectID) {
        while (true) {
            if (AIObjects[objectID].enableSpawner && AIObjects[objectID].objectPrefab != null) {

                //Initialize the enemies group on the corresponding game area
                GameObject tempGroup = GameObject.Find(AIObjects[objectID].AIGroupName);
                tempGroup.transform.parent = this.transform.parent;

                int a = tempGroup.GetComponentInChildren<Transform>().childCount;
                if (tempGroup.GetComponentInChildren<Transform>().childCount < AIObjects[objectID].maxAI) {
                    for (int n = 0; n < AIObjects[objectID].spawnAmount; n++) {
                        Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
                        
                        //Instantiate the temporary enemy
                        GameObject tempSpawn;
                        tempSpawn = Instantiate(AIObjects[objectID].objectPrefab, RandomPosition(), randomRotation);
                        tempSpawn.transform.parent = tempGroup.transform;

                        //tempSpawn.AddComponent<EnemyBehaviour>();
                        tempSpawn.GetComponent<EnemyBehaviour>().setAISubTeamID(AIObjects[objectID].AIGroupName + subTeamID.ToString());

                        tempSpawn.GetComponent<EnemyBehaviour>().SetArea(transform.parent.name);
                        tempSpawn.name = AIObjects[objectID].AIGroupName + spawnedElements.Count.ToString();
                        tempSpawn.GetComponent<EnemyBehaviour>().InitializeTarget();

                        spawnedElements.Add(tempSpawn);
                    }
                    subTeamID++;
                }
            }
            yield return new WaitForSeconds(AIObjects[objectID].spawnRate);
        }
    }

    public Vector3 RandomPosition() {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
            );
        randomPosition = transform.TransformPoint(randomPosition * 0.5f);
        return randomPosition;
    }
    

    void CreateAIGroups() {
        for (int i = 0; i < AIObjects.Length; i++) {
            GameObject m_AIGroupSpawn;

            m_AIGroupSpawn = new GameObject(AIObjects[i].AIGroupName);
            m_AIGroupSpawn.transform.parent = gameObject.transform;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = m_spawnColor;
        Gizmos.DrawCube(transform.position, spawnArea);
    }

    public List<GameObject> getSpawnedElements()
    {
        return spawnedElements;
    }
}
