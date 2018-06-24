using UnityEngine;

public class EvilManaController : MonoBehaviour {

    #region Attributes

    [Header("Evil UI")]
    [SerializeField]
    private EvilPointController[] evilPoints;
    private float maxEvil;
    private float currentEvil;
	#endregion
	
	#region MonoBehaviour methods
	// Use this for initialization
	void Start () 
	{
        maxEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
        currentEvil = maxEvil;
        UnityEngine.Assertions.Assert.AreEqual(maxEvil, evilPoints.Length, "ERROR: There has to be as many UI EvilPoints as Player maxEvil value, check GameObject called: " + gameObject.name);
        InitializeEvilPoints();
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	#endregion
			
	#region Public methods
	public void ModifyEvil(float _currentEvil)
    {
        //Check if evilPoints have not either increased or decreased in comparison with last modification
        if ((int) currentEvil == (int)_currentEvil) 
        {
            evilPoints[(int)currentEvil].ModifyEvilPoint(_currentEvil);
        }
        else
        {
            int evilPointsAvailable = DetermineCurrentEvilPointsAvailable(_currentEvil);
            int evilPointsUnavailable = (int) maxEvil - evilPointsAvailable;

            //Check on the available evil points
            for(int i = 0; i < evilPointsAvailable; i++)
            {
                evilPoints[i].CompleteFill();
            }

            //Check on the consumed evil points
            for(int i = evilPointsAvailable; i < maxEvil; i++)
            {
                evilPoints[i].CompleteVoid();
                //Singularity on the smallest evil point available because it will be "on progress"
                if (i == evilPointsAvailable)
                    evilPoints[i].ModifyEvilPoint(_currentEvil);               
            }
        }
        currentEvil = _currentEvil;
    }
	#endregion
	
	#region Private methods
	private void InitializeEvilPoints()
    {
        for(int i = 0; i < evilPoints.Length; i++)
        {
            evilPoints[i].CompleteFill();
        }
    } 

    private int DetermineCurrentEvilPointsAvailable(float _currentEvilPoints)
    {
        int currentEvilPoints = (int)_currentEvilPoints;

        return currentEvilPoints;
    }
    
    
	#endregion
}
