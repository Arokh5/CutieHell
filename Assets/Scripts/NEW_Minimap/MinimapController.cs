using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [System.Serializable]
    private class WorldReference
    {
        public Transform bottomLeft;
        public Transform topRight;
    }

    #region Fields
    public static MinimapController instance;

    [SerializeField]
    private WorldReference worldReference;
    [SerializeField]
    private RectTransform elementsParent;
    [SerializeField]
    private Image representationPrefab;

    private List<MinimapElement> minimapElements = new List<MinimapElement>();
    private List<Image> minimapImages = new List<Image>();

    private Vector2 worldBottomLeft;
    private Vector2 worldTopRight;
    private float worldWidth;
    private float worldHeight;
    private float minimapWidth;
    private float minimapHeight;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

    }

    private void Start()
    {
        worldBottomLeft = new Vector2(worldReference.bottomLeft.position.x, worldReference.bottomLeft.position.z);
        worldTopRight = new Vector2(worldReference.topRight.position.x, worldReference.topRight.position.z);
        worldWidth = worldTopRight.x - worldBottomLeft.x;
        worldHeight = worldTopRight.y - worldBottomLeft.y;
        minimapWidth = elementsParent.sizeDelta.x;
        minimapHeight = elementsParent.sizeDelta.y;
    }

    private void Update()
    {
        for (int i = 0; i < minimapElements.Count; ++i)
        {
            Vector2 newPos = WorldToMinimap(minimapElements[i].transform.position);
            minimapImages[i].rectTransform.localPosition = newPos;
        }
    }
    #endregion

    #region Public Methods
    public bool AddMinimapElement(MinimapElement mmElement)
    {
        if (!minimapElements.Contains(mmElement))
        {
            minimapElements.Add(mmElement);
            minimapImages.Add(CreateElementImage(mmElement));
            return true;
        }
        return false;
    }

    public bool RemoveMinimapElement(MinimapElement mmElement)
    {
        return minimapElements.Remove(mmElement);
    }
    #endregion

    #region Private Methods
    private Image CreateElementImage(MinimapElement mmElement)
    {
        Image element = Instantiate(representationPrefab, elementsParent);
        element.sprite = mmElement.sprite;
        element.color = mmElement.color;
        return element;
    }

    private Vector2 WorldToMinimap(Vector3 worldPos)
    {
        // Normalize worldPos between reference points using only the XZ-plane
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.z);
        Vector2 normalizedPos = new Vector2((worldPos2D.x - worldBottomLeft.x) / worldWidth, (worldPos2D.y - worldBottomLeft.y) / worldHeight);

        // Scale the normalizedPos to the minimap size
        Vector2 minimapPos = new Vector2(normalizedPos.x * minimapWidth, normalizedPos.y * minimapHeight);

        return minimapPos;
    }
    #endregion
}
