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

    public enum MinimapImageType
    {
        DEFAULT,
        FLASHER
    }

    [System.Serializable]
    private class WorldReference
    {
        public Transform bottomLeft;
        public Transform topRight;
        public Sprite backgroundSprite;
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

    [Header("Elements setup")]
    [SerializeField]
    private MinimapImage minimapImagePrefab;
    [SerializeField]
    private MinimapFlashImage minimapFlashImagePrefab;
    [SerializeField]
    private Image elementsBackgroundImage;
    [SerializeField]
    private RectTransform elementsParent;
    [SerializeField]
    private RectTransform poolParent;

    [Header("World References")]
    [SerializeField]
    private WorldReference[] worldReferences;

    [Header("Alerts setup")]
    [SerializeField]
    private int initialWorldReference;
    [SerializeField]
    private AlertImageInfo[] alertImageInfos;

    private int currentWorldReferenceIndex = 0;

    private List<MinimapElement> minimapElements = new List<MinimapElement>();
    private List<MinimapImage> minimapImages = new List<MinimapImage>();
    private ObjectPool<MinimapImage> minimapImagesPool;
    private ObjectPool<MinimapFlashImage> minimapFlashImagesPool;
    private Dictionary<MinimapBorder, AlertImageInfo> alertImages = new Dictionary<MinimapBorder, AlertImageInfo>();

    private Vector2 worldBottomLeft;
    private Vector2 worldTopRight;
    private float worldWidth;
    private float worldHeight;
    private float minimapWidth;
    private float minimapHeight;
    #endregion

    #region MonoBehaviour Methods
    public int testIndex = 0;
    private void OnValidate()
    {
        ChangeMinimapReference(testIndex);
    }

    private void OnDrawGizmos()
    {
        if (worldReferences != null)
        {
            foreach (WorldReference wr in worldReferences)
            {
                // Draw square
                if (wr.bottomLeft && wr.topRight)
                {
                    // Calculate corners
                    float y = 0.0f;
                    Vector3 bl = new Vector3(wr.bottomLeft.position.x, y, wr.bottomLeft.position.z);
                    Vector3 br = new Vector3(wr.topRight.position.x, y, wr.bottomLeft.position.z);
                    Vector3 tr = new Vector3(wr.topRight.position.x, y, wr.topRight.position.z);
                    Vector3 tl = new Vector3(wr.bottomLeft.position.x, y, wr.topRight.position.z);

                    // Draw corners
                    Gizmos.color = new Color(0.25f, 0.25f, 0.375f);
                    Gizmos.DrawWireSphere(bl, 1.0f);
                    Gizmos.DrawWireSphere(br, 1.0f);
                    Gizmos.DrawWireSphere(tr, 1.0f);
                    Gizmos.DrawWireSphere(tl, 1.0f);

                    // Draw lines
                    Gizmos.color = new Color(0.75f, 0.75f, 0.75f);
                    Gizmos.DrawLine(bl, br);
                    Gizmos.DrawLine(br, tr);
                    Gizmos.DrawLine(tr, tl);
                    Gizmos.DrawLine(tl, bl);
                }
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        
        UnityEngine.Assertions.Assert.IsNotNull(minimapImagePrefab, "ERROR: Minimap Image Prefab (Image Prefab) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(minimapFlashImagePrefab, "ERROR: Minimap Flash Image Prefab (Image Prefab) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(elementsBackgroundImage, "ERROR: Background Image (Image) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(elementsParent, "ERROR: Elements Parent (Transform) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(poolParent, "ERROR: Pool Parent (Transform) has NOT been assigned in MinimapController in GameObject called " + gameObject.name);

        for (int i = 0; i < worldReferences.Length; ++i)
        {
            WorldReference wr = worldReferences[i];
            UnityEngine.Assertions.Assert.IsNotNull(wr.bottomLeft, "ERROR: World Reference at index " + i + " does NOT have a Bottom Left (Transform) assigned in MinimapController in GameObject called " + gameObject.name);
            UnityEngine.Assertions.Assert.IsNotNull(wr.topRight, "ERROR: World Reference at index " + i + " does NOT have a Top Right (Transform) assigned in MinimapController in GameObject called " + gameObject.name);
            UnityEngine.Assertions.Assert.IsNotNull(wr.backgroundSprite, "ERROR: World Reference at index " + i + " does NOT have a Background Sprite (Image) assigned in MinimapController in GameObject called " + gameObject.name);
        }
        

        minimapImagesPool = new ObjectPool<MinimapImage>(minimapImagePrefab, poolParent.transform);
        minimapFlashImagesPool = new ObjectPool<MinimapFlashImage>(minimapFlashImagePrefab, poolParent.transform);

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
        SetupWorldReference(worldReferences[0]);
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
        UpdateElements();
        UpdateAlerts();
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

    public bool ChangeMinimapReference(int index)
    {
        bool success;
        if (index >= 0 && index < worldReferences.Length)
        {
            currentWorldReferenceIndex = index;
            SetupWorldReference(worldReferences[currentWorldReferenceIndex]);
            success = true;
        }
        else
        {
            Debug.LogWarning("WARNING: MinimapController::ChangeMinimapReference called with a parameter (" + index + ") out of the range [0, " + worldReferences.Length + "]. The call will be ignored!");
            success = false;
        }
        return success;
    }
    #endregion

    #region Private Methods
    private void UpdateElements()
    {
        for (int i = 0; i < minimapElements.Count; ++i)
        {
            MinimapElement minimapElement = minimapElements[i];
            MinimapImage minimapImage = minimapImages[i];
            if (minimapElement.gameObject.activeInHierarchy)
            {
                minimapImage.gameObject.SetActive(true);

                if (minimapElement.ExtractEffectRequestState())
                {
                    minimapImage.RequestEffect();
                }

                Vector2 newPos = WorldToMinimap(minimapElement.transform.position);
                minimapImage.localPosition = newPos;
                if (minimapElement.updatesRotation)
                {
                    minimapImage.rotationCW = minimapElement.transform.rotation.eulerAngles.y;
                }

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
    }

    private void UpdateAlerts()
    {
        foreach (MinimapBorder border in alertImages.Keys)
        {
            alertImages[border].image.gameObject.SetActive(alertImages[border].requiredState);
            alertImages[border].requiredState = false;
        }
    }

    private void SetupWorldReference(WorldReference reference)
    {
        worldBottomLeft = new Vector2(reference.bottomLeft.position.x, reference.bottomLeft.position.z);
        worldTopRight = new Vector2(reference.topRight.position.x, reference.topRight.position.z);
        worldWidth = worldTopRight.x - worldBottomLeft.x;
        worldHeight = worldTopRight.y - worldBottomLeft.y;
        elementsBackgroundImage.sprite = reference.backgroundSprite;
    }

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
        MinimapImage mmImage;
        switch (mmElement.minimapImageType)
        {
            case MinimapImageType.DEFAULT:
                mmImage = minimapImagesPool.GetObject(elementsParent, false);
                break;
            case MinimapImageType.FLASHER:
                mmImage = minimapFlashImagesPool.GetObject(elementsParent, false);
                break;
            default:
                Debug.LogError("ERROR (MinimapController): The CreateElementImage has not been updated to handle a newly introduced MinimapImageType!");
                mmImage = minimapImagesPool.GetObject(elementsParent, false);
                break;
        }
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
