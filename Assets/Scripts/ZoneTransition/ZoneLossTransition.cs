using UnityEngine;

public class ZoneLossTransition : MonoBehaviour
{
    public delegate void VoidCallback();

    #region Fields
    [SerializeField]
    private CinematicStripes cinematicStripes;
    [SerializeField]
    [Tooltip("GameObjects that will be deactivated for the duration of the Transition.")]
    private GameObject[] objectsToHide;

    [SerializeField]
    private ScriptedAnimation[] scriptedAnimations;

    private bool inTransition = false;
    private int currentAnimationIndex = -1;
    private VoidCallback endCallback = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(cinematicStripes, "ERROR: Cinematic Stripes (CinematicStripes) not assigned for ZoneLossTransition script in GameObject " + gameObject.name);
    }
    #endregion

    #region Public Methods
    public void StartTransition(VoidCallback voidCallback)
    {
        endCallback = voidCallback;
        inTransition = true;
        StartAnimationChain();
    }

    public bool InTransition()
    {
        return inTransition;
    }
    #endregion

    #region Private Methods
    private void StartAnimationChain()
    {
        currentAnimationIndex = -1;
        if (HasAnimations())
        {
            cinematicStripes.ShowAnimated();
            SetObjectsToHideActiveState(false);
            StartNextAnimation();
        }
        else
        {
            OnTransitionFinished();
        }
    }

    private void StartNextAnimation()
    {
        ++currentAnimationIndex;

        if (currentAnimationIndex < scriptedAnimations.Length)
        {
            scriptedAnimations[currentAnimationIndex].StartAnimation(StartNextAnimation);
        }
        else
        {
            OnTransitionFinished();
        }
    }

    private void OnTransitionFinished()
    {
        if (HasAnimations())
        {
            cinematicStripes.HideAnimated();
            SetObjectsToHideActiveState(true);
        }
        inTransition = false;
        VoidCallback callback = endCallback;
        endCallback = null;
        callback();
    }

    private bool HasAnimations()
    {
        return scriptedAnimations != null && scriptedAnimations.Length > 0;
    }

    private void SetObjectsToHideActiveState(bool activeState)
    {
        if (objectsToHide != null)
        {
            foreach (GameObject go in objectsToHide)
            {
                go.SetActive(activeState);
            }
        }
    }
    #endregion
}
