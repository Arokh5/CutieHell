using UnityEngine;
using UnityEngine.UI;

public class Monument : Building
{

    #region Fields
    [Header("Monument health bar")]
    public bool isFirstEnemyTarget = false;
    [SerializeField]
    private string healthBarTitle = "UNNAMED";
    [SerializeField]
    private Monument protectedMonument;

    private MonumentsHealthBar healthBar;
    private MinimapElement minimapElement;
    #endregion

    #region MonoBehaviour Methods
    private new void Awake()
    {
        base.Awake();
        minimapElement = GetComponent<MinimapElement>();
    }

    private new void Start()
    {
        base.Start();
        healthBar = UIManager.instance.monumentsHealthBar;
        if (isFirstEnemyTarget)
        {
            SetUpHealthBar();
            UIManager.instance.markersController.MonumentTargetted(zoneController.iconIndex);
        }
    }
    #endregion

    #region Public Methods
    // IDamageable
    public override void TakeDamage(float damage, AttackType attacktype)
    {
        base.TakeDamage(damage, attacktype);

        if (minimapElement)
            minimapElement.RequestEffect();

        if (!IsDead())
            healthBar.SetHealthBarFill(currentHealth / baseHealth);

        float normalizedDamage = (baseHealth - currentHealth) / baseHealth;
        zoneController.InformMonumentDamage(normalizedDamage);
    }

    public void SetUpHealthBar()
    {
        healthBar.SetHealthBarTitle(healthBarTitle);
        healthBar.RefillHealthBar(!isFirstEnemyTarget);
    }

    public override void BuildingKilled()
    {
        healthBar.SetHealthBarFill(0.0f);
        zoneController.OnMonumentTaken();
        if (protectedMonument)
        {
            protectedMonument.SetUpHealthBar();
            int protectedMonumentIndex = protectedMonument.zoneController.iconIndex;
            UIManager.instance.markersController.MonumentTargetted(protectedMonumentIndex);
        }
    }

    public override void BuildingConverted()
    {
        
    }
    #endregion
}