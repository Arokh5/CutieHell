using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy_temp : MonoBehaviour
{
	#region Fields
	
    public enum MoveStates { Right, Left }
    public MoveStates moveState;

	#endregion
	
	#region Properties
	
    #endregion
	
	#region MonoBehaviour Methods
	
    private void Update()
    {
        switch (moveState)
        {
            case MoveStates.Right:
                transform.Translate(Vector3.right * 2f * Time.deltaTime);

                if (transform.position.x >= 20)
                    moveState = MoveStates.Left;

                break;

            case MoveStates.Left:
                transform.Translate(Vector3.left * 3f * Time.deltaTime);

                if (transform.position.x <= -20)
                    moveState = MoveStates.Right;

                break;
        }
    }

	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods
	
	#endregion
}