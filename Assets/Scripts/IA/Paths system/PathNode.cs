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

        foreach (PathNode pathNode in outlets)
        {
            UnityEngine.Assertions.Assert.IsNotNull(pathNode, "ERROR: PathNode in gameObject '" + gameObject.name + "' has a null as an outlet!");
            UnityEngine.Assertions.Assert.IsTrue(pathNode != this, "ERROR: PathNode in gameObject '" + gameObject.name + "' has set itself as an outlet!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color layerColor = GetLayerColor(pathNodeLayer);
        Gizmos.color = layerColor;
        Gizmos.DrawWireSphere(transform.position, radius);

        foreach (PathNode pathNode in outlets)
        {
            if (!pathNode)
                continue;

            Gizmos.color = layerColor;
            Vector3 midPoint = transform.position + 0.5f * (pathNode.transform.position - transform.position);
            GizmosHelper.DrawArrow(transform.position, midPoint, 1.5f, 25);
            if (pathNode.pathNodeLayer == pathNodeLayer)
            {
                GizmosHelper.DrawLine(transform.position, pathNode.transform.position, 3);
            }
            else
            {
                Color lineEndColor = GetLayerColor(pathNode.pathNodeLayer);
                GizmosHelper.DrawLine(transform.position, midPoint, 3);
                Gizmos.color = lineEndColor;
                GizmosHelper.DrawLine(midPoint, pathNode.transform.position, 3);
            }
        }
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

    #region Private Methods
    private Color GetLayerColor(int layer)
    {
        Color layerColor;
        switch (layer)
        {
            case 1:
                layerColor = Color.green;
                break;
            case 2:
                layerColor = Color.yellow;
                break;
            case 3:
                layerColor = Color.blue;
                break;
            case 4:
                layerColor = Color.magenta;
                break;
            default:
                layerColor = Color.white;
                break;
        }
        return layerColor;
    }
    #endregion
}
