using UnityEngine;
using UnityEngine.UI;

public class MonumentIndicator : MonoBehaviour {

    #region Fields
    [SerializeField]
    private Image bannerImage;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Sprite openedSprite;
    [SerializeField]
    private Sprite closedSprite;
    [SerializeField]
    private ObjectiveMarker marker;
    [SerializeField]
    private GameObject iconConquered;

    public bool startClosed;
    private bool shouldClose;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(bannerImage, "ERROR: MonumentIndicator in gameObject '" + gameObject.name + "' doesn't have bannerImage assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(fillImage, "ERROR: MonumentIndicator in gameObject '" + gameObject.name + "' doesn't have fillImage assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(iconConquered, "ERROR: iconConquered could not be unassigned on the GameObject " + gameObject.name);
    }

    private void Start()
    {
        if (startClosed)
            Close();
    }
    #endregion

    #region Public Methods
    public void SetFill(float normalizedFill)
    {
        fillImage.fillAmount = normalizedFill;
    }

    public void OnNewRoundStarted()
    {
        if (shouldClose)
        {
            shouldClose = false;
            Close();
        }
    }

    public void RequestOpen()
    {
        Open();
    }

    public void RequestClose()
    {
        shouldClose = true;
    }

    public void ActivateIconConquered()
    {
        iconConquered.SetActive(true);
    }

    public void DeactivateIconConquered()
    {
        iconConquered.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void Open()
    {
        bannerImage.sprite = openedSprite;
        fillImage.enabled = true;
        marker.gameObject.SetActive(true);
    }

    private void Close()
    {
        bannerImage.sprite = closedSprite;
        fillImage.enabled = false;
        marker.gameObject.SetActive(false);
    }
    #endregion
}
