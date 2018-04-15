using UnityEngine;

public static class GizmosHelper
{
    public static void DrawLine(Vector3 from, Vector3 to, int width)
    {
        if (width <= 0)
            return;

        if (width == 1)
            Gizmos.DrawLine(from, to);
        else
        {
            Camera camara = Camera.current;
            if (camara == null)
            {
                return;
            }
            Vector3 v1 = (to - from).normalized; // line direction
            Vector3 v2 = (camara.transform.position - from).normalized; // direction to camera
            Vector3 normal = Vector3.Cross(v1, v2); // normal vector
            for (int i = 0; i < width; ++i)
            {
                Vector3 offset = normal * ((float)i/(width - 1) - 0.5f);
                Gizmos.DrawLine(from + offset, to + offset);
            }
        }
    }
}
