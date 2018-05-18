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
    public bool startClosed;
    private bool shouldClose;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(bannerImage, "ERROR: MonumentIndicator in gameObject '" + gameObject.name + "' doesn't have bannerImage assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(fillImage, "ERROR: MonumentIndicator in gameObject '" + gameObject.name + "' doesn't have fillImage assigned!");
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

    public void OnNewWaveStarted()
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
    #endregion

    #region Private Methods
    private void Open()
    {
        bannerImage.sprite = openedSprite;
    }

    private void Close()
    {
        bannerImage.sprite = closedSprite;
    }
    #endregion
}
