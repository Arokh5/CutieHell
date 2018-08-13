using UnityEngine;

public class InputManager : Subject {

    private enum ButtonState { IDLE, DOWN, PRESSED, UP }
    private enum AxisAsButton { L2, R2, LS_DOWN, LS_UP, PAD_DOWN, PAD_UP }

    #region Fields

    public static InputManager instance;
    private ButtonState[] buttonStates;
    private int axisAsButtonsCount;
    public const float joystickThreshold = 0.1f;
    public bool isXbox;
    public bool isPS4;
    [SerializeField]
    private GameObject errorMessage;
    public bool stopGameIfNoController;

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

        CheckDevice();

    }

    private void Update()
    {
        CheckDevice();
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
    public bool GetButtonDown(ControllerButton button)
    {
        switch (button)
        {
            case ControllerButton.X:
                return GetXButtonDown();
            case ControllerButton.SQUARE:
                return GetSquareButtonDown();
            case ControllerButton.TRIANGLE:
                return GetTriangleButtonDown();
            case ControllerButton.CIRCLE:
                return GetOButtonDown();
            case ControllerButton.L1:
                return GetL1ButtonDown();
            case ControllerButton.L2:
                return GetL2ButtonDown();
            case ControllerButton.R1:
                return GetR1ButtonDown();
            case ControllerButton.R2:
                return GetR2ButtonDown();
            case ControllerButton.LS:
                return GetLeftStickButtonDown();
            case ControllerButton.RS:
                return GetRightStickButtonDown();
            case ControllerButton.SHARE:
                return GetPS4ShareDown();
            case ControllerButton.OPTIONS:
                return GetPS4OptionsDown();
            case ControllerButton.PS:
                return GetPS4PSDown();
        }
        Debug.LogError("ERROR: InputManager::GetButtonDown called with an unknown ControllerButton");
        return false;
    }

    public bool GetButton(ControllerButton button)
    {
        switch (button)
        {
            case ControllerButton.X:
                return GetXButton();
            case ControllerButton.SQUARE:
                return GetSquareButton();
            case ControllerButton.TRIANGLE:
                return GetTriangleButton();
            case ControllerButton.CIRCLE:
                return GetOButton();
            case ControllerButton.L1:
                return GetL1Button();
            case ControllerButton.L2:
                return GetL2Button();
            case ControllerButton.R1:
                return GetR1Button();
            case ControllerButton.R2:
                return GetR2Button();
            case ControllerButton.LS:
                return GetLeftStickButton();
            case ControllerButton.RS:
                return GetRightStickButton();
            case ControllerButton.SHARE:
                return GetPS4Share();
            case ControllerButton.OPTIONS:
                return GetPS4Options();
            case ControllerButton.PS:
                return GetPS4PS();
        }
        Debug.LogError("ERROR: InputManager::GetButton called with an unknown ControllerButton");
        return false;
    }

    public bool GetButtonUp(ControllerButton button)
    {
        switch (button)
        {
            case ControllerButton.X:
                return GetXButtonUp();
            case ControllerButton.SQUARE:
                return GetSquareButtonUp();
            case ControllerButton.TRIANGLE:
                return GetTriangleButtonUp();
            case ControllerButton.CIRCLE:
                return GetOButtonUp();
            case ControllerButton.L1:
                return GetL1ButtonUp();
            case ControllerButton.L2:
                return GetL2ButtonUp();
            case ControllerButton.R1:
                return GetR1ButtonUp();
            case ControllerButton.R2:
                return GetR2ButtonUp();
            case ControllerButton.LS:
                return GetLeftStickButtonUp();
            case ControllerButton.RS:
                return GetRightStickButtonUp();
            case ControllerButton.SHARE:
                return GetPS4ShareUp();
            case ControllerButton.OPTIONS:
                return GetPS4OptionsUp();
            case ControllerButton.PS:
                return GetPS4PSUp();
        }
        Debug.LogError("ERROR: InputManager::GetButtonUp called with an unknown ControllerButton");
        return false;
    }


    /* Buttons */

    public bool GetXButtonDown()
    {
        if (isXbox)
        {
            return Input.GetButtonDown("PS4_Square");
        }
        else
        {
            return Input.GetButtonDown("PS4_X");
        }
    }

    public bool GetOButtonDown()
    {
        if (isXbox)
        {
            return Input.GetButtonDown("PS4_X");
        }
        else
        {
            return Input.GetButtonDown("PS4_O");
        }

    }

    public bool GetTriangleButtonDown()
    {
        if (isXbox)
        {
            return Input.GetButtonDown("PS4_Triangle");
        }
        else
        {
            return Input.GetButtonDown("PS4_Triangle");
        }

    }

    public bool GetSquareButtonDown()
    {
        if (isXbox)
        {
            return Input.GetButtonDown("PS4_O");
        }
        else
        {
            return Input.GetButtonDown("PS4_Square");
        }

    }

    public bool GetXButton()
    {
        if (isXbox)
        {
            return Input.GetButton("PS4_Square");
        }
        else
        {
            return Input.GetButton("PS4_X");
        }

    }

    public bool GetOButton()
    {
        if (isXbox)
        {
            return Input.GetButton("PS4_X");
        }
        else
        {
            return Input.GetButton("PS4_O");
        }

    }

    public bool GetTriangleButton()
    {
        if (isXbox)
        {
            return Input.GetButton("PS4_Triangle");
        }
        else
        {
            return Input.GetButton("PS4_Triangle");
        }

    }

    public bool GetSquareButton()
    {
        if (isXbox)
        {
            return Input.GetButton("PS4_O");
        }
        else
        {
            return Input.GetButton("PS4_Square");
        }

    }

    public bool GetXButtonUp()
    {
        if (isXbox)
        {
            return Input.GetButtonUp("PS4_Square");
        }
        else
        {
            return Input.GetButtonUp("PS4_X");
        }

    }

    public bool GetOButtonUp()
    {
        if (isXbox)
        {
            return Input.GetButtonUp("PS4_X");
        }
        else
        {
            return Input.GetButtonUp("PS4_O");
        }

    }

    public bool GetTriangleButtonUp()
    {
        if (isXbox)
        {
            return Input.GetButtonUp("PS4_Triangle");
        }
        else
        {
            return Input.GetButtonUp("PS4_Triangle");
        }

    }

    public bool GetSquareButtonUp()
    {
        if (isXbox)
        {
            return Input.GetButtonUp("PS4_O");
        }
        else
        {
            return Input.GetButtonUp("PS4_Square");
        }

    }

    /* Left Stick */

    public bool GetLeftStickRight()
    {
        return Input.GetAxis("PS4_L_Horizontal") > joystickThreshold;
    }

    public bool GetLeftStickLeft()
    {
        return Input.GetAxis("PS4_L_Horizontal") < -joystickThreshold;
    }

    public float GetLeftStickHorizontalValue()
    {
        return Input.GetAxis("PS4_L_Horizontal");
    }

    public float GetLeftStickHorizontalSqrValue()
    {
        return Input.GetAxis("PS4_L_Horizontal") * Input.GetAxis("PS4_L_Horizontal") * Mathf.Sign(Input.GetAxis("PS4_L_Horizontal"));
    }

    public bool GetLeftStickUpDown()
    {
        return buttonStates[(int)AxisAsButton.LS_UP] == ButtonState.DOWN;
    }

    public bool GetLeftStickUp()
    {
        return Input.GetAxis("PS4_L_Vertical") < -joystickThreshold;
    }

    public bool GetLeftStickDown()
    {
        return Input.GetAxis("PS4_L_Vertical") > joystickThreshold;
    }

    public float GetLeftStickVerticalValue()
    {
        return Input.GetAxis("PS4_L_Vertical");
    }

    public float GetLeftStickVerticalSqrValue()
    {
        return Input.GetAxis("PS4_L_Vertical") * Input.GetAxis("PS4_L_Vertical") * Mathf.Sign(Input.GetAxis("PS4_L_Vertical"));
    }

    public bool GetLeftStickDownDown()
    {
        return buttonStates[(int)AxisAsButton.LS_DOWN] == ButtonState.DOWN;
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
        if (isXbox)
        {
            return Input.GetAxis("PS4_L2") > joystickThreshold;
        }
        else
        {
            return Input.GetAxis("PS4_R_Horizontal") > joystickThreshold;
        }
    }

    public bool GetRightStickLeft()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_L2") < -joystickThreshold;
        }
        else
        {
            return Input.GetAxis("PS4_R_Horizontal") < -joystickThreshold;
        }
    }

    public float GetRightStickHorizontalValue()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_L2");
        }
        else
        {
            return Input.GetAxis("PS4_R_Horizontal");
        }
    }

    public float GetRightStickHorizontalSqrValue()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_L2") * Input.GetAxis("PS4_L2") * Mathf.Sign(Input.GetAxis("PS4_L2"));
        }
        else
        {
            return Input.GetAxis("PS4_R_Horizontal") * Input.GetAxis("PS4_R_Horizontal") * Mathf.Sign(Input.GetAxis("PS4_R_Horizontal"));
        }
    }

    public bool GetRightStickUp()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_R2") < -joystickThreshold;
        }
        else
        {
            return Input.GetAxis("PS4_R_Vertical") < -joystickThreshold;
        }
    }

    public bool GetRightStickDown()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_R2") > joystickThreshold;
        }
        else
        {
            return Input.GetAxis("PS4_R_Vertical") > joystickThreshold;
        }
    }

    public float GetRightStickVerticalValue()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_R2");
        }
        else
        {
            return Input.GetAxis("PS4_R_Vertical");
        }
    }

    public float GetRightStickVerticalSqrValue()
    {
        if (isXbox)
        {
            return Input.GetAxis("PS4_R2") * Input.GetAxis("PS4_R2") * Mathf.Sign(Input.GetAxis("PS4_R2"));
        }
        else
        {
            return Input.GetAxis("PS4_R_Vertical") * Input.GetAxis("PS4_R_Vertical") * Mathf.Sign(Input.GetAxis("PS4_R_Vertical"));
        }
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
        if (isXbox)
        {
            return Input.GetAxis("XBOX_RT") > joystickThreshold;
        }
        else
        {
            return Input.GetAxis("PS4_R2") > joystickThreshold;
        }
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
        if (isXbox)
        {
            return Input.GetAxis("XBOX_LT") > joystickThreshold;
        }
        else
        {
            return Input.GetAxis("PS4_L2") > joystickThreshold;
        }
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
        if (isXbox)
        {
            return Input.GetAxis("XBOX_Pad_Vertical") == 1;
        }
        else
        {
            return Input.GetAxis("PS4_D_Y") == 1;
        }
    }

    public bool GetPadDownDown()
    {
        return buttonStates[(int)AxisAsButton.PAD_DOWN] == ButtonState.DOWN;
    }

    public bool GetPadDown()
    {
        if (isXbox)
        {
            return Input.GetAxis("XBOX_Pad_Vertical") == -1;
        }
        else
        {
            return Input.GetAxis("PS4_D_Y") == -1;
        }
    }

    public bool GetPadRight()
    {
        if (isXbox)
        {
            return Input.GetAxis("XBOX_Pad_Horizontal") == 1;
        }
        else
        {
            return Input.GetAxis("PS4_D_X") == 1;
        }
    }

    public bool GetPadLeft()
    {
        if (isXbox)
        {
            return Input.GetAxis("XBOX_Pad_Horizontal") == -1;
        }
        else
        {
            return Input.GetAxis("PS4_D_X") == -1;
        }
    }

    /* PS4 buttons */

    public bool GetPS4OptionsDown()
    {
        if (isXbox)
        {
            return Input.GetButtonDown("XBOX_Start");
        }
        else
        {
            return Input.GetButtonDown("PS4_Options");
        }
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

    private void CheckDevice()
    {
        isXbox = false;
        isPS4 = false;
        string[] joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (joystickNames[i].Contains("360") || joystickNames[i].Contains("XBOX") || joystickNames[i].Contains("Xbox"))
            {
                if (!isXbox)
                    NotifyAll();
                isXbox = true;
                if(errorMessage.activeSelf)
                    errorMessage.SetActive(false);
                return;
            }
            else if (joystickNames[i].Contains("Wireless Controller"))
            {
                if (!isPS4)
                    NotifyAll();
                isPS4 = true;
                if (errorMessage.activeSelf)
                    errorMessage.SetActive(false);
                return;
            }
        }
        if (stopGameIfNoController)
        {
            if(GameManager.instance != null) GameManager.instance.OnGamePaused();
            errorMessage.SetActive(true);
        }

    }

    #endregion
}