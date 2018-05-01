using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool<T> where T : MonoBehaviour
{
    public T prefab;
    private Transform poolParent;
    private Stack<T> objectsAvailable = new Stack<T>();

    public ObjectPool(T prefab, Transform poolParent, int preFillAmount = 0)
    {
        UnityEngine.Assertions.Assert.IsNotNull(prefab, "ERROR: Attempting to construct an object pool using NULL as the prefab");
        UnityEngine.Assertions.Assert.IsNotNull(prefab, "ERROR: Attempting to construct an object pool using NULL as the poolParent");
        this.prefab = prefab;
        this.poolParent = poolParent;

        for (int i = 0; i < preFillAmount; ++i)
        {
            T pooledObject = Object.Instantiate(prefab);
            pooledObject.gameObject.SetActive(false);
            pooledObject.transform.SetParent(poolParent);
            objectsAvailable.Push(pooledObject);
        }
    }

    public T GetObject(bool objectActive = false)
    {
        return GetObject(null, Vector3.zero, Quaternion.identity, true, objectActive);
    }

    public T GetObject(Transform parent, bool objectActive = false)
    {
        return GetObject(parent, Vector3.zero, Quaternion.identity, false, objectActive);
    }

    public T GetObject(Transform parent, Vector3 position, bool useWorldSpace = true, bool objectActive = false)
    {
        return GetObject(parent, position, Quaternion.identity, useWorldSpace, objectActive);
    }

    public T GetObject(Transform parent, Vector3 position, Quaternion rotation, bool useWorldSpace = true, bool objectActive = false)
    {
        T pooledObject = null;
        if (objectsAvailable.Count > 0)
            pooledObject = objectsAvailable.Pop();
        else
            pooledObject = Object.Instantiate(prefab);

        pooledObject.transform.SetParent(parent);
        if(useWorldSpace)
        {
            pooledObject.transform.position = position;
            pooledObject.transform.rotation = rotation;
        }
        else
        {
            pooledObject.transform.localPosition = position;
            pooledObject.transform.localRotation = rotation;
        }

        pooledObject.gameObject.SetActive(objectActive);
        return pooledObject;
    }

    public void ReturnToPool(T pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.SetParent(poolParent);
        objectsAvailable.Push(pooledObject);
    }
}
