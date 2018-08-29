using UnityEngine;

public class GameObjectActivation : ScriptedAnimation
{
    #region
    [SerializeField]
    [Tooltip("The array of GameObjects to be activated or deactivated depending on the ActiveState field")]
    private GameObject[] gameObjects;
    [SerializeField]
    [Tooltip("Indicates whether the GameObjects should be activated or deactivated")]
    private bool activeState;
    #endregion

    #region Protected Methods
    protected override void StartAnimationInternal()
    {
        if (gameObjects != null)
        {
            foreach (GameObject go in gameObjects)
                go.SetActive(activeState);
        }

        OnAnimationFinished();
    }
    #endregion
}
