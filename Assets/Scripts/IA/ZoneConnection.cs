using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneConnection : MonoBehaviour, IZoneTakenListener {

    #region Fields
    [SerializeField]
    [Tooltip("Optional: The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("Optional: The Cute Wall that Vlad can't walk through. This requires a referenceZone.")]
    ParticleSystem cuteWall;
    private List<AIEnemy> aiEnemiesInConnection = new List<AIEnemy>();
    private List<AIEnemy> toRemove = new List<AIEnemy>();
    private Player playerInConnection = null;
    #endregion

    #region MonoBehavior Methods
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Start()
    {
        if (referenceZone)
        {
            referenceZone.AddIZoneTakenListener(this);
            if (referenceZone.isConquered)
                Close();
            else
                Open();
        }
    }

    private void Update()
    {
        foreach (AIEnemy enemy in aiEnemiesInConnection)
        {
            if (enemy.IsDead())
                toRemove.Add(enemy);
        }
        foreach (AIEnemy enemy in toRemove)
        {
            aiEnemiesInConnection.Remove(enemy);
        }
        toRemove.Clear();
    }

    private void OnDestroy()
    {
        if (referenceZone)
            referenceZone.RemoveIZoneTakenListener(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        AIEnemy aiEnemy = other.GetComponent<AIEnemy>();
        if (aiEnemy)
        {
            aiEnemiesInConnection.Add(aiEnemy);
        }
        else
        {
            Player player = other.GetComponentInParent<Player>();
            if (player)
            {
                playerInConnection = player;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AIEnemy aiEnemy = other.GetComponent<AIEnemy>();
        if (aiEnemy)
        {
            aiEnemiesInConnection.Remove(aiEnemy);
        }
        else
        {
            Player player = other.GetComponentInParent<Player>();
            if (player)
            {
                playerInConnection = null;
            }
        }
    }
    #endregion

    #region Public Methods
    // IZoneTakenListener
    public void OnZoneTaken()
    {
        Close();
    }

    public bool ContainsEnemy(AIEnemy enemy)
    {
        return aiEnemiesInConnection.Contains(enemy);
    }

    public bool ContainsPlayer()
    {
        return playerInConnection != null;
    }
    #endregion

    #region Private Methods
    private void Open()
    {
        if (cuteWall)
        {
            cuteWall.Stop();
            cuteWall.gameObject.SetActive(false);
        }
    }

    private void Close()
    {
        if (cuteWall)
        {
            cuteWall.gameObject.SetActive(true);
            cuteWall.Play();
        }
    }
    #endregion
}
