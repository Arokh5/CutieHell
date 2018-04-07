using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private int pathNodeLayer = -1;
    public float radius = 5;
    [SerializeField]
    private bool isEndOfPath = false;
    public List<PathNode> outlets = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(pathNodeLayer >= 0, "ERROR: PathNode in gameObject '" + gameObject.name + "' doesn't have a valid pathNodeLayer. Valid values are 0 or higher!");
        if (isEndOfPath)
            UnityEngine.Assertions.Assert.IsTrue(outlets == null || outlets.Count == 0, "ERROR: PathNode in gameObject '" + gameObject.name + "' is marked as end of path and shouldn't have any outlets!");
        else
            UnityEngine.Assertions.Assert.IsTrue(outlets != null && outlets.Count >= 2 || outlets != null && outlets.Count >= 1 && outlets[0].pathNodeLayer < pathNodeLayer, "ERROR: PathNode in gameObject '" + gameObject.name + "' contains less than 2 outlets (Count: " + outlets.Count + "). All PathNodes should have at least 2 outlets (or 1 from a lower-numbered layer)!");
    }
    #endregion

    #region Public Methods
    public bool IsEndOfPath()
    {
        return isEndOfPath;
    }

    public PathNode GetRandomOutlet(PathNode excludedPathNode)
    {
        if (isEndOfPath || outlets.Count == 0 || outlets.Count == 1 && outlets[0] == excludedPathNode)
            return null;

        int excludedIndex = -1;
        if (excludedPathNode)
            excludedIndex = outlets.IndexOf(excludedPathNode);

        int index = -1;
        if (excludedIndex == -1)
            index = Random.Range(0, outlets.Count);
        else
        {
            index = Random.Range(0, outlets.Count - 1);
            if (index >= excludedIndex)
                ++index;
        }

        return outlets[index];
    }
    #endregion
}
