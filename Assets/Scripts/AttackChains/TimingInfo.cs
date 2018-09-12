using UnityEngine;

[System.Serializable]
public struct TimingInfo
{
    [Tooltip("The time after the activation of the chain in which the button icon in alert state gets shown.")]
    public float alert;
    [Tooltip("The time after the activation of the chain in which the button icon switched to active state (pushing it triggers the follow up).")]
    public float start;
    [Tooltip("The time after the activation of the chain in which the button icon is hidden and the chain is discarded.")]
    public float end;
}
