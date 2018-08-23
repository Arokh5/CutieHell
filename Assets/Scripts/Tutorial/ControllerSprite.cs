using UnityEngine;
using UnityEngine.UI;

public class ControllerSprite : Observer
{
    #region Fields
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite spritePS4;
    [SerializeField]
    private Sprite spriteXbox;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        InputManager.instance.AddObserver(this);
        UnityEngine.Assertions.Assert.IsNotNull(image, "ERROR: Image (Image) not assigned for ControllerSprite script in GameObject " + gameObject.name);
    }

    private void OnValidate()
    {
        if (image)
        {
            if (spritePS4)
                image.sprite = spritePS4;
            else if (spriteXbox)
                image.sprite = spriteXbox;
        }
    }
    #endregion

    #region Public Methods
    // Observer
    public override void OnNotify()
    {
        if (InputManager.instance.isPS4)
            image.sprite = spritePS4;
        else if (InputManager.instance.isXbox)
            image.sprite = spriteXbox;
    }
    #endregion
}
