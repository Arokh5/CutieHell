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

    public static void DrawArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawLine(from, to);

        Vector3 direction = to - from;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(to, left * arrowHeadLength);
        Gizmos.DrawRay(to, right * arrowHeadLength);
    }

    public static void DrawWireRectangularCuboid(Transform referenceCenter, float xSize, float ySize, float zSize, bool rotates = false)
    {
        // Calculate direction vectors
        Vector3 xDir = rotates ? referenceCenter.right : Vector3.right;
        Vector3 yDir = rotates ? referenceCenter.up : Vector3.up;
        Vector3 zDir = rotates ? referenceCenter.forward : Vector3.forward;

        // Calculate corner values (### stand for x, y and z respectively, n and p means negative and positive)
        Vector3 center = referenceCenter.position;
        Vector3 nnn = center - xDir * (0.5f * xSize) - yDir * (0.5f * ySize) - zDir * (0.5f * zSize);
        Vector3 nnp = center - xDir * (0.5f * xSize) - yDir * (0.5f * ySize) + zDir * (0.5f * zSize);
        Vector3 npp = center - xDir * (0.5f * xSize) + yDir * (0.5f * ySize) + zDir * (0.5f * zSize);
        Vector3 npn = center - xDir * (0.5f * xSize) + yDir * (0.5f * ySize) - zDir * (0.5f * zSize);
        Vector3 pnn = center + xDir * (0.5f * xSize) - yDir * (0.5f * ySize) - zDir * (0.5f * zSize);
        Vector3 pnp = center + xDir * (0.5f * xSize) - yDir * (0.5f * ySize) + zDir * (0.5f * zSize);
        Vector3 ppp = center + xDir * (0.5f * xSize) + yDir * (0.5f * ySize) + zDir * (0.5f * zSize);
        Vector3 ppn = center + xDir * (0.5f * xSize) + yDir * (0.5f * ySize) - zDir * (0.5f * zSize);

        // Draw x lines
        Gizmos.DrawLine(nnn, pnn);
        Gizmos.DrawLine(nnp, pnp);
        Gizmos.DrawLine(npp, ppp);
        Gizmos.DrawLine(npp, ppp);

        // Draw y lines
        Gizmos.DrawLine(nnn, npn);
        Gizmos.DrawLine(pnn, ppn);
        Gizmos.DrawLine(pnp, ppp);
        Gizmos.DrawLine(nnp, npp);

        // Draw z lines
        Gizmos.DrawLine(nnn, nnp);
        Gizmos.DrawLine(pnn, pnp);
        Gizmos.DrawLine(ppn, ppp);
        Gizmos.DrawLine(npn, npp);
    }
}
