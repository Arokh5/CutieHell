using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    #region Fields

    public static InputManager instance;
    private bool L2buttonPrevState = false;
    private bool R2buttonPrevState = false;
    private bool leftStickDownPrevState = false;
    private bool leftStickUpPrevState = false;
    private bool padDownPrevState = false;
    private bool padUpPrevState = false;

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

    public bool GetXButtonDown()
    {
        return Input.GetButtonDown("PS4_X");
    }

    public bool GetOButtonDown()
    {
        return Input.GetButtonDown("PS4_O");
    }

    public bool GetTriangleButtonDown()
    {
        return Input.GetButtonDown("PS4_Triangle");
    }

    public bool GetSquareButtonDown()
    {
        return Input.GetButtonDown("PS4_Square");
    }

    public bool GetXButton()
    {
        return Input.GetButton("PS4_X");
    }

    public bool GetOButton()
    {
        return Input.GetButton("PS4_O");
    }

    public bool GetTriangleButton()
    {
        return Input.GetButton("PS4_Triangle");
    }

    public bool GetSquareButton()
    {
        return Input.GetButton("PS4_Square");
    }

    public bool GetXButtonUp()
    {
        return Input.GetButtonUp("PS4_X");
    }

    public bool GetOButtonUp()
    {
        return Input.GetButtonUp("PS4_O");
    }

    public bool GetTriangleButtonUp()
    {
        return Input.GetButtonUp("PS4_Triangle");
    }

    public bool GetSquareButtonUp()
    {
        return Input.GetButtonUp("PS4_Square");
    }

    /* Left Stick */

    public bool GetLeftStickRight()
    {
        return Input.GetAxis("PS4_L_Horizontal") > 0.1;
    }

    public float GetLeftStickRightValue()
    {
        return Input.GetAxis("PS4_L_Horizontal");
    }

    public bool GetLeftStickLeft()
    {
        return Input.GetAxis("PS4_L_Horizontal") < -0.1;
    }

    public float GetLeftStickLeftValue()
    {
        return Input.GetAxis("PS4_L_Horizontal");
    }

    public bool GetLeftStickUpDown()
    {
        bool buttonDown = false;

        if (GetLeftStickUp())
        {
            if (!leftStickUpPrevState)
            {
                buttonDown = true;
                leftStickUpPrevState = true;
            }
        }
        else
        {
            leftStickUpPrevState = false;
        }

        return buttonDown;
    }

    public bool GetLeftStickUp()
    {
        return Input.GetAxis("PS4_L_Vertical") < -0.1;
    }

    public float GetLeftStickUpValue()
    {
        return Input.GetAxis("PS4_L_Vertical");
    }

    public bool GetLeftStickDownDown()
    {
        bool buttonDown = false;

        if (GetLeftStickDown())
        {
            if (!leftStickDownPrevState)
            {
                buttonDown = true;
                leftStickDownPrevState = true;
            }
        }
        else
        {
            leftStickDownPrevState = false;
        }

        return buttonDown;
    }

    public bool GetLeftStickDown()
    {
        return Input.GetAxis("PS4_L_Vertical") > 0.1;
    }

    public float GetLeftStickDownValue()
    {
        return Input.GetAxis("PS4_L_Vertical");
    }

    public bool GetLeftStickButtonDown()
    {
        return Input.GetButtonDown("PS4_L_Bnt");
    }

    public bool GetLeftStickButton()
    {
        return Input.GetButton("PS4_L_Bnt");
    }

    public bool GetLeftStickButtonUp()
    {
        return Input.GetButtonUp("PS4_L_Bnt");
    }

    /* Right Stick */

    public bool GetRightStickRight()
    {
        return Input.GetAxis("PS4_R_Horizontal") > 0.1;
    }

    public float GetRightStickRightValue()
    {
        return Input.GetAxis("PS4_R_Horizontal");
    }

    public bool GetRightStickLeft()
    {
        return Input.GetAxis("PS4_R_Horizontal") < -0.1;
    }

    public float GetRightStickLeftValue()
    {
        return Input.GetAxis("PS4_R_Horizontal");
    }

    public bool GetRightStickUp()
    {
        return Input.GetAxis("PS4_R_Vertical") < -0.1;
    }

    public float GetRightStickUpValue()
    {
        return Input.GetAxis("PS4_R_Vertical");
    }

    public bool GetRightStickDown()
    {
        return Input.GetAxis("PS4_R_Vertical") > 0.1;
    }

    public float GetRightStickDownValue()
    {
        return Input.GetAxis("PS4_R_Vertical");
    }

    public bool GetRightStickButtonDown()
    {
        return Input.GetButtonDown("PS4_R_Bnt");
    }

    public bool GetRightStickButton()
    {
        return Input.GetButton("PS4_R_Bnt");
    }

    public bool GetRightStickButtonUp()
    {
        return Input.GetButtonUp("PS4_R_Bnt");
    }

    /* Top side */

    public bool GetR2ButtonDown()
    {
        bool buttonDown = false;

        if (GetR2Button())
        {
            if (!R2buttonPrevState)
            {
                buttonDown = true;
                R2buttonPrevState = true;
            }
        }
        else
        {
            R2buttonPrevState = false;
        }

        return buttonDown;
    }

    public bool GetR2Button()
    {
        return Input.GetAxis("PS4_R2") > 0.1;
    }

    public bool GetR2ButtonUp()
    {
        bool buttonUp = false;

        if (!GetR2Button())
        {
            if (R2buttonPrevState)
            {
                buttonUp = true;
                R2buttonPrevState = false;
            }
        }
        else
        {
            R2buttonPrevState = true;
        }

        return buttonUp;
    }

    public float GetR2ButtonValue()
    {
        return Input.GetAxis("PS4_R2");
    }

    public bool GetL2ButtonDown()
    {
        bool buttonDown = false;

        if (GetL2Button())
        {
            if (!L2buttonPrevState)
            {
                buttonDown = true;
                L2buttonPrevState = true;
            }
        }
        else
        {
            L2buttonPrevState = false;
        }

        return buttonDown;
    }

    public bool GetL2Button()
    {
        return Input.GetAxis("PS4_L2") > 0.1;
    }

    public bool GetL2ButtonUp()
    {
        bool buttonUp = false;

        if (!GetL2Button())
        {
            if (L2buttonPrevState)
            {
                buttonUp = true;
                L2buttonPrevState = false;
            }
        }
        else
        {
            L2buttonPrevState = true;
        }

        return buttonUp;
    }

    public float GetL2ButtonValue()
    {
        return Input.GetAxis("PS4_L2");
    }

    public bool GetR1ButtonDown()
    {
        return Input.GetButtonDown("PS4_R1");
    }

    public bool GetL1ButtonDown()
    {
        return Input.GetButtonDown("PS4_L1");
    }

    public bool GetR1Button()
    {
        return Input.GetButton("PS4_R1");
    }

    public bool GetL1Button()
    {
        return Input.GetButton("PS4_L1");
    }

    public bool GetR1ButtonUp()
    {
        return Input.GetButtonUp("PS4_R1");
    }

    public bool GetL1ButtonUp()
    {
        return Input.GetButtonUp("PS4_L1");
    }

    /* D-Pad */

    public bool GetPadUpDown()
    {
        bool buttonDown = false;

        if (GetPadUp())
        {
            if (!padUpPrevState)
            {
                buttonDown = true;
                padUpPrevState = true;
            }
        }
        else
        {
            padUpPrevState = false;
        }

        return buttonDown;
    }

    public bool GetPadUp()
    {
        return Input.GetAxis("PS4_D_Y") == 1;
    }

    public bool GetPadDownDown()
    {
        bool buttonDown = false;

        if (GetPadDown())
        {
            if (!padDownPrevState)
            {
                buttonDown = true;
                padDownPrevState = true;
            }
        }
        else
        {
            padDownPrevState = false;
        }

        return buttonDown;
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

    public bool GetPS4OptionsDown()
    {
        return Input.GetButtonDown("PS4_Options");
    }

    public bool GetPS4ShareDown()
    {
        return Input.GetButtonDown("PS4_Share");
    }

    public bool GetPS4PadDown()
    {
        return Input.GetButtonDown("PS4_Pad");
    }

    public bool GetPS4PSDown()
    {
        return Input.GetButtonDown("PS4_PS");
    }

    public bool GetPS4Options()
    {
        return Input.GetButton("PS4_Options");
    }

    public bool GetPS4Share()
    {
        return Input.GetButton("PS4_Share");
    }

    public bool GetPS4Pad()
    {
        return Input.GetButton("PS4_Pad");
    }

    public bool GetPS4PS()
    {
        return Input.GetButton("PS4_PS");
    }

    public bool GetPS4OptionsUp()
    {
        return Input.GetButtonUp("PS4_Options");
    }

    public bool GetPS4ShareUp()
    {
        return Input.GetButtonUp("PS4_Share");
    }

    public bool GetPS4PadUp()
    {
        return Input.GetButtonUp("PS4_Pad");
    }

    public bool GetPS4PSUp()
    {
        return Input.GetButtonUp("PS4_PS");
    }

    #endregion
}