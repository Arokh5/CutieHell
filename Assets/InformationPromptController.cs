using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPromptController : MonoBehaviour
{
    private enum AnimationState
    {
        HIDDEN,
        MOVE_IN,
        WAIT,
        MOVE_OUT
    }

    #region Fields
    public float animationDuration = 0.5f;
    [SerializeField]
    private Text infoText;
    
    private AnimationState state = AnimationState.HIDDEN;
    private float moveInTimer = 0;
    private float waitTimer = 0;
    private float moveOutTimer = 0;
    private float showYPos;
    private float hideYPos;
    private RectTransform rectTransform;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(infoText, "ERROR: The InformationPromptController in gameObject '" + gameObject.name + "' doesn't have a Text (infoText) assigned!");
        rectTransform = GetComponent<RectTransform>();
        UnityEngine.Assertions.Assert.IsNotNull(rectTransform, "ERROR: A RectTransform Component could not be found by InformationPromptController in GameObject " + gameObject.name);

        showYPos = rectTransform.anchoredPosition.y;
        hideYPos = -rectTransform.sizeDelta.y;

        Move(hideYPos, hideYPos, 0);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (state)
        {
            case AnimationState.HIDDEN:
                break;
            case AnimationState.MOVE_IN:
                MoveIn();
                break;
            case AnimationState.WAIT:
                Wait();
                break;
            case AnimationState.MOVE_OUT:
                MoveOut();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Public Methods
    public void ShowPrompt(string content, float duration)
    {
        SetPromptText(content);
        switch (state)
        {
            case AnimationState.HIDDEN:
                gameObject.SetActive(true);
                state = AnimationState.MOVE_IN;
                moveInTimer = animationDuration;
                break;
            case AnimationState.MOVE_OUT:
                state = AnimationState.MOVE_IN;
                moveInTimer = animationDuration - moveOutTimer;
                break;
            default:
                break;
        }
        waitTimer = duration;
        moveOutTimer = animationDuration;
    }
    #endregion

    #region Private Methods
    private void SetPromptText(string info)
    {
        infoText.text = info;
    }

    private void MoveIn()
    {
        moveInTimer -= Time.deltaTime;
        if (moveInTimer < 0)
            moveInTimer = 0;

        float u = (animationDuration - moveInTimer) / animationDuration;
        Move(hideYPos, showYPos, u);

        if (moveInTimer == 0)
            state = AnimationState.WAIT;
    }

    private void Wait()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
            state = AnimationState.MOVE_OUT;
    }

    private void MoveOut()
    {
        moveOutTimer -= Time.deltaTime;
        if (moveOutTimer < 0)
            moveOutTimer = 0;

        float u = (animationDuration - moveOutTimer) / animationDuration;
        Move(showYPos, hideYPos, u);

        if (moveOutTimer == 0)
        {
            gameObject.SetActive(false);
            state = AnimationState.HIDDEN;
        }
    }

    private void Move(float startY, float endY, float normalizedProgress)
    {
        Vector2 currentPos = rectTransform.anchoredPosition;
        currentPos.y = startY * (1 - normalizedProgress) + endY * normalizedProgress;
        rectTransform.anchoredPosition = currentPos;
    }
    #endregion
}
