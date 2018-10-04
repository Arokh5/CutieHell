using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstructionHandler : IObstructionsHandler
{
    #region Fields
    private List<Renderer> hiddenRenderers = new List<Renderer>();
    private RaycastHit[] hitsBuffer = new RaycastHit[16];
    private List<Renderer> renderersBuffer = new List<Renderer>();
    #endregion

    #region Public Methods
    // IObstructionsHandler
    public void HideObstructions(Vector3 rayStart, Vector3 rayEnd)
    {
        float distance = (rayEnd - rayStart).magnitude;
        Vector3 direction = (rayEnd - rayStart) / distance;

        int hitCount = Physics.RaycastNonAlloc(rayStart, direction, hitsBuffer, distance);
        for (int i = 0; i < hitCount; ++i)
        {
            RaycastHit hit = hitsBuffer[i];
            hit.transform.GetComponentsInChildren(true, renderersBuffer);
            foreach (Renderer renderer in renderersBuffer)
            {
                if (!renderer.gameObject.CompareTag("PlayerMesh"))
                {
                    renderer.enabled = false;
                    hiddenRenderers.Add(renderer);
                }
            }
            renderersBuffer.Clear();
        }
    }

    // IObstructionsHandler
    public void ShowObstructions()
    {
        foreach (Renderer renderer in hiddenRenderers)
        {
            renderer.enabled = true;
        }
    }
    #endregion
}
