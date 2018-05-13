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
    [SerializeField]
    private float fadeOutBaseTime;
    private float fadeOutTime;
    private Image healthImage;
    private float baseHealth;
    private float time;
    private bool fadeOut;
    private Color transparent;

	#endregion
	
	#region MonoBehaviour Methods

    private void Awake()
    {
        fadeOut = false;
        fadeOutTime = fadeOutBaseTime;
        healthImage = healthBarCanvas.transform.GetChild(0).GetComponent<Image>();
        baseHealth = GetComponent<AIEnemy>().baseHealth;
        transparent = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        healthBarCanvas.GetComponent<RectTransform>().LookAt(GameManager.instance.GetPlayer1().transform);

        if (healthBarCanvas.activeSelf && !fadeOut)
        {
            DisableHealthBar();
        }

        if (fadeOut)
        {
            HealthBarFadeOut();
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
        healthBarCanvas.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        healthBarCanvas.transform.GetChild(1).GetComponent<Image>().color = Color.white;

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
            fadeOut = true;
        }
    }

    private void HealthBarFadeOut()
    {
        fadeOutTime -= Time.deltaTime;
        healthBarCanvas.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.white, transparent, 1 - (fadeOutTime / fadeOutBaseTime));
        healthBarCanvas.transform.GetChild(1).GetComponent<Image>().color = Color.Lerp(Color.white, transparent, 1 - (fadeOutTime / fadeOutBaseTime));

        if (fadeOutTime <= 0)
        {
            fadeOut = false;
            fadeOutTime = fadeOutBaseTime;
            healthBarCanvas.SetActive(false);
        }
    }

    #endregion
}