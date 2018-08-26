using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    #region Fields
    [Range(0.0f, 1.0f)]
    public float alpha;
    private Graphic[] graphics;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        graphics = GetComponentsInChildren<Graphic>();
    }

    private void OnValidate()
    {
        if (graphics == null)
            graphics = GetComponentsInChildren<Graphic>();

        SetAlpha(alpha);
    }
    #endregion

    #region Private Methods
    private void SetAlpha(float alpha)
    {
        foreach (Graphic graphic in graphics)
        {
            Color c = graphic.color;
            c.a = alpha;
            graphic.color = c;
        }
    }
    #endregion
}
