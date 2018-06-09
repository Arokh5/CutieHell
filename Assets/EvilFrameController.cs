using UnityEngine;
using UnityEngine.UI;

public class EvilFrameController : MonoBehaviour {

    #region Attributes
    [Header("Fragments UI info")]
    [SerializeField]
    private Image evilFragmentsFiller;
    [SerializeField]
    private Text evilFragmentsNumber;
    private float maxPlayerEvil = 0;

    public bool isMainFrame;
    #endregion

    #region MonoBehaviour methods

    #endregion

    // Use this for initialization
    void Start () 
	{
        UnityEngine.Assertions.Assert.IsNotNull(evilFragmentsNumber, "Error: No Text assigned to: " + gameObject.name);
        maxPlayerEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();

        if(isMainFrame)
        {
            evilFragmentsNumber.text = NormalizeEvilNumber(maxPlayerEvil).ToString("0");
        }else
        {
            evilFragmentsNumber.text = NormalizeEvilNumber(maxPlayerEvil * 0.5f).ToString("0");
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	#region Public methods
	public Image GetEvilFragmentsFiller()
    {
        return evilFragmentsFiller;
    }

    public Text GetEvilFragmentsNumber()
    {
        return evilFragmentsNumber;
    }

    public void UpdateFragmentsFiller(float currentEvil)
    {
        if (isMainFrame)
        {
            evilFragmentsFiller.fillAmount = (currentEvil / maxPlayerEvil) - (1 - currentEvil / maxPlayerEvil);
        }
        else
        {
            evilFragmentsFiller.fillAmount = (currentEvil * 2/ maxPlayerEvil);
        }
        Debug.Log("CurrentEvil : " + currentEvil);
        Debug.Log("Current fill amount : " + evilFragmentsFiller.fillAmount);

        if(int.Parse(evilFragmentsNumber.text) != NormalizeEvilNumber(currentEvil))
        {
            float normalizedEvil = NormalizeEvilNumber(currentEvil);
            evilFragmentsNumber.text = normalizedEvil.ToString("0");
            Debug.Log("Normalized number : " + normalizedEvil);
        }
        
    }
	#endregion
	
	#region Private methods
	private float NormalizeEvilNumber(float evil)
    {
        float normalizedEvilNumber = 0f;
        //Divided / 10 because we are working two digits evil values, but the UI shows only one digit value. 
        normalizedEvilNumber = (evil / 10); 

        return normalizedEvilNumber;
    }
	#endregion
}
