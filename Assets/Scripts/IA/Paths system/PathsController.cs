using System.Collections.Generic;
using UnityEngine;

public class PathsController : MonoBehaviour
{
    [System.Serializable]
    private class Path
    {
        public List<PathNode> nodes;
    }

    #region Fields
    [SerializeField]
    private List<Path> predeterminedPaths;

    private List<PathNode> allPathNodes;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        allPathNodes = new List<PathNode>(GetComponentsInChildren<PathNode>());
        ValidatePredeterminedPaths();
    }

    private void OnValidate()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        const float radius = 0.25f;

        foreach (Path path in predeterminedPaths)
        {
            if (path.nodes.Count > 0)
            {
                Gizmos.color = Color.red;
                if (path.nodes[0])
                {
                    Gizmos.DrawSphere(path.nodes[0].transform.position + Vector3.up, radius);
                }

                Gizmos.color = Color.blue;
                Vector3 previousNodePos;
                Vector3 currentNodePos;
                for (int i = 1; i < path.nodes.Count; ++i)
                {
                    if (path.nodes[i - 1] && path.nodes[i])
                    {
                        previousNodePos = path.nodes[i - 1].transform.position + Vector3.up;
                        currentNodePos = path.nodes[i].transform.position + Vector3.up;
                        GizmosHelper.DrawArrow(previousNodePos, currentNodePos, 1.0f);
                        if (i == path.nodes.Count - 1)
                        {
                            Gizmos.color = Color.green;
                        }
                        Gizmos.DrawSphere(currentNodePos, radius);
                    }
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public List<PathNode> GetPath(Vector3 startingPos)
    {
        List<PathNode> path = new List<PathNode>();

        PathNode currentNode = FindClosestPathNode(startingPos);

        if (currentNode.IsEndOfPath())
            path.Add(currentNode);
        else
        {
            if (Vector3.Distance(startingPos, currentNode.transform.position) > currentNode.radius)
                path.Add(currentNode);

            PathNode previousNode = null;
            while (!currentNode.IsEndOfPath())
            {
                PathNode nextNode = currentNode.GetRandomOutlet(previousNode);
                path.Add(nextNode);
                previousNode = currentNode;
                currentNode = nextNode;
            }
        }

        return path;
    }

    public List<PathNode> GetPath(Vector3 startingPos, int index)
    {
        if (index >= 0 && index < predeterminedPaths.Count)
        {
            return predeterminedPaths[index].nodes;
        }
        return GetPath(startingPos);
    }
    #endregion

    #region Private Methods
    private PathNode FindClosestPathNode(Vector3 referencePos)
    {
        PathNode closestNode = null;
        float distance = float.MaxValue;

        foreach (PathNode pathNode in allPathNodes)
        {
            float currentNodeDistance = Vector3.Distance(referencePos, pathNode.transform.position);

            // Early exit in case the referencePos is within a certain node's radius,
            // which guarantees that said node is the closest one to the referencePos.
            if (currentNodeDistance < pathNode.radius)
                return pathNode;

            if (currentNodeDistance < distance)
            {
                distance = currentNodeDistance;
                closestNode = pathNode;
            }
        }

        return closestNode;
    }

    private bool ValidatePredeterminedPaths()
    {
        bool isValid = true;

        for (int i = 0; i < predeterminedPaths.Count; ++i)
        {
            Path path = predeterminedPaths[i];
            if (path.nodes.Count > 0)
            {
                for (int j = 0; j < path.nodes.Count; ++j)
                {
                    if (!path.nodes[j])
                    {
                        Debug.LogError("ERROR: The node at index " + j + " in the path at index " + i + " in PathsController in GameObject '" + gameObject.name + "' is null!");
                    }
                }
            }
            else
            {
                Debug.LogError("ERROR: The path at index " + i + " in PathsController in GameObject '" + gameObject.name + "' is empty!");
            }
        }

        return isValid;
    }
    #endregion
}
