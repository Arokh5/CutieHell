using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvasController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private RectTransform healthBarCanvas;
    [SerializeField]
    private float damageActiveTime;
    [SerializeField]
    private float targetActiveTime;
    [SerializeField]
    private float fadeOutBaseTime;
    private float fadeOutTime;
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Image healthContainer;
    private float baseHealth;
    private float time;
    private bool fadeOut;
    private Color transparent;
    private AIEnemy enemyScript;

	#endregion
	
	#region MonoBehaviour Methods

    private void Awake()
    {
        fadeOut = false;
        fadeOutTime = fadeOutBaseTime;
        enemyScript = GetComponent<AIEnemy>();
        baseHealth = enemyScript.baseHealth;
        transparent = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        healthBarCanvas.LookAt(Camera.main.transform);

        if (healthBarCanvas.gameObject.activeSelf && !fadeOut)
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
        healthImage.fillAmount = enemyScript.GetCurrentHealth() / baseHealth;
    }

    public void EnableHealthBar(bool takeDamage)
    {
        healthBarCanvas.gameObject.SetActive(true);
        healthImage.color = Color.white;
        healthContainer.color = Color.white;

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
        healthImage.color = Color.Lerp(Color.white, transparent, 1 - (fadeOutTime / fadeOutBaseTime));
        healthContainer.color = Color.Lerp(Color.white, transparent, 1 - (fadeOutTime / fadeOutBaseTime));

        if (fadeOutTime <= 0)
        {
            fadeOut = false;
            fadeOutTime = fadeOutBaseTime;
            healthBarCanvas.gameObject.SetActive(false);
        }
    }

    #endregion
}