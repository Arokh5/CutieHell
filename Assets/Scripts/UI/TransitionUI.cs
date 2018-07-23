using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TransitionState {TRANSITION_START, TRANSITION_ONGOING, TRANSITION_FINISH}
public class TransitionUI : MonoBehaviour {
    public static TransitionUI instance;

    #region Attributes
    [SerializeField]
    private GameObject elementToTransition;
    [SerializeField]
    private Image elementBackground;
    [SerializeField]
    private Image elementIcon;
    [SerializeField]
    private Text elementText;
    [Header("Positions")]
    [SerializeField]
    private Transform[] transforms;
    [Header("Colors")]
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private float[] times;
    

    private bool isTransitionOn = false;
    private Queue queueImages = new Queue();
    private Queue queueNames = new Queue();

    private float elapsedTime;
    private float elapsedColor;

    private TransitionState transitionState;

    #endregion

    #region Monobehaviour methods

    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () 
	{
		if(isTransitionOn)
        {
            switch(transitionState)
            {
                case (TransitionState.TRANSITION_START):
                    elementBackground.transform.position = transforms[0].position;
                    elapsedTime += Time.deltaTime * times[0];
                    elapsedColor = Mathf.Lerp(elementBackground.color.a, colors[0].a, elapsedTime);

                    elementBackground.transform.localScale = Vector3.Lerp(elementBackground.transform.localScale, transforms[0].localScale, elapsedTime);

                    elementBackground.color = new Color(elementBackground.color.r, elementBackground.color.g, elementBackground.color.b, elapsedColor);
                    elementIcon.color = new Color(elementIcon.color.r, elementIcon.color.g, elementIcon.color.b, elapsedColor);
                    elementText.color = new Color(elementText.color.r, elementText.color.g, elementText.color.b, elapsedColor);

                    if (elapsedTime >= times[0])
                    {
                        transitionState = TransitionState.TRANSITION_ONGOING;
                        elapsedTime = 0;
                    }
                    break;
                case (TransitionState.TRANSITION_ONGOING):
                    elapsedTime += Time.deltaTime * times[1];
                    elapsedColor = Mathf.Lerp(elementBackground.color.a, colors[1].a, elapsedTime);

                    elementBackground.transform.localScale = Vector3.Lerp(elementBackground.transform.localScale, transforms[1].localScale, elapsedTime);
                    elementBackground.transform.position = Vector3.Lerp(elementBackground.transform.position, transforms[1].position, elapsedTime);

                    elementBackground.color = new Color(elementBackground.color.r, elementBackground.color.g, elementBackground.color.b, elapsedColor);
                    elementIcon.color = new Color(elementIcon.color.r, elementIcon.color.g, elementIcon.color.b, elapsedColor);
                    elementText.color = new Color(elementText.color.r, elementText.color.g, elementText.color.b, elapsedColor);

                    if (elapsedTime >= times[0])
                    {
                        transitionState = TransitionState.TRANSITION_FINISH;
                        elapsedTime = 0;
                    }
                    break;
                case (TransitionState.TRANSITION_FINISH):
                    elapsedTime += Time.deltaTime * times[1];
                    elapsedColor = Mathf.Lerp(elementBackground.color.a, colors[2].a, elapsedTime);

                    elementBackground.color = new Color(elementBackground.color.r, elementBackground.color.g, elementBackground.color.b, elapsedColor);
                    elementIcon.color = new Color(elementIcon.color.r, elementIcon.color.g, elementIcon.color.b, elapsedColor);
                    elementText.color = new Color(elementText.color.r, elementText.color.g, elementText.color.b, elapsedColor);

                    if (elapsedTime >= times[1])
                    {
                        isTransitionOn = false;
                        elapsedTime = 0;

                        //In case these didn't get to be full transparent
                        elementBackground.color = new Color(elementBackground.color.r, elementBackground.color.g, elementBackground.color.b, colors[2].a);
                        elementIcon.color = new Color(elementIcon.color.r, elementIcon.color.g, elementIcon.color.b, colors[2].a);
                        elementText.color = new Color(elementText.color.r, elementText.color.g, elementText.color.b, colors[2].a);
                    }
                    // desinicializar
                    break;
            }                
        }
        else
        {
            if(queueImages.Count !=0 && queueNames.Count != 0)
            {
                elementIcon.sprite = (Sprite) queueImages.Dequeue();
                elementText.text = (string)queueNames.Dequeue();
                isTransitionOn = true;
                transitionState = TransitionState.TRANSITION_START;
            }
        }
	}
	
	#region Public methods
	public void AskForTransition(string name, Sprite image)
    {
        if(isTransitionOn)
        {
            queueImages.Enqueue(image);
            queueNames.Enqueue(name);
        }
        else
        {
            isTransitionOn = true;
            transitionState = TransitionState.TRANSITION_START;
            elementIcon.sprite = image;
            elementText.text = name;
        }
    }
	#endregion
	
	#region Private methods
	
	#endregion
}
