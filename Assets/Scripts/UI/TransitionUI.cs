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
    private Image background;
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private string[] texts;
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

    private TransitionState transitionState;
     
    #endregion

    #region Monobehaviour methods

    #endregion

    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isTransitionOn)
        {
            switch(transitionState)
            {
                case (TransitionState.TRANSITION_START):
                    elementToTransition.transform.position = transforms[0].position;
                    elementToTransition.transform.localScale = transforms[0].localScale;
                    background.color = new Color(background.color.r, background.color.g, background.color.b, colors[0].a);
                    images[0].color = new Color(images[0].color.r, images[0].color.g, images[0].color.b, colors[0].a);
                    // inicializar posiciones etc
                    break;
                case (TransitionState.TRANSITION_ONGOING):
                    // transición
                    break;
                case (TransitionState.TRANSITION_FINISH):
                    // desinicializar
                    break;
            }
                
        }
	}
	
	#region Public methods
	public void AskForTransition(string name, Image image)
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
            images[0] = image;
            texts[0] = name;
        }
    }
	#endregion
	
	#region Private methods
	
	#endregion
}
