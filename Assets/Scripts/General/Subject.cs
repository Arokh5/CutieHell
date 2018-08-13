using UnityEngine;
using System.Collections.Generic;

public class Subject : MonoBehaviour {

    #region Attributes
    private List<Observer> observers = new List<Observer>();
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	#region Public methods
	public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }
	#endregion
	
	#region Private methods
	protected virtual void NotifyAll()
    {
        if(observers!= null)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnNotify();
            }
        }        
    }
	#endregion
}
