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
    void Awaken () 
	{
        UnityEngine.Assertions.Assert.IsNotNull(evilFragmentsNumber, "Error: No Text assigned to: " + gameObject.name);
        maxPlayerEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
        evilFragmentsNumber.text = NormalizeEvilNumber(maxPlayerEvil).ToString("0");
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
        evilFragmentsFiller.fillAmount = currentEvil / maxPlayerEvil;

        if(int.Parse(evilFragmentsNumber.text) != NormalizeEvilNumber(currentEvil))
        {
            float normalizedEvil = NormalizeEvilNumber(currentEvil);
            evilFragmentsNumber.text = normalizedEvil.ToString("0");

        }
    }
	#endregion
	
	#region Private methods
	private float NormalizeEvilNumber(float evil)
    {
        float normalizedEvilNumber = 0f;
        normalizedEvilNumber = evil / 10;

        return normalizedEvilNumber;
    }
	#endregion
}
