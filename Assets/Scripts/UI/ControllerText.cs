using UnityEngine;
using UnityEngine.UI;

public class ControllerText : Observer
{
    #region Fields
    [SerializeField]
    private Text textElement;
    [SerializeField]
    private string textPS4;
    [SerializeField]
    private string textXbox;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(textElement, "ERROR: Text Element (Text) not assigned for ControllerText script in GameObject " + gameObject.name);
        InputManager.instance.AddObserver(this);
        OnNotify();
    }

    private void OnValidate()
    {
        if (textElement)
        {
            textElement.text = textPS4;
        }
    }
    #endregion

    #region Public Methods
    // Observer
    public override void OnNotify()
    {
        if (InputManager.instance.isPS4)
            textElement.text = textPS4;
        else if (InputManager.instance.isXbox)
            textElement.text = textXbox;
    }
    #endregion
}
