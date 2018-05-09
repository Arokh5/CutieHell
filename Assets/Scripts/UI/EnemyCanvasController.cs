using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvasController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject healthBarCanvas;
    private Image healthImage;
    private float baseHealth;

	#endregion
	
	#region MonoBehaviour Methods

    private void Start()
    {
        healthImage = healthBarCanvas.transform.GetChild(0).GetComponent<Image>();
        baseHealth = GetComponent<AIEnemy>().baseHealth;
    }

    private void Update()
    {
        transform.LookAt(GameManager.instance.GetPlayer1().transform);
    }

	#endregion
	
	#region Public Methods
	
    public void SetHealthBar()
    {
        healthImage.fillAmount = GetComponent<AIEnemy>().GetCurrentHealth() / baseHealth;
    }

	#endregion
}