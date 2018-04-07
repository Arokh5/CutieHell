﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private List<PathNode> startingPathNodes = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(startingPathNodes != null && startingPathNodes.Count > 0, "ERROR: The PathsController in gameObject '" + gameObject.name + "' doesn't have any startingPathNodes. There should be at least 1 element in startingPathNodes!");
    }
    #endregion

    #region Public Methods
    public List<PathNode> GetPath(Vector3 startingPos)
    {
        List<PathNode> path = null;

        PathNode currentNode = FindClosestPathNode(startingPos);

        if (currentNode.IsEndOfPath())
            path.Add(currentNode);
        else
        {
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

        Stack<PathNode> stack = new Stack<PathNode>();
        foreach (PathNode outletNode in startingPathNodes)
        {
            stack.Push(outletNode);
        }

        while (stack.Count > 0)
        {
            PathNode pathNode = stack.Pop();
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

            foreach (PathNode outletNode in pathNode.outlets)
            {
                stack.Push(outletNode);
            }
        }

        return closestNode;
    }
    #endregion
}
