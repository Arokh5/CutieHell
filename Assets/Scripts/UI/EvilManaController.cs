using UnityEngine;

public class EvilManaController : MonoBehaviour {

    #region Attributes

    [Header("Evil UI")]
    [SerializeField]
    private EvilPointController[] evilPoints;
    private float maxEvil;
	#endregion
	
	#region MonoBehaviour methods
	// Use this for initialization
	void Start () 
	{
        maxEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
        UnityEngine.Assertions.Assert.AreEqual(maxEvil, evilPoints.Length, "ERROR: There has to be as many UI EvilPoints as Player maxEvil value, check GameObject called: " + gameObject.name);
        InitializeEvilPoints();
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	#endregion
	
	
	
	#region Public methods
	
	#endregion
	
	#region Private methods
	void InitializeEvilPoints()
    {
        for(int i = 0; i < evilPoints.Length; i++)
        {
            evilPoints[i].completeFill();
        }
    } 
	#endregion
}
