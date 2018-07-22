using UnityEngine;

public static class Helpers
{
    public static bool GameObjectInLayerMask(GameObject go, LayerMask mask)
    {
        return (mask & (1 << go.layer)) != 0;
    }
}
