using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    #region Public Data

    public static InputManager instance;

    #endregion

    #region Private Serialized Fields

    #endregion

    #region Private Non-Serialized Fields

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Public Methods

    /* Buttons */

    public bool GetXButton()
    {
        return Input.GetButtonDown("PS4_X");
    }

    public bool GetOButton()
    {
        return Input.GetButtonDown("PS4_O");
    }

    public bool GetTriangleButton()
    {
        return Input.GetButtonDown("PS4_Triangle");
    }

    public bool GetSquareButton()
    {
        return Input.GetButtonDown("PS4_Square");
    }

    /* Left Stick */

    public bool GetLeftStickRight()
    {
        return Input.GetAxis("PS4_L_Horizontal") > 0.1;
    }

    public bool GetLeftStickLeft()
    {
        return Input.GetAxis("PS4_L_Horizontal") < -0.1;
    }

    public bool GetLeftStickUp()
    {
        return Input.GetAxis("PS4_L_Vertical") < -0.1;
    }

    public bool GetLeftStickDown()
    {
        return Input.GetAxis("PS4_L_Vertical") > 0.1;
    }

    public bool GetLeftStickButton()
    {
        return Input.GetButtonDown("PS4_L_Bnt");
    }

    /* Right Stick */

    public bool GetRightStickRight()
    {
        return Input.GetAxis("PS4_R_Horizontal") > 0.1;
    }

    public bool GetRightStickLeft()
    {
        return Input.GetAxis("PS4_R_Horizontal") < -0.1;
    }

    public bool GetRightStickUp()
    {
        return Input.GetAxis("PS4_R_Vertical") < -0.1;
    }

    public bool GetRightStickDown()
    {
        return Input.GetAxis("PS4_R_Vertical") > 0.1;
    }

    public bool GetRightStickButton()
    {
        return Input.GetButtonDown("PS4_R_Bnt");
    }

    /* Top side */

    public bool GetR2Button()
    {
        return Input.GetAxis("PS4_R2") > 0.1;
    }

    public bool GetL2Button()
    {
        return Input.GetAxis("PS4_L2") > 0.1;
    }

    public bool GetR1Button()
    {
        return Input.GetButtonDown("PS4_R1");
    }

    public bool GetL1Button()
    {
        return Input.GetButtonDown("PS4_L1");
    }

    /* D-Pad */

    public bool GetPadUp()
    {
        return Input.GetAxis("PS4_D_Y") == 1;
    }

    public bool GetPadDown()
    {
        return Input.GetAxis("PS4_D_Y") == -1;
    }

    public bool GetPadRight()
    {
        return Input.GetAxis("PS4_D_X") == 1;
    }

    public bool GetPadLeft()
    {
        return Input.GetAxis("PS4_D_X") == -1;
    }

    /* PS4 buttons */

    public bool GetPS4Options()
    {
        return Input.GetButtonDown("PS4_Options");
    }

    public bool GetPS4Share()
    {
        return Input.GetButtonDown("PS4_Share");
    }

    public bool GetPS4Pad()
    {
        return Input.GetButtonDown("PS4_Pad");
    }

    public bool GetPS4PS()
    {
        return Input.GetButtonDown("PS4_PS");
    }

    #endregion

    #region Private Methods

    #endregion
}