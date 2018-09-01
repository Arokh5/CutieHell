using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsController : MonoBehaviour
{
    #region Fields
    private List<PathNode> allPathNodes;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        allPathNodes = new List<PathNode>(GetComponentsInChildren<PathNode>());
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
    #endregion

    #region Private Methods
    PathNode FindClosestPathNode(Vector3 referencePos)
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
    #endregion
}
