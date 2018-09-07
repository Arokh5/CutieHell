using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public enum MinimapBorder
    {
        NONE,
        TOP,
        RIGHT,
        BOTTOM,
        LEFT
    }

    [System.Serializable]
    private class WorldReference
    {
        public Transform bottomLeft;
        public Transform topRight;
    }

    [System.Serializable]
    private class AlertImageInfo
    {
        public MinimapBorder border;
        public Image image;
        [HideInInspector]
        public bool requiredState = false;
    }

    #region Fields
    public static MinimapController instance;

    [SerializeField]
    private WorldReference worldReference;
    [SerializeField]
    private MinimapImage minimapImagePrefab;
    [SerializeField]
    private RectTransform elementsParent;
    [SerializeField]
    private RectTransform poolParent;
    [SerializeField]
    private AlertImageInfo[] alertImageInfos;

    private List<MinimapElement> minimapElements = new List<MinimapElement>();
    private List<MinimapImage> minimapImages = new List<MinimapImage>();
    private ObjectPool<MinimapImage> minimapImagesPool;
    private Dictionary<MinimapBorder, AlertImageInfo> alertImages = new Dictionary<MinimapBorder, AlertImageInfo>();

    private Vector2 worldBottomLeft;
    private Vector2 worldTopRight;
    private float worldWidth;
    private float worldHeight;
    private float minimapWidth;
    private float minimapHeight;
    #endregion

    #region MonoBehaviour Methods
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(worldReference.bottomLeft.position, 1.0f);
        Gizmos.DrawSphere(worldReference.topRight.position, 1.0f);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(worldReference.bottomLeft.position, 0.2f);
        Gizmos.DrawSphere(worldReference.topRight.position, 0.2f);
    }

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

        minimapImagesPool = new ObjectPool<MinimapImage>(minimapImagePrefab, poolParent.transform);

        for (int i = 0; i < alertImageInfos.Length - 1; ++i)
        {
            if (alertImageInfos[i].border == MinimapBorder.NONE)
            {
                Debug.LogError("ERROR (MinimapController): Alert Image Info at index " + i + " has the default value of NONE. Assign a valid value!");
            }
            for (int j = i + 1; j < alertImageInfos.Length; ++j)
            {
                if (alertImageInfos[i].border == alertImageInfos[j].border)
                {
                    Debug.LogError("ERROR (MinimapController): Alert Image Infos at indexes " + i + " and " + j + " have the same border value. There can only be one Alert Image Info for each border value!");
                }
            }
        }
    }

    private void Start()
    {
        worldBottomLeft = new Vector2(worldReference.bottomLeft.position.x, worldReference.bottomLeft.position.z);
        worldTopRight = new Vector2(worldReference.topRight.position.x, worldReference.topRight.position.z);
        worldWidth = worldTopRight.x - worldBottomLeft.x;
        worldHeight = worldTopRight.y - worldBottomLeft.y;
        minimapWidth = elementsParent.sizeDelta.x;
        minimapHeight = elementsParent.sizeDelta.y;

        foreach (AlertImageInfo info in alertImageInfos)
        {
            info.image.gameObject.SetActive(false);
            alertImages.Add(info.border, info);
        }
    }

    private void Update()
    {
        for (int i = 0; i < minimapElements.Count; ++i)
        {
            MinimapElement minimapElement = minimapElements[i];
            MinimapImage minimapImage = minimapImages[i];
            if (minimapElement.gameObject.activeInHierarchy)
            {
                minimapImage.gameObject.SetActive(true);

                Vector2 newPos = WorldToMinimap(minimapElements[i].transform.position);
                minimapImage.localPosition = newPos;
                MinimapBorder exitBorder = GetMinimapImageExitBorder(minimapImage);
                if (exitBorder == MinimapBorder.NONE)
                {
                    minimapImage.Show();
                }
                else
                {
                    minimapImage.Hide();
                    if (minimapElement.triggersAlert)
                    {
                        if (alertImages.ContainsKey(exitBorder))
                        {
                            alertImages[exitBorder].requiredState = true;
                        }
                    }
                }
            }
            else
            {
                minimapImage.gameObject.SetActive(false);
            }
        }

        
        foreach (MinimapBorder border in alertImages.Keys)
        {
            alertImages[border].image.gameObject.SetActive(alertImages[border].requiredState);
            alertImages[border].requiredState = false;
        }
    }
    #endregion

    #region Public Methods
    public bool AddMinimapElement(MinimapElement mmElement)
    {
        if (!minimapElements.Contains(mmElement))
        {
            int hierarchyIndex = GetMinimapElementInsertionIndex(mmElement);
            minimapElements.Insert(hierarchyIndex, mmElement);
            minimapImages.Insert(hierarchyIndex, CreateElementImage(mmElement, hierarchyIndex));
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
            MinimapImage minimapImage = minimapImages[index];
            if (minimapImage != null)
            {
                // The null case occurs during destruction of the scene
                minimapImage.CleanUp();
                minimapImagesPool.ReturnToPool(minimapImage);
            }
            minimapImages.RemoveAt(index);
        }
        return index != -1;
    }
    #endregion

    #region Private Methods
    private int GetMinimapElementInsertionIndex(MinimapElement mmElement)
    {
        int count = minimapElements.Count;
        int index = count;

        for (int i = 0; i < count; ++i)
        {
            if (mmElement.priority < minimapElements[i].priority)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    private MinimapImage CreateElementImage(MinimapElement mmElement, int hierarchyIndex)
    {
        MinimapImage mmImage = minimapImagesPool.GetObject(elementsParent, false);
        mmImage.SetupMinimapImage(mmElement);
        mmImage.transform.SetSiblingIndex(hierarchyIndex);
        return mmImage;
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

    private bool IsMinimapImageWithinMinimap(MinimapImage minimapImage)
    {
        return minimapImage.localPosition.x > 0.5f * minimapImage.width
            && minimapImage.localPosition.x < minimapWidth - 0.5f * minimapImage.width
            && minimapImage.localPosition.y > 0.5f * minimapImage.height
            && minimapImage.localPosition.y < minimapHeight - 0.5f * minimapImage.height;
    }

    private MinimapBorder GetMinimapImageExitBorder(MinimapImage minimapImage)
    {
        // Do note that exiting through the side takes precedence over top and bottom
        MinimapBorder exitBorder = MinimapBorder.NONE;

        if (minimapImage.localPosition.x < 0.5f * minimapImage.width)
            exitBorder = MinimapBorder.LEFT;
        else if (minimapImage.localPosition.x > minimapWidth - 0.5f * minimapImage.width)
            exitBorder = MinimapBorder.RIGHT;
        else if (minimapImage.localPosition.y < 0.5f * minimapImage.height)
            exitBorder = MinimapBorder.BOTTOM;
        else if (minimapImage.localPosition.y > minimapHeight - 0.5f * minimapImage.height)
            exitBorder = MinimapBorder.TOP;

        return exitBorder;
    }
    #endregion
}
