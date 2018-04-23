using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields

    public static UIManager instance;
    public enum ComboTypes { None, StrongCombo, BadCombo };
    public ComboTypes activeCombo;

    public UICompass compass;
    [SerializeField]
    private GameObject evilnessBar;

    private int currentWaveNumber = -1;
    [SerializeField]
    private Text waveNumberText;

    [SerializeField]
    private WaveTimer waveRadialProgressBar;

    [SerializeField]
    private GameObject strongCombo;
    private Vector3 strongComboOriginalScale;
    private Color strongComboOriginalColor;
    [SerializeField]
    private GameObject badCombo;
    private Vector3 badComboOriginalScale;
    private Color badComboOriginalColor;

    [Header("Repair and Use panels")]
    public Color lockedPanelTintColor = Color.red;
    [SerializeField]
    private float distanceBetweenPopUps = 500.0f;
    [SerializeField]
    private Image repairText;
    [SerializeField]
    private Image useText;
        
    private float strongComboscaleModifier;
    private float strongComboColorModifier;
    [SerializeField]
    private float strongComboScaleVelocity;
    [SerializeField]
    private float fadeOutStrongComboVelocity;

    private float badComboscaleModifier;
    private float badComboColorModifier;
    [SerializeField]
    private float badComboScaleVelocity;
    [SerializeField]
    private float fadeOutBadComboVelocity;

    [SerializeField]
    private Text basicEnemies;
    [SerializeField]
    private Text rangeEnemies;
    [SerializeField]
    private Text conquerorEnemies;

    [SerializeField]
    private Text waveEndText;
    [SerializeField]
    private Text endBtnText;

    [SerializeField]
    private AudioClip coinEfx;

    private const float enemiesCountVel = 0.15f;

    private float basicEnemiesPrevTimeCount;
    private float basicEnemiesTimeCount;
    private int basicEnemiesCounter;

    private float rangeEnemiesPrevTimeCount;
    private float rangeEnemiesTimeCount;
    private int rangeEnemiesCounter;

    private float conquerorEnemiesPrevTimeCount;
    private float conquerorEnemiesTimeCount;
    private int conquerorEnemiesCounter;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        instance = this;
        activeCombo = ComboTypes.None;
        strongComboOriginalScale = strongCombo.transform.localScale;
        strongComboOriginalColor = strongCombo.GetComponent<Text>().color;
        badComboOriginalScale = badCombo.transform.localScale;
        badComboOriginalColor = badCombo.GetComponent<Text>().color;
        strongComboscaleModifier = 0f;
        strongComboColorModifier = 0f;
        badComboscaleModifier = 0f;
        badComboColorModifier = 0f;
        ResetEnemiesCounters();
    }

    private void Update()
    {
        if (activeCombo != ComboTypes.None)
        {
            ComboFx();
        }
    }

    #endregion

    #region Public Methods

    public void ChangeWaveEndText(string text)
    {
        waveEndText.text = text;
    }

    public void ChangeEndBtnText(string text)
    {
        endBtnText.text = text;
    }

    // Called by Player when using or earning Evil Points
    public void SetEvilBarValue(int value)
    {
        evilnessBar.GetComponent<Image>().fillAmount = ((float)value / GameManager.instance.GetPlayer1().GetMaxEvilLevel());
    }

    // Called by Player when increasing its maximum of Evil Point
    public void SetEvilBarMaxValue(int maxValue)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::SetEvilBarMaxValue");
    }

    // Called by ZonesConnection when the connection gets opened
    public void ZoneConnectionOpened(int zoneConnectionID)
    {
        Debug.LogError("NOT IMPLEMENTED:UIManager::ZoneConnectionOpened");
    }

    // Called by AISpawnController to move the Wave indicator forward
    public void SetWaveNumberAndProgress(int waveNumber, float normalizedProgress)
    {
        if (currentWaveNumber != waveNumber)
        {
            currentWaveNumber = waveNumber;
            waveNumberText.text = currentWaveNumber.ToString();
        }
        waveRadialProgressBar.SetNormalizedAmount(normalizedProgress);
    }

    public void ShowRepairText()
    {
        repairText.color = Color.white;
        repairText.gameObject.SetActive(true);
        UpdatePopUpsPosition();
    }

    public void ShowLockedRepairText()
    {
        repairText.color = lockedPanelTintColor;
        repairText.gameObject.SetActive(true);
        UpdatePopUpsPosition();
    }

    public void HideRepairText()
    {
        repairText.gameObject.SetActive(false);
        UpdatePopUpsPosition();
    }

    public void ShowUseText()
    {
        useText.color = Color.white;
        useText.gameObject.SetActive(true);
        UpdatePopUpsPosition();
    }

    public void ShowLockedUseText()
    {
        useText.color = lockedPanelTintColor;
        useText.gameObject.SetActive(true);
        UpdatePopUpsPosition();
    }

    public void HideUseText()
    {
        useText.gameObject.SetActive(false);
        UpdatePopUpsPosition();
    }

    public void ShowComboText(ComboTypes comboType)
    {
        switch (comboType)
        {
            case ComboTypes.StrongCombo:
                strongCombo.SetActive(true);
                activeCombo = ComboTypes.StrongCombo;
                break;

            case ComboTypes.BadCombo:
                badCombo.SetActive(true);
                activeCombo = ComboTypes.BadCombo;
                break;
        }
    }

    public void SetEnemiesKilledCount()
    {
        if (basicEnemiesTimeCount - basicEnemiesPrevTimeCount >= enemiesCountVel)
        {
            if (basicEnemiesCounter < StatsManager.instance.GetBasicEnemiesKilled())
            {
                SoundManager.instance.PlayEfxClip(coinEfx);
                basicEnemiesCounter++;
                basicEnemies.text = "Basic: " + basicEnemiesCounter.ToString();
                basicEnemiesPrevTimeCount = basicEnemiesTimeCount;
            }
        }

        if (rangeEnemiesTimeCount - rangeEnemiesPrevTimeCount >= enemiesCountVel)
        {
            if (rangeEnemiesCounter < StatsManager.instance.GetRangeEnemiesKilled())
            {
                SoundManager.instance.PlayEfxClip(coinEfx);
                rangeEnemiesCounter++;
                rangeEnemies.text = "Range: " + rangeEnemiesCounter.ToString();
                rangeEnemiesPrevTimeCount = rangeEnemiesTimeCount;
            }
        }

        if (conquerorEnemiesTimeCount - conquerorEnemiesPrevTimeCount >= enemiesCountVel)
        {
            if (conquerorEnemiesCounter < StatsManager.instance.GetConquerorEnemiesKilled())
            {
                SoundManager.instance.PlayEfxClip(coinEfx);
                conquerorEnemiesCounter++;
                conquerorEnemies.text = "Conqueror: " + conquerorEnemiesCounter.ToString();
                conquerorEnemiesPrevTimeCount = conquerorEnemiesTimeCount;
            }
        }
    }

    public void ResetEnemiesCounters()
    {
        basicEnemiesTimeCount = enemiesCountVel;
        basicEnemiesPrevTimeCount = 0f;
        basicEnemiesCounter = 0;

        rangeEnemiesTimeCount = enemiesCountVel;
        rangeEnemiesPrevTimeCount = 0f;
        rangeEnemiesCounter = 0;

        conquerorEnemiesTimeCount = enemiesCountVel;
        conquerorEnemiesPrevTimeCount = 0f;
        conquerorEnemiesCounter = 0;
    }

    public void IncreaseEnemiesTimeCount()
    {
        IncreaseBasicEnemiesTimeCount();
        IncreaseRangeEnemiesTimeCount();
        IncreaseConquerorEnemiesTimeCount();
    }

    #endregion

    #region Private Methods

    private void UpdatePopUpsPosition()
    {
        if (useText.IsActive() && repairText.IsActive())
        {
            SetLocalXPos(useText.rectTransform, -0.5f * distanceBetweenPopUps);
            SetLocalXPos(repairText.rectTransform, 0.5f * distanceBetweenPopUps);
        }
        else if (useText.IsActive())
            SetLocalXPos(useText.rectTransform, 0);
        else if (repairText.IsActive())
            SetLocalXPos(repairText.rectTransform, 0);
    }

    private void SetLocalXPos(RectTransform rectTransform, float xValue)
    {
        Vector3 refPos = rectTransform.localPosition;
        refPos.x = xValue;
        rectTransform.localPosition = refPos;
    }

    private void IncreaseBasicEnemiesTimeCount()
    {
        basicEnemiesTimeCount += Time.deltaTime;
    }

    private void IncreaseRangeEnemiesTimeCount()
    {
        rangeEnemiesTimeCount += Time.deltaTime;
    }

    private void IncreaseConquerorEnemiesTimeCount()
    {
        conquerorEnemiesTimeCount += Time.deltaTime;
    }

    private void ComboFx()
    {
        switch (activeCombo)
        {
            case ComboTypes.StrongCombo:
                if (strongCombo.transform.localScale.x < 0.75f)
                {
                    strongComboscaleModifier += Time.deltaTime;
                    strongCombo.transform.localScale = new Vector3(strongCombo.transform.localScale.x + strongComboscaleModifier * strongComboScaleVelocity, strongCombo.transform.localScale.y + strongComboscaleModifier * strongComboScaleVelocity, strongCombo.transform.localScale.z);
                }
                else
                {
                    strongComboColorModifier += Time.deltaTime;
                    strongCombo.GetComponent<Text>().color = new Color(strongComboOriginalColor.r, strongComboOriginalColor.g, strongComboOriginalColor.b, strongComboOriginalColor.a - strongComboColorModifier * fadeOutStrongComboVelocity);
                    
                    if (strongCombo.GetComponent<Text>().color.a <= 0)
                    {
                        strongCombo.transform.localScale = strongComboOriginalScale;
                        strongCombo.GetComponent<Text>().color = strongComboOriginalColor;
                        strongComboscaleModifier = 0f;
                        strongComboColorModifier = 0f;
                        activeCombo = ComboTypes.None;
                        strongCombo.SetActive(false);
                    }
                }

                break;

            case ComboTypes.BadCombo:
                if (badCombo.transform.localScale.x < 1f)
                {
                    badComboscaleModifier += Time.deltaTime;
                    badCombo.transform.localScale = new Vector3(badCombo.transform.localScale.x + badComboscaleModifier * badComboScaleVelocity, badCombo.transform.localScale.y + badComboscaleModifier * badComboScaleVelocity, badCombo.transform.localScale.z);
                }
                else
                {
                    badComboColorModifier += Time.deltaTime;
                    badCombo.GetComponent<Text>().color = new Color(badComboOriginalColor.r, badComboOriginalColor.g, badComboOriginalColor.b, badComboOriginalColor.a - badComboColorModifier * fadeOutBadComboVelocity);

                    if (badCombo.GetComponent<Text>().color.a <= 0)
                    {
                        badCombo.transform.localScale = badComboOriginalScale;
                        badCombo.GetComponent<Text>().color = badComboOriginalColor;
                        badComboscaleModifier = 0f;
                        badComboColorModifier = 0f;
                        activeCombo = ComboTypes.None;
                        badCombo.SetActive(false);
                    }
                }

                break;
        }
    }

    #endregion
}