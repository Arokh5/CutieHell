using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvasController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject healthBarCanvas;
    [SerializeField]
    private float damageActiveTime;
    [SerializeField]
    private float targetActiveTime;
    private Image healthImage;
    private float baseHealth;
    private float time;

	#endregion
	
	#region MonoBehaviour Methods

    private void Awake()
    {
        healthImage = healthBarCanvas.transform.GetChild(0).GetComponent<Image>();
        baseHealth = GetComponent<AIEnemy>().baseHealth;
    }

    private void Update()
    {
        healthBarCanvas.GetComponent<RectTransform>().LookAt(GameManager.instance.GetPlayer1().transform);

        if (healthBarCanvas.activeSelf)
        {
            DisableHealthBar();
        }
    }

	#endregion
	
	#region Public Methods
	
    public void SetHealthBar()
    {
        healthImage.fillAmount = GetComponent<AIEnemy>().GetCurrentHealth() / baseHealth;
    }

    public void EnableHealthBar(bool takeDamage)
    {
        healthBarCanvas.SetActive(true);

        if (takeDamage || (!takeDamage && time <= targetActiveTime))
        {
            time = takeDamage ? damageActiveTime : targetActiveTime;
        }
    }

    #endregion

    #region Private Methods

    private void DisableHealthBar()
    {
        time -= Time.deltaTime;

        if (time <= 0f)
        {
            healthBarCanvas.SetActive(false);
        }
    }

    #endregion
}