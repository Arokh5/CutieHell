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
    private Image minimapImagePrefab;
    [SerializeField]
    private RectTransform elementsParent;
    [SerializeField]
    private RectTransform poolParent;

    private List<MinimapElement> minimapElements = new List<MinimapElement>();
    private List<Image> minimapImages = new List<Image>();
    private ObjectPool<Image> minimapImagesPool;

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

        UnityEngine.Assertions.Assert.IsNotNull(worldReference.bottomLeft, "ERROR: World Reference > Bottom Left (Transform) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(worldReference.topRight, "ERROR: World Reference > Top Right (Transform) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(minimapImagePrefab, "ERROR: Minimap Image Prefab (Image Prefab) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(elementsParent, "ERROR: Elements Parent (Transform) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(poolParent, "ERROR: Pool Parent (Transform) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);

        minimapImagesPool = new ObjectPool<Image>(minimapImagePrefab, poolParent.transform);
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
            MinimapElement minimapElement = minimapElements[i];
            Image minimapImage = minimapImages[i];
            if (minimapElement.gameObject.activeInHierarchy)
            {
                minimapImage.gameObject.SetActive(true);

                Vector2 newPos = WorldToMinimap(minimapElements[i].transform.position);
                minimapImage.rectTransform.localPosition = newPos;
                if (IsMinimapImageWithinMinimap(minimapImage))
                {
                    minimapImage.enabled = true;
                }
                else
                {
                    minimapImage.enabled = false;
                }
            }
            else
            {
                minimapImage.gameObject.SetActive(false);
            }
            
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
        int index = minimapElements.IndexOf(mmElement);
        if (index != -1)
        {
            minimapElements.RemoveAt(index);
            Image minimapImage = minimapImages[index];
            if (minimapImage != null)
            {
                // The null case occurs during destruction of the scene
                minimapImagesPool.ReturnToPool(minimapImage);
            }
            minimapImages.RemoveAt(index);
        }
        return index != -1;
    }
    #endregion

    #region Private Methods
    private Image CreateElementImage(MinimapElement mmElement)
    {
        Image element = minimapImagesPool.GetObject(elementsParent, false);
        element.sprite = mmElement.sprite;
        element.color = mmElement.color;
        element.rectTransform.sizeDelta = new Vector2(mmElement.size, mmElement.size);
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

    private bool IsMinimapImageWithinMinimap(Image minimapImage)
    {
        return minimapImage.rectTransform.localPosition.x > 0.5f * minimapImage.rectTransform.sizeDelta.x
            && minimapImage.rectTransform.localPosition.x < minimapWidth - 0.5f * minimapImage.rectTransform.sizeDelta.x
            && minimapImage.rectTransform.localPosition.y > 0.5f * minimapImage.rectTransform.sizeDelta.y
            && minimapImage.rectTransform.localPosition.y < minimapHeight - 0.5f * minimapImage.rectTransform.sizeDelta.y;
    }
    #endregion
}
