using UnityEngine;

public interface IObstructionsHandler
{
    void HideObstructions(Vector3 rayStart, Vector3 rayEnd);
    void ShowObstructions();
}
