using UnityEngine;
using UnityEngine.UI;


public class EvilManaController : MonoBehaviour {


    #region Attributes
    [Header("Fragments UI info")]
    [SerializeField]
    private Image evilFragmentsFiller;
    [SerializeField]
    private Text evilFragmentsNumber;

    [Header("Colors for the evil Circle")]
    [SerializeField]
    Color[] colors;

    private float maxPlayerEvil = 0;
    private float evilUnits = 0;
    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(evilFragmentsNumber, "Error: No Text assigned to: " + gameObject.name);
        
        maxPlayerEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
        UnityEngine.Assertions.Assert.AreEqual(maxPlayerEvil / 10, colors.Length, "Error: Player's max evil level attribute and and numEvilColors must match" + gameObject.name);

        evilUnits = NormalizeEvilNumber(maxPlayerEvil);

        evilFragmentsNumber.text = NormalizeEvilNumber(maxPlayerEvil).ToString("0");
        
    }
    #endregion

    #region Public methods

    public void UpdateCurrentEvil(float currentEvil)
    {
        Debug.Log(currentEvil);
        evilFragmentsFiller.fillAmount = ( (float) currentEvil/100);
        Debug.Log(evilFragmentsFiller.fillAmount);
        
        if (int.Parse(evilFragmentsNumber.text) != NormalizeEvilNumber(currentEvil))
        {
            float normalizedEvil = NormalizeEvilNumber(currentEvil);
            evilFragmentsNumber.text = normalizedEvil.ToString();
        }

    }
    #endregion

    #region Private methods
    private int NormalizeEvilNumber(float evil)
    {
        int normalizedEvilNumber = 0;
        //Divided / 10 because we are working two digits evil values, but the UI shows only one digit value. 
        normalizedEvilNumber = (int)(evil / 10);

        return normalizedEvilNumber;
    }

    private void ColorCircleFiller(int currentEvil)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if(i == currentEvil)
            {
                evilFragmentsFiller.color = colors[i];
                break;
            }
        }
    }
    #endregion
}
