using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private enum ButtonState { IDLE, DOWN, PRESSED, UP }
    private enum AxisAsButton { L2, R2, LS_DOWN, LS_UP, PAD_DOWN, PAD_UP }

    #region Fields

    public static InputManager instance;
    private ButtonState[] buttonStates;
    private int axisAsButtonsCount;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            axisAsButtonsCount = System.Enum.GetNames(typeof(AxisAsButton)).Length;
            buttonStates = new ButtonState[axisAsButtonsCount];
        }
        else
            Destroy(this);
    }

    private void Update()
    {
        for (int i = 0; i < axisAsButtonsCount; ++i)
        {
            bool currentlyPressed = false;
            switch ((AxisAsButton)i)
            {
                case AxisAsButton.L2:
                    currentlyPressed = GetL2Button();
                    break;
                case AxisAsButton.R2:
                    currentlyPressed = GetR2Button();
                    break;
                case AxisAsButton.LS_DOWN:
                    currentlyPressed = GetLeftStickDown();
                    break;
                case AxisAsButton.LS_UP:
                    currentlyPressed = GetLeftStickUp();
                    break;
                case AxisAsButton.PAD_DOWN:
                    currentlyPressed = GetPadDown();
                    break;
                case AxisAsButton.PAD_UP:
                    currentlyPressed = GetPadUp();
                    break;
                default:
                    break;
            }

            switch (buttonStates[i])
            {
                case ButtonState.IDLE:
                    if (currentlyPressed)
                        buttonStates[i] = ButtonState.DOWN;
                    break;
                case ButtonState.DOWN:
                    if (currentlyPressed)
                        buttonStates[i] = ButtonState.PRESSED;
                    else
                        buttonStates[i] = ButtonState.UP;
                    break;
                case ButtonState.PRESSED:
                    if (!currentlyPressed)
                        buttonStates[i] = ButtonState.UP;
                    break;
                case ButtonState.UP:
                    if (currentlyPressed)
                        buttonStates[i] = ButtonState.DOWN;
                    else
                        buttonStates[i] = ButtonState.IDLE;
                    break;
                default:
                    break;
            }
        }
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
        return buttonStates[(int)AxisAsButton.LS_UP] == ButtonState.DOWN;
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
        return buttonStates[(int)AxisAsButton.LS_DOWN] == ButtonState.DOWN;
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
        return buttonStates[(int)AxisAsButton.R2] == ButtonState.DOWN;
    }

    public bool GetR2Button()
    {
        return Input.GetAxis("PS4_R2") > 0.1;
    }

    public bool GetR2ButtonUp()
    {
        return buttonStates[(int)AxisAsButton.R2] == ButtonState.UP;
    }

    public float GetR2ButtonValue()
    {
        return Input.GetAxis("PS4_R2");
    }

    public bool GetL2ButtonDown()
    {
        return buttonStates[(int)AxisAsButton.L2] == ButtonState.DOWN;
    }

    public bool GetL2Button()
    {
        return Input.GetAxis("PS4_L2") > 0.1;
    }

    public bool GetL2ButtonUp()
    {
        return buttonStates[(int)AxisAsButton.L2] == ButtonState.UP;
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
        return buttonStates[(int)AxisAsButton.PAD_UP] == ButtonState.DOWN;
    }

    public bool GetPadUp()
    {
        return Input.GetAxis("PS4_D_Y") == 1;
    }

    public bool GetPadDownDown()
    {
        return buttonStates[(int)AxisAsButton.PAD_DOWN] == ButtonState.DOWN;
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