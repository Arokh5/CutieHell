using UnityEngine;
using UnityEngine.UI;

public class ButtonPadController : Observer {

    #region Attributes
    [SerializeField]
    private Sprite xboxButtonSprite;
    [SerializeField]
    private Sprite ps4ButtonSprite;
    [SerializeField]
    private Image buttonPadImage;
	#endregion
	
	#region MonoBehaviour methods
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
        InputManager.instance.AddObserver(this);

        if (InputManager.instance.isPS4)
        {
            ChangeButtonSprite(ps4ButtonSprite);
        }
        else
        {
            ChangeButtonSprite(xboxButtonSprite);
        }

    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    #region Public methods
    public override void OnNotify()
    {
        if(InputManager.instance.isPS4)
        {
            ChangeButtonSprite(ps4ButtonSprite);
        }
        else
        {
            ChangeButtonSprite(xboxButtonSprite);
        }
        
    }
    #endregion

    #region Private methods
    private void ChangeButtonSprite(Sprite sprite)
    {
        buttonPadImage.sprite = sprite;
    }
    #endregion
}
